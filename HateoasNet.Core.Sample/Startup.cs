using HateoasNet.Core.Sample.JsonData;
using HateoasNet.DependencyInjection.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace HateoasNet.Core.Sample
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
            services
                .AddScoped<Seeder>()
                .HateoasConfigurationUsingAssembly()
                .RegisterAllCustomHateoas(new Assembly[] { typeof(Startup).Assembly }, ServiceLifetime.Scoped)
                .AddControllers();

            services.AddMvc(options => options.EnableEndpointRouting = false)
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection()
               .UseRouting()
               .UseAuthorization()
               .UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
