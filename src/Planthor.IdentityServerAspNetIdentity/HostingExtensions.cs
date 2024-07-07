using Planthor.IdentityServerAspNetIdentity.Data;
using Planthor.IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Duende.IdentityServer;
using Microsoft.IdentityModel.Protocols.Configuration;

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

        builder.Services
            .AddIdentityServer(identityServerOptions =>
            {
                identityServerOptions.Events.RaiseErrorEvents = true;
                identityServerOptions.Events.RaiseInformationEvents = true;
                identityServerOptions.Events.RaiseFailureEvents = true;
                identityServerOptions.Events.RaiseSuccessEvents = true;

                // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
                identityServerOptions.EmitStaticAudienceClaim = true;
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

        builder
            .Services
            .AddAuthentication()
            .AddFacebook(facebookOptions =>
            {
                facebookOptions.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                facebookOptions.AppId = builder.Configuration["Authentication:Facebook:AppId"] ?? throw new InvalidConfigurationException("Missing third party configuration");
                facebookOptions.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"] ?? throw new InvalidConfigurationException("Missing third party configuration");
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

        app.MapRazorPages()
            .RequireAuthorization();

        return app;
    }
}
