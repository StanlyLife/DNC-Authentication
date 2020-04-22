using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Authentication {

	public class Startup {

		public void ConfigureServices(IServiceCollection services) {
			services.AddAuthentication("cookieAuth").AddCookie("cookieAuth", config => {
				config.Cookie.Name = "StiansCookie";
				config.LoginPath = "/home/Authenticate";
			});
			services.AddControllersWithViews();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			//Who are you?
			//Always call before Authorization
			app.UseAuthentication();

			//Are you allowed?
			app.UseAuthorization();

			app.UseEndpoints(endpoints => {
				endpoints.MapDefaultControllerRoute();
			});
		}
	}
}