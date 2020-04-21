using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Claims_With_IdentityUser.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Claims_With_IdentityUser {

	public class Startup {

		public Startup(IConfiguration configuration) {
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services) {
			//Add database
			services.AddDbContext<ApplicationDbContext>(options => {
				options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
			});

			//Add identity
			services.AddIdentity<IdentityUser, IdentityRole>(options => {
				options.Password.RequireDigit = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequiredUniqueChars = 1;
				options.Password.RequiredLength = 1;
			}).AddEntityFrameworkStores<ApplicationDbContext>()
			.AddDefaultTokenProviders();

			services.ConfigureApplicationCookie(options => {
				options.Cookie.Name = "my websites cookie";
				options.AccessDeniedPath = "/home/denied";
				options.LoginPath = "/home/loginRequired";
			});
			services.AddAuthentication(options => {
			}).AddCookie("My Authentication schema", options => {
				options.Cookie.Name = "my authentication cookie";
				options.AccessDeniedPath = "/home/authenticationdenied";
			});

			services.AddAuthorization(options => {
				options.AddPolicy("myWebsiteGenderPolicy", MyPolicyBuilder => {
					MyPolicyBuilder.RequireClaim(ClaimTypes.Gender);
				});

				options.AddPolicy("myWebsiteAgePolicy", MyPolicyBuilder => {
					MyPolicyBuilder.RequireClaim("Age", "21");
				});
			});

			services.AddControllersWithViews();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			} else {
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints => {
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}