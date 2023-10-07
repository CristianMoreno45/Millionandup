using IdentityServer4;
using IdentityServer4.Models;
using static IdentityServer4.IdentityServerConstants;

namespace Millionandup.MsIdentityServer
{
    /// <summary>
    /// Class that loads the initial information of the IS4.
    /// </summary>
    public class Config
    {

        public static IEnumerable<ApiScope> ApiScopes()
        {
            return new List<ApiScope> {
                 new ApiScope("Owner.Add"                           ,"That allows adding an Owner"),
                 new ApiScope("Owner.Get"                           , "That allows you to get an Owner"),
                 new ApiScope("Property.CreatePropertyBuilding"     , "That allows you to create a property"),
                 new ApiScope("Property.AddImageFromProperty"       , "That allows you to add an image to a property"),
                 new ApiScope("Property.ChangePrice"                , "That allows you to change the price of a property."),
                 new ApiScope("Property.UpdateProperty"             , "That allows you to change the data of a property."),
                 new ApiScope("Property.ListPropertyWithFilters"    , "That allows extracting property information according to a filter"),
                 new ApiScope(LocalApi.ScopeName, "Identity server Users service"), // Enable IS4Endpoints 

            };
        }
        public static IEnumerable<ApiResource> ApiResources()
        {
            return new List<ApiResource> {
                 new ApiResource(LocalApi.ScopeName, "Identity server Users service"), // Enable IS4Endpoints
            };
        }
        public static IEnumerable<Client> Clients(string identityServerHost)
        {
            return new List<Client>
            {
                // machine to machine client (AUTH 2.0)
                new Client
                {
                    ClientId = "2b120e8f-817a-484a-9095-0065addbdfff",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = {
                        new Secret("1873aa7d-cdd0-4ce9-a94b-e6799b1e48f7".Sha256())
                    },
                    AllowedScopes = {
                        "Owner.Add",
                        "Owner.Get",
                        "Property.CreatePropertyBuilding",
                        "Property.AddImageFromProperty",
                        "Property.ChangePrice",
                        "Property.UpdateProperty",
                        "Property.ListPropertyWithFilters",
                        LocalApi.ScopeName // Enable IS4Endpoints
                    }
                },
                new Client
                {
                    ClientId = "b4ad0b50-9160-4896-99ed-c7a3f4564174",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = {
                        new Secret("1253332b-71be-4634-b9d9-f24e87beaa95".Sha256())
                    },
                    AllowedScopes = {
                        "Owner.Add",
                        "Owner.Get",
                        "Property.CreatePropertyBuilding",
                        "Property.AddImageFromProperty",
                        "Property.ChangePrice",
                        "Property.UpdateProperty",
                        "Property.ListPropertyWithFilters",
                         LocalApi.ScopeName // Enable IS4Endpoints
                    }
                },

                // interactive ASP.NET Core MVC client (OpenId)
                new Client
                {
                    ClientId = "mvc",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    // where to redirect to after login
                    RedirectUris = {$"https://{identityServerHost}/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "https://{identityServerHost}/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "AttributeScope"
                    }
                }
            };
        }
        public static IEnumerable<IdentityResource> IdentityResources()
        {
            return new List<IdentityResource>
            {
            // Built-in resources for OpenId
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
            };
        }

    }
}
