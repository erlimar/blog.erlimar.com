using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using Microsoft.Net.Http.Headers;
using Models;
using Newtonsoft.Json;

public class Startup
{
	public IServiceProvider ConfigureServices(IServiceCollection services)
	{
		services
#if DNXCORE50
			.AddMvcCore()
			.AddAuthorization()
			.AddFormatterMappings(m => m.SetMediaTypeMappingForFormat("js", new MediaTypeHeaderValue("application/json")))
			.AddJsonFormatters(j => {
				j.Formatting = Formatting.Indented;
				j.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
			})
#else
			.AddMvc()
			.AddFormatterMappings(m => m.SetMediaTypeMappingForFormat("js", new MediaTypeHeaderValue("application/json")))
			.AddJsonOptions(opt => {
				opt.SerializerSettings.Formatting = Formatting.Indented;
				opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
			})
#endif
		;
		
		services.AddTransient<BlogContext>();
		
		return services.BuildServiceProvider();
	}
	
	public void Configure(IApplicationBuilder application, IHostingEnvironment env, ILoggerFactory loggerfactory)
	{
		loggerfactory.AddConsole(LogLevel.Verbose);
		
		var log = loggerfactory.CreateLogger(this.GetType().Name);
		
		log.LogInformation(env.EnvironmentName);
		
		if(env.IsDevelopment()) {
			application
				.UseErrorPage()
				.UseStatusCodePages();
		}
			
		application
			.UseFileServer()
			.UseMvcWithDefaultRoute();
	}
}
