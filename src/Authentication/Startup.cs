using Authentication.Policies;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;

namespace Authentication
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // only allow authenticated users
            var defaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            services.AddMvc(setup =>
            {
                setup.Filters.Add(new AuthorizeFilter(defaultPolicy));
            });

            services.ConfigureAuthorization(options =>
            {
                // inline policies
                options.AddPolicy("SalesOnly", policy =>
                {
                    policy.RequireClaim("department", "sales");
                });
                options.AddPolicy("SalesSenior", policy =>
                {
                    policy.RequireClaim("department", "sales");
                    policy.RequireClaim("status", "senior");
                });

                // custom policy
                options.AddPolicy("DevInterns", policy =>
                {
                    policy.AddRequirements(new StatusRequirement("development", "intern"));

                    // ..or using extension method
                    //policy.RequireStatus("development", "intern");
                });

            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.MinimumLevel = LogLevel.Information;
            
            app.UseCookieAuthentication(options =>
            {
                options.LoginPath = "/account/login";
                options.AccessDeniedPath = "/account/forbidden";

                options.AuthenticationScheme = "Cookies";
                options.AutomaticAuthentication = true;
            });

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
