using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoleBasedAuth.Models;

namespace RoleBasedAuth.Controllers {

	public class LoginController : Controller {
		private readonly UserManager<IdentityUser> userManager;
		private readonly SignInManager<IdentityUser> signInManager;

		public LoginController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) {
			this.userManager = userManager;
			this.signInManager = signInManager;
		}

		[HttpGet]
		public IActionResult login() {
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> loginAsync(UserModel model) {
			if (ModelState.IsValid) {
				var user = await userManager.FindByNameAsync(model.username);
				if (user != null) {
					var signInresult = await signInManager.PasswordSignInAsync(user, model.password, false, false);
					if (signInresult.Succeeded) {
						Console.WriteLine("logged in successfully!");
					} else {
						ModelState.AddModelError("All", signInresult.ToString());
					}
				} else {
					ModelState.AddModelError("All", "user not found");
				}
			} else {
				Console.WriteLine("invalid");
			}
			return View(model);
		}
	}
}