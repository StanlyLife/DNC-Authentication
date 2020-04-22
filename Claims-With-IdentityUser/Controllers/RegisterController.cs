using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Claims_With_IdentityUser.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Claims_With_IdentityUser.Controllers {

	public class RegisterController : Controller {
		private readonly SignInManager<IdentityUser> signInManager;
		private readonly UserManager<IdentityUser> userManager;

		public RegisterController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager) {
			this.signInManager = signInManager;
			this.userManager = userManager;
		}

		public async Task<IActionResult> IndexAsync(ContainerModel model) {
			if (ModelState.IsValid) {
				if (string.IsNullOrWhiteSpace(model.Lrm.username) || string.IsNullOrWhiteSpace(model.Lrm.password)) {
					TempData["Message"] = "User NOT Created! No info recieved";
					return RedirectToAction("index", "home");
				}
				IdentityUser newUser = new IdentityUser(model.Lrm.username);
				newUser.UserName = model.Lrm.username;
				newUser.EmailConfirmed = true;
				await userManager.CreateAsync(newUser, model.Lrm.password);

				TempData["Message"] = $"User created! {model.Lrm.username}";
			} else {
				Console.WriteLine($"Errors: {ModelState.Values}");
			}
			return RedirectToAction("index", "home");
		}
	}
}