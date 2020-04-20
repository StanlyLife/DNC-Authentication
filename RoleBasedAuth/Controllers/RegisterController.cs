using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoleBasedAuth.Models;

namespace RoleBasedAuth.Controllers {

	public class RegisterController : Controller {
		private readonly UserManager<IdentityUser> userManager;

		public RegisterController(UserManager<IdentityUser> userManager) {
			this.userManager = userManager;
		}

		[HttpGet]
		public IActionResult register() {
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> register(UserModel model) {
			if (ModelState.IsValid) {
				var newUser = new IdentityUser();

				newUser.UserName = model.username;
				newUser.PasswordHash = model.password;

				var createdUser = await userManager.CreateAsync(newUser);
				if (createdUser.Succeeded) {
					Console.WriteLine($"CREATED USER! username: {newUser.UserName}, password: {newUser.PasswordHash}");
				} else {
					ModelState.AddModelError("All", createdUser.ToString());
				}
			} else {
				Console.WriteLine("invalid");
				foreach (var error in ModelState) {
					Console.WriteLine(error);
				}
			}
			return View(model);
		}
	}
}