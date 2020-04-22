using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Claims_With_IdentityUser.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Claims_With_IdentityUser.Data;

namespace Claims_With_IdentityUser.Controllers {

	public class HomeController : Controller {
		private readonly ILogger<HomeController> _logger;
		private readonly UserManager<IdentityUser> userManager;
		private readonly ApplicationDbContext context;
		private readonly SignInManager<IdentityUser> signInManager;
		public ContainerModel model;

		public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager, ApplicationDbContext context, SignInManager<IdentityUser> signInManager) {
			_logger = logger;
			this.userManager = userManager;
			this.context = context;
			this.signInManager = signInManager;
			model = new ContainerModel();
		}

		public async Task<IActionResult> IndexAsync() {
			if (User.Identity.Name != null) {
				await getUsersAndClaims();
			}

			return View(model);
		}

		public async Task<IActionResult> LoginAsync(ContainerModel model) {
			if (string.IsNullOrWhiteSpace(model.Lrm.password) || string.IsNullOrWhiteSpace(model.Lrm.username)) {
				TempData["Message"] = "Enter username and password";
				return RedirectToAction("index", "home");
			}

			var user = await userManager.FindByNameAsync(model.Lrm.username);

			if (user != null) {
				if (signInManager.IsSignedIn(User)) {
					Console.WriteLine($"{User.Identity.Name} is already signed in");
				} else {
					Console.WriteLine("User is not signed in, starting signin...");
				}

				var result = await signInManager.PasswordSignInAsync(user, model.Lrm.password, true, false);
				Console.WriteLine(result.Succeeded);
				if (result.Succeeded) {
					TempData["message"] = "login successfull";
				} else {
					TempData["message"] = "login failed";
				}
			} else {
				TempData["Message"] = "no user found";
			}

			return RedirectToAction("index");
		}

		private async Task<ContainerModel> getUsersAndClaims() {
			List<string> usernames = new List<string>();
			List<List<string>> claims = new List<List<string>>();
			var listOfUsers = context.Users.ToList();
			foreach (var user in listOfUsers) {
				List<string> userClaims = new List<string>();
				foreach (var claim in await userManager.GetClaimsAsync(user)) {
					userClaims.Add(claim.ToString());
				}
				usernames.Add(user.UserName);
				claims.Add(userClaims);
			}

			UserClaimsModel ucm = new UserClaimsModel {
				username = usernames,
				claimsList = claims
			};

			model.Ucm = ucm;

			return model;
		}

		#region NotAuthenticated

		public IActionResult loginRequired() {
			return View();
		}

		public IActionResult Denied() {
			return View();
		}

		#endregion NotAuthenticated

		#region errorView

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error() {
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		#endregion errorView
	}
}