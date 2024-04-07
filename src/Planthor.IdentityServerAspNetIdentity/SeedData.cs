using System.Security.Claims;
using IdentityModel;
using Planthor.IdentityServerAspNetIdentity.Data;
using Planthor.IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;

namespace Planthor.IdentityServerAspNetIdentity;

public static class SeedData
{
    public static async Task EnsureSeedDataAsync(IHost app)
    {
        ArgumentNullException.ThrowIfNull(app);
        using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        applicationDbContext.Database.EnsureDeleted();
        applicationDbContext.Database.Migrate();

        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var alice = await userMgr.FindByNameAsync("alice");
        if (alice == null)
        {
            alice = new ApplicationUser
            {
                UserName = "alice",
                Email = "AliceSmith@email.com",
                EmailConfirmed = true,
            };
            var result = await userMgr.CreateAsync(alice, "Planthor@123");
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(result.Errors.First().Description);
            }

            result = await userMgr.AddClaimsAsync(
                alice,
                [
                    new Claim(JwtClaimTypes.Name, "Alice Smith"),
                    new Claim(JwtClaimTypes.GivenName, "Alice"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                ]);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(result.Errors.First().Description);
            }

            Log.Debug("alice created");
        }
        else
        {
            Log.Debug("alice already exists");
        }

        var bob = await userMgr.FindByNameAsync("bob");
        if (bob == null)
        {
            bob = new ApplicationUser
            {
                UserName = "bob",
                Email = "BobSmith@email.com",
                EmailConfirmed = true
            };
            var result = await userMgr.CreateAsync(bob, "Planthor@123");
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(result.Errors.First().Description);
            }

            result = await userMgr.AddClaimsAsync(
                bob,
                [
                    new Claim(JwtClaimTypes.Name, "Bob Smith"),
                    new Claim(JwtClaimTypes.GivenName, "Bob"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                    new Claim("location", "somewhere")
                ]);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(result.Errors.First().Description);
            }
            Log.Debug("bob created");
        }
        else
        {
            Log.Debug("bob already exists");
        }

        scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

        var configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
        configurationDbContext.Database.Migrate();
        if (!configurationDbContext.Clients.Any())
        {
            foreach (var client in Config.Clients)
            {
                configurationDbContext.Clients.Add(client.ToEntity());
            }
            configurationDbContext.SaveChanges();
        }

        if (!configurationDbContext.IdentityResources.Any())
        {
            foreach (var resource in Config.IdentityResources)
            {
                configurationDbContext.IdentityResources.Add(resource.ToEntity());
            }
            configurationDbContext.SaveChanges();
        }

        if (!configurationDbContext.ApiScopes.Any())
        {
            foreach (var resource in Config.ApiScopes)
            {
                configurationDbContext.ApiScopes.Add(resource.ToEntity());
            }
            configurationDbContext.SaveChanges();
        }
    }
}
