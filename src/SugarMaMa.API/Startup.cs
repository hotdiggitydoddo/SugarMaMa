using System;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using SugarMaMa.API.DAL.Entities;
using SugarMaMa.API.DAL;
using SugarMaMa.API.Services;
using SugarMaMa.API.DAL.Repositories;
using SugarMaMa.API.Helpers;

namespace SugarMaMa.API
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);
            services.AddAuthentication();
            services.AddMvc();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            services.AddTransient<IRepository<Esthetician, int>, Repository<Esthetician, int>>();
            services.AddTransient<IRepository<SpaService, int>, Repository<SpaService, int>>();
            services.AddTransient<ISpaServicesService, SpaServicesService>();
            services.AddTransient<IEstheticianService, EstheticianService>();

            services.AddSingleton(AutoMapperConfig.Configure());

            var connectionString = Configuration["DbConnectionString"]; //Configuration["DbContextSettings:ConnectionString"];

            services.AddDbContext<SMDbContext>(
                opts => opts.UseNpgsql(connectionString)
            );
           
            services.AddIdentity<ApplicationUser, ApplicationRole>(o =>
            {
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
                o.Password.RequireUppercase = false;
            })
           .AddEntityFrameworkStores<SMDbContext, Guid>()
           .AddDefaultTokenProviders();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseOAuthValidation();
            app.UseCors(p => p.AllowAnyOrigin());
            app.UseOpenIdConnectServer(options => {
                // Create your own authorization provider by subclassing
                // the OpenIdConnectServerProvider base class.
                options.Provider = new AuthorizationProvider();

                // Enable the authorization and token endpoints.
                options.AuthorizationEndpointPath = "/api/authorize";
                options.TokenEndpointPath = "/api/token";

                // During development, you can set AllowInsecureHttp
                // to true to disable the HTTPS requirement.
                if (env.IsDevelopment())
                    options.AllowInsecureHttp = true;

                // Note: uncomment this line to issue JWT tokens.
                // options.AccessTokenHandler = new JwtSecurityTokenHandler();
            });

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //if (env.IsDevelopment() || env.IsStaging())
            //    DbContextExtensions.Seed(app);

            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseMvc();
        }
    }
}
