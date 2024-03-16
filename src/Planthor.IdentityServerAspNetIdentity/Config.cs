using Duende.IdentityServer.Models;
using static Duende.IdentityServer.IdentityServerConstants;

namespace Planthor.IdentityServerAspNetIdentity;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        [
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        ];

    public static IEnumerable<ApiScope> ApiScopes =>
        [
            new ApiScope("scope1"),
        ];

    // TODO: Trung: find a way to have UI to configure these client / seed these in db and configurable in IDP.
    public static IEnumerable<Client> Clients =>
        [
            new Client
            {
                ClientId = "planthor",
                ClientName = "Planthor Client",

                // TODO: Trung: find way to store secret securely.
                ClientSecrets = { new Secret("Planthor@123".Sha256()) },

                AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                AllowOfflineAccess = true,
                AllowedScopes = {
                    StandardScopes.OpenId,
                    StandardScopes.Profile },

                RedirectUris = {
                    "https://www.thunderclient.com/oauth/callback", // VS Code Thunder Client
                    "https://localhost:5173/api/auth/callback", // Local Planthor Web
                },
            }
        ];
}
