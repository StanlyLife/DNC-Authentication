using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityExample.Controllers {

	public class HomeController : Controller {

		//GET user info or POST user info
		private readonly UserManager<IdentityUser> userManager;

		private readonly SignInManager<IdentityUser> signInManager;

		public HomeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) {
			//Inject services
			this.userManager = userManager;
			this.signInManager = signInManager;
		}

		public IActionResult Index() {
			return View();
		}

		[Authorize]
		public IActionResult Secret() {
			return View();
		}

		public IActionResult Login() {
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> LoginAsync(string userName, string password) {
			//Login Functionality
			var user = await userManager.FindByNameAsync(userName);

			if (user != null) {
				//sign in
				var SignInresult = await signInManager.PasswordSignInAsync(user, password, false, false);

				if (SignInresult.Succeeded) {
					//Signin successfull
					return RedirectToAction("index");
				}
			}
			return RedirectToAction("index");
		}

		public IActionResult Register() {
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(string username, string password) {
			//REgister Functionallity
			var user = new IdentityUser {
				UserName = username,
				Email = "",
			};

			var result = await userManager.CreateAsync(user, password);

			if (result.Succeeded) {
				//Signin user here
				var SignInresult = await signInManager.PasswordSignInAsync(user, password, false, false);

				if (SignInresult.Succeeded) {
					//Signin successfull
					return RedirectToAction("index");
				}
			}

			return RedirectToAction("index");
		}

		public async Task<IActionResult> Logout() {
			await signInManager.SignOutAsync();
			return RedirectToAction("index");
		}
	}
}