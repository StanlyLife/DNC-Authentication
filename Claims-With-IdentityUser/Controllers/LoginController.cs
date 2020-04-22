using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Claims_With_IdentityUser.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Claims_With_IdentityUser.Controllers {

	public class LoginController : Controller {
		private readonly SignInManager<IdentityUser> signInManager;
		private readonly UserManager<IdentityUser> userManager;

		public LoginController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager) {
			this.signInManager = signInManager;
			this.userManager = userManager;
		}

		public async Task<IActionResult> IndexAsync(ContainerModel model) {
			if (string.IsNullOrWhiteSpace(model.Lrm.password) || string.IsNullOrWhiteSpace(model.Lrm.username)) {
				TempData["Message"] = "Enter username and password";
				return RedirectToAction("index", "home");
			}

			var user = await userManager.FindByNameAsync(model.Lrm.username);

			if (user != null) {
				//if(signInManager.IsSignedIn())

				var result = await signInManager.PasswordSignInAsync(user, model.Lrm.password, true, false);
				if (result.Succeeded) {
					TempData["message"] = "login successfull";
				} else {
					TempData["message"] = "login failed";
				}
			} else {
				TempData["Message"] = "no user found";
			}

			return RedirectToAction("index", "home");
		}
	}
}