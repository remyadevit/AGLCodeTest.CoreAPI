using System;
using System.Reflection;
using AGLTest.Common.Domain.People;
using AGLTest.Common.Settings;
using AGLTest.CQRS;
using AGLTest.CQRS.Cats.Queries.GetCats;
using AGLTest.Service;
using AGLTest.Service.Services.People;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Swashbuckle.AspNetCore.Swagger;

[assembly: ApiController]
[assembly: ApiConventionType(typeof(DefaultApiConventions))]
namespace AGLCodeTest.API
{
    public class Startup
    {
        private const string API_NAME = "AGL Test API";

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public virtual void ConfigureServices(IServiceCollection services)
        {
            // Add CORS policy
            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            corsBuilder.AllowAnyOrigin();
            corsBuilder.AllowCredentials();
            services.AddCors(options =>
            {
                options.AddPolicy("SiteCorsPolicy", corsBuilder.Build());
            });

            // Add MVC
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));

            // Add MediatR
            services.AddMediatR(typeof(GetCatsQuery).GetTypeInfo().Assembly);

            // Add AppSettings
            services.Configure<AppSettings>(Configuration);

            // Add Services
            services.AddTransient<IApiResponseHandler, ApiResponseHandler>();
            services.AddTransient<IErrorResponseFactory, ErrorResponseFactory>();

            // Add HTTP Services
            services.AddHttpClient();
            services.AddHttpClient<IPeopleService, PeopleService>();

            // Add Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = API_NAME, Version = "V1" });
                c.EnableAnnotations();
                c.DescribeAllEnumsAsStrings();
                c.CustomSchemaIds(x => x.FullName);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            IServiceProvider serviceProvider)
        {
            // Enable CORS
            app.UseCors("SiteCorsPolicy");

            app.UseMvc();

            // Add Healthcheck
            app.UseRouter(a => a.MapGet("healthcheck", context =>
            {
                context.Response.Headers[HeaderNames.CacheControl] = "no-cache, no-store";
                context.Response.Headers[HeaderNames.Pragma] = "no-cache";
                return context.Response.WriteAsync($"Success from ({env.EnvironmentName})");
            }));

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("../swagger/v1/swagger.json", API_NAME); });
        }
    }
}
