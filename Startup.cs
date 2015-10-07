using System;
using Microsoft.AspNet.Builder;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

public class Startup
{
	public IServiceProvider ConfigureServices(IServiceCollection services)
	{
		services
			.AddMvcCore()
			.AddAuthorization()
			.AddFormatterMappings(m => m.SetMediaTypeMappingForFormat("js", new MediaTypeHeaderValue("application/json")))
			.AddJsonFormatters(j => j.Formatting = Formatting.Indented)
		;
		
		return services.BuildServiceProvider();
	}
	
	public void Configure(IApplicationBuilder app)
	{
		app
			.UseStatusCodePages()
			.UseFileServer()
			.UseMvcWithDefaultRoute()
		;
	}
}
