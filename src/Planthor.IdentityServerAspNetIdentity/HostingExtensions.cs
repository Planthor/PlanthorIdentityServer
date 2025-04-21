using Planthor.IdentityServerAspNetIdentity.Data;
using Planthor.IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Duende.IdentityServer;
using Microsoft.IdentityModel.Protocols.Configuration;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;

namespace Planthor.IdentityServerAspNetIdentity;

public static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorPages();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        var migrationsAssembly = typeof(Program).Assembly.GetName().Name;

        var keyVaultName = "planthor-keyvault";
        var kvUri = $"https://{keyVaultName}.vault.azure.net";
        var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());

        const string DuendeLicenseName = "duende-comm-key";
        var kvDuendeLicense = client.GetSecret(DuendeLicenseName);
        Console.WriteLine($"Your secret is '{kvDuendeLicense.Value.Value}'.");

        builder.Services
            .AddIdentityServer(identityServerOptions =>
            {
                identityServerOptions.Events.RaiseErrorEvents = true;
                identityServerOptions.Events.RaiseInformationEvents = true;
                identityServerOptions.Events.RaiseFailureEvents = true;
                identityServerOptions.Events.RaiseSuccessEvents = true;

                identityServerOptions.Authentication.CookieLifetime = TimeSpan.FromHours(1);
                identityServerOptions.Authentication.CookieSlidingExpiration = false;

                identityServerOptions.UserInteraction.LoginUrl = "/Account/Login";

                // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
                identityServerOptions.LicenseKey = kvDuendeLicense.Value.Value;
            })
            .AddConfigurationStore(storeOptions =>
            {
                storeOptions.ConfigureDbContext = contextOptions =>
                {
                    contextOptions.UseNpgsql(
                        builder.Configuration.GetConnectionString("DefaultConnection"),
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                };
            })
            .AddOperationalStore(storeOption =>
            {
                storeOption.ConfigureDbContext = contextOptions =>
                {
                    contextOptions.UseNpgsql(
                        builder.Configuration.GetConnectionString("DefaultConnection"),
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                };
            })
            .AddAspNetIdentity<ApplicationUser>();

        builder.Services
            .ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/path/to/login/for/aspnet_identity";
            });

        const string FBAppIdName = "fb-app-id";
        var kvFBAppId = client.GetSecret(FBAppIdName);
        Console.WriteLine($"Your secret is '{kvFBAppId.Value.Value}'.");

        const string FBAppSecretName = "fb-app-secret";
        var kvFBSecretId = client.GetSecret(FBAppSecretName);
        Console.WriteLine($"Your secret is '{kvFBSecretId.Value.Value}'.");

        builder
            .Services
            .AddAuthentication()
            .AddFacebook(facebookOptions =>
            {
                facebookOptions.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                facebookOptions.AppId = kvFBAppId.Value.ToString() ?? throw new InvalidConfigurationException("Missing third party configuration");
                facebookOptions.AppSecret = kvFBSecretId.Value.ToString() ?? throw new InvalidConfigurationException("Missing third party configuration");
                facebookOptions.AccessDeniedPath = "/AccessDeniedPathInfo";
            });

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.UseIdentityServer();
        app.UseAuthorization();

        app.MapRazorPages().RequireAuthorization();

        return app;
    }
}
