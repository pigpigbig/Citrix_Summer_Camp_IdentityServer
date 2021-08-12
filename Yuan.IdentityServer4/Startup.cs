using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Yuan.IdentityServer4.Extendsions;

namespace Yuan.IdentityServer4
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            var builder = services.AddIdentityServer()
               .AddTestUsers(TestUsers.Users);// add some identity in your database,in this case we hard coded it
                                                // and it use ef to data work in practice

            // in-memory, code config
            builder.AddInMemoryIdentityResources(Config.IdentityResources);
            
            builder.AddInMemoryApiScopes(Config.ApiScopes); // the combination of resources for users
            builder.AddInMemoryApiResources(Config.ApiResources); // what resources can we get?
            builder.AddInMemoryClients(Config.Clients); // what clients are we allowed to connected to

            // not recommended for production - you need to store your key material somewhere secure(database)
            // it tells the Identityserver about where the certificates store
            builder.AddDeveloperSigningCredential();

            // using entityframework you can store the client and resource information with more security

            services.ConfigureNonBreakingSameSiteCookies();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseIdentityServer();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            }); 
        }
    }
}
