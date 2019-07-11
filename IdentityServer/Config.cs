// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer
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
            return new List<Client> {
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
                }


            };
        }


        // Kullanıcı ekleme
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
    {
        new TestUser
        {
            SubjectId = "1",
            Username = "admin",
            Password = "1234",
            // Bu özellik token içersine ek claim eklemek için kullanılır.
            Claims = new []
            {
                new Claim("name", "Murat"),
                new Claim("website", "https://guvenmurat.com")
            }
        },
        new TestUser
        {
            SubjectId = "2",
            Username = "user",
            Password = "1234",
            Claims = new []
            {
                new Claim("name", "User Test"),
                new Claim("website", "https://alice.com")
            }
        }
    };
        }
    }
}