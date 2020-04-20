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
		private readonly RoleManager<IdentityRole> roleManager;

		public RegisterController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) {
			this.userManager = userManager;
			this.roleManager = roleManager;
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
				/*creating passwords this way creates unhashed passwords*/
				/*If you want to hash your own passwords, you can do it here*/
				/*If you want to use built in hash you have to pass password as a parameter in create async*/
				//newUser.PasswordHash = model.password;

				var createdUser = await userManager.CreateAsync(newUser, model.password);
				if (createdUser.Succeeded) {
					//User created successfully!
					Console.WriteLine($"CREATED USER! username: {newUser.UserName}, password: {newUser.PasswordHash}");

					//Add user to role
					await AddUserToRole(newUser, createdUser);
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

		private async Task<IdentityResult> AddUserToRole(IdentityUser newUser, IdentityResult createdUser) {
			//Check if role exists
			if (!await roleManager.RoleExistsAsync("myRole")) {
				var role = new IdentityRole();
				role.Name = "myRole";
				await roleManager.CreateAsync(role);
			}

			//Add user to role
			createdUser = await userManager.AddToRoleAsync(newUser, "myRole");
			return createdUser;
		}
	}
}