using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Claims_With_IdentityUser.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
				options.SignIn.RequireConfirmedAccount = false;
				options.SignIn.RequireConfirmedEmail = false;
				options.SignIn.RequireConfirmedPhoneNumber = false;

				options.Password.RequireDigit = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequiredUniqueChars = 1;
				options.Password.RequiredLength = 1;
			}).AddEntityFrameworkStores<ApplicationDbContext>()
			.AddDefaultTokenProviders();

			services.ConfigureApplicationCookie(options => {
				options.Cookie.Name = "My.Custom.Identity.Cookie";
				options.AccessDeniedPath = "/home/denied";
				options.LoginPath = "/home/loginRequired";
			});
			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options => {
					options.Cookie.IsEssential = true;
					options.Cookie.HttpOnly = true;
					options.Cookie.Name = "my.authentication.cookie";
					options.AccessDeniedPath = "/home/authenticationdenied";
				});

			//services.AddAuthentication("myCookieAuthentication").AddCookie("myCookieAuthentication", config => {
			//	config.Cookie.Name = "Life.Cookie";
			//	config.LoginPath = "/Login/login"; /*unauthorized login*/
			//});

			services.AddAuthorization(options => {
				options.AddPolicy("myWebsiteGenderPolicy", MyPolicyBuilder => {
					MyPolicyBuilder.RequireClaim(ClaimTypes.Gender);
				});

				options.AddPolicy("myWebsiteAgePolicy", MyPolicyBuilder => {
					MyPolicyBuilder.RequireClaim("age", "21");
				});
			});

			/**/
			services.Configure<CookiePolicyOptions>(options => {
				options.ConsentCookie.IsEssential = true;
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => false;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});
			/**/

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