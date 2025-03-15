using Duende.IdentityServer.Models;
using static Duende.IdentityServer.IdentityServerConstants;

namespace Planthor.IdentityServerAspNetIdentity;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        [
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
            new IdentityResources.Phone(),
        ];

    public static IEnumerable<ApiScope> ApiScopes =>
        [
            new ApiScope(name: "tribe.read", displayName: "Read Planthor tribes."),
            new ApiScope(name: "tribe.full", displayName: "Fully control Planthor tribes."),
            new ApiScope(name: "personGoal.read", displayName: "Read Planthor personal goals."),
            new ApiScope(name: "personGoal.full", displayName: "Fully control Planthor goals."),
            new ApiScope(name: "member.read", displayName: "Read Planthor members."),
            new ApiScope(name: "member.full", displayName: "Fully control Planthor members."),

            new ApiScope(name: "admin", displayName: "Provide administrative access."),
        ];

    public static IEnumerable<ApiResource> ApiResources =>
        [
            new ApiResource(name: "planthorAPI", displayName: "Planthor API")
            {
                Scopes = {
                    "tribe.read",
                    "tribe.full",
                    "personGoal.read",
                    "personGoal.full",
                    "member.read",
                    "member.full",
                    "admin"
                },
            }
        ];

    // TODO: Trung: find a way to have UI to configure these client / seed these in db and configurable in IDP.
    public static IEnumerable<Client> Clients =>
        [
            // Thunder Client
            new Client {
                ClientId = "thunderClient",
                ClientName = "Planthor Thunder Client",

                // TODO: Trung: find way to store secret securely.
                ClientSecrets = { new Secret("Planthor@123".Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,

                AllowedScopes = {
                    "tribe.read",
                    "tribe.full",
                    "admin"
                },
            },

            // Svelte Planthor WebApp
            new Client {
                ClientId = "planthorWebApp",
                ClientName = "Planthor Web App",

                ClientSecrets = { new Secret("Planthor@123".Sha256()) },
                AllowedGrantTypes = GrantTypes.Code,

                AllowedScopes = {
                    "tribe.read",
                    "tribe.full",
                    "admin",
                    StandardScopes.OpenId,
                    StandardScopes.Profile,
                    StandardScopes.Email
                },

                RedirectUris = {
                    "https://localhost:5173/api/auth/callback", // Local Planthor Web https
                    "http://localhost:5173/api/auth/callback", // Local Planthor Web http
                },

                // where to redirect to after logout
                // TODO: Trung: clarify this
                PostLogoutRedirectUris = { "https://localhost:5173/signout-callback-oidc" },
            },
        ];
}
