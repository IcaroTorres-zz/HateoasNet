using HateoasNet.Core.DependencyInjection;
using HateoasNet.Core.Sample.JsonData;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
				/*
				 * switch this lines to test different configuration styles
				 * Check 'HateoasSetupExtensions.cs' file for details of each one
				 */
				//.HateoasInlineConfiguration()
				//.HateoasConfigurations()
				.HateoasConfigurationUsingAssembly()

				// MvcBuilder
				.AddControllers()
				.AddHateoasFormatter();
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
