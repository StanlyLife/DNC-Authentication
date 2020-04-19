using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityExample.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityExample {

	public class Startup {

		public void ConfigureServices(IServiceCollection services) {
			services.AddDbContext<AppDbContext>(config => {
				config.UseInMemoryDatabase("DatabaseName");
			}
			);

			//AddIdentity registers the services, allows identiy to communicate with database
			services.AddIdentity<IdentityUser, IdentityRole>(config => {
				config.Password.RequireDigit = false;
				config.Password.RequiredLength = 1;
				config.Password.RequireNonAlphanumeric = false;
				config.Password.RequireUppercase = false;
			})
				.AddEntityFrameworkStores<AppDbContext>()
				.AddDefaultTokenProviders();

			services.ConfigureApplicationCookie(config => {
				config.Cookie.Name = "IdentityCookie";
				config.LoginPath = "/home/login";
			});

			/*Cannot use commented out code bellow when using identity*/

			//services.AddAuthentication("cookieAuth").AddCookie("cookieAuth", config => {
			//	config.Cookie.Name = "StiansCookie";
			//	config.LoginPath = "/home/Authenticate";
			//});
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