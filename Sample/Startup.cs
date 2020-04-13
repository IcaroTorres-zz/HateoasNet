using HateoasNet.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Sample
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
			// register required services and dependencies for hateoas
			services.AddHateoasServices();

			/*
			 * switch this lines to test different configuration styles
			 * Check 'HateoasSetupExtensions.cs' file for details of each one
			 */
			//services.AddControllers().HateoasOneFileMapping();
			//services.AddControllers().HateoasSeparatedFilesMapping();
			services.AddControllers().HateoasSeparatedFilesUsingAssembly();
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