﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OpenIdClientMvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);



            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            // BU KISIM OPENID HYBRID MODEL İLE IDENTITY SERVER KONFIGURASYONU
            // OR DOC ADRESS: http://docs.identityserver.io/en/latest/quickstarts/5_hybrid_and_api_access.html
              .AddOpenIdConnect("oidc", options => {
                  options.SignInScheme = "Cookies";
                  options.Authority = "http://localhost:5000";
                  options.RequireHttpsMetadata = false;

                  options.ClientId = "openidHybridClient";
                  options.ClientSecret = "secret";
                  options.ResponseType = "code id_token";

                  options.SaveTokens = true;
                  options.GetClaimsFromUserInfoEndpoint = true;

                  options.Scope.Add("catchApi");
                  options.Scope.Add("offline_access");
                  options.ClaimActions.MapJsonKey("website", "website");
              });

            //BU KISIM OPENID IMPLICIT ORNEĞİ İÇİN GEREKLİ KONFİGURASYON YAPISI
            // ORNEK DOC ADRESS: http://docs.identityserver.io/en/latest/quickstarts/3_interactive_login.html
            //.AddOpenIdConnect("oidc", options => {
            //    options.Authority = "http://localhost:5000";
            //    options.RequireHttpsMetadata = false;
            //    options.ClientId = "openidMvcClient";
            //    options.SaveTokens = true;
            //});


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseAuthentication();
            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
