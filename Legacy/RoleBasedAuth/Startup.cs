using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RoleBasedAuth.AuthorizationRequirements;
using RoleBasedAuth.Data;

namespace RoleBasedAuth {

	public class Startup {

		public Startup(IConfiguration configuration) {
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services) {
			//Add in memory database
			services.AddDbContext<ApplicationUserDbContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

			//Add Identity
			services.AddIdentity<IdentityUser, IdentityRole>(config => {
				config.Password.RequireNonAlphanumeric = false;
			})
				.AddEntityFrameworkStores<ApplicationUserDbContext>() /*connect database to user*/
				.AddDefaultTokenProviders();

			//Add cookies for claims and policies
			services.ConfigureApplicationCookie(config => {
				config.Cookie.Name = "My.Custom.Identity.Cookie";
				config.LoginPath = "/Login/login"; /*unauthorized login*/
			});

			//Add policies
			services.AddAuthentication("myCookieAuthentication").AddCookie("myCookieAuthentication", config => {
				config.Cookie.Name = "Life.Cookie";
				config.LoginPath = "/Login/login"; /*unauthorized login*/
			});

			//IgnoreCookiePolicies
			//Core 2.1 or higher uses GDPR
			//https://stackoverflow.com/questions/59742825/httpcontext-signinasync-fails-to-set-cookie-and-return-user-identity-isauthent
			services.Configure<CookiePolicyOptions>(options => {
				options.ConsentCookie.IsEssential = true;
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => false;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

			services.AddAuthorization(config => {
				/*This works the same as the one bellow*/
				//var defaultAuthBuilder = new AuthorizationPolicyBuilder();
				//var defaultAuthPolicy = defaultAuthBuilder
				//.RequireAuthenticatedUser()
				//.Build();
				//config.DefaultPolicy = defaultAuthPolicy;

				/*This works the same as the one bellow*/
				//config.AddPolicy("Claim.DoB", PolicyBuilder => {
				//	PolicyBuilder.RequireClaim(ClaimTypes.DateOfBirth);
				//});

				config.AddPolicy("EmployeeOnly", policy => policy.RequireClaim("EmployeeNumber"));

				config.AddPolicy("Claim.DoB", PolicyBuilder => {
					PolicyBuilder.RequireCustomClaim(ClaimTypes.DateOfBirth);
				});
			});

			services.AddScoped<IAuthorizationHandler, CustomRequiredClaimHandler>();

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