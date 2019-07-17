// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServerAspNetIdentity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new ApiResource[] {
                new ApiResource("catchApi","Test catch IdentityServer4"),
                new ApiResource("roApi","Resource Owner Test Api")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                // client credentials flow client
                new Client
                {
                    ClientId ="catchClient",
                    ClientSecrets=
                    {
                        new Secret("secret".Sha256())
                    },
                    // Bu Kısım machine to machine için kullanılır.
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    AllowedScopes =
                    {
                        "catchApi"
                    }
                },
                  new Client
                {
                    ClientId ="roClient",
                    ClientSecrets=
                    {
                        new Secret("secret".Sha256())
                    },
                    // Bu Kısım machine to machine için kullanılır.
                    AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,
                    AllowedScopes =
                    {
                        "roApi"
                    }
                },


                        // OpenID Connect implicit flow client (MVC)
                new Client
                {
                    ClientId = "openidMvcClient",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.Implicit,

                    // where to redirect to after login
                    RedirectUris = { "http://localhost:5008/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://localhost:5008/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                },

                // Hybrid flow Example Client conf.

                new Client
                {
                    ClientId ="openidHybridClient",
                    ClientName ="MVC Client",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    // where to redirect to after login
                    RedirectUris = { "http://localhost:5008/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://localhost:5008/signout-callback-oidc" },
                     AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "catchApi"
                    },
                     AllowOfflineAccess = true
                },


                // JavaScript (Or SPA) Client Config

                new Client
                {
                    ClientId="js",
                    ClientName = "JavaScript Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris =           { "http://localhost:5003/callback.html" },
                    PostLogoutRedirectUris = { "http://localhost:5003/index.html" },
                    AllowedCorsOrigins =     { "http://localhost:5003" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "catchApi"
                    }

                }

            };
        }
    }
}