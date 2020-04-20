using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using RoleBasedAuth.Data;

namespace RoleBasedAuth.Controllers {

	public class PermissionController : Controller {
		private readonly UserManager<IdentityUser> userManager;
		private readonly RoleManager<IdentityRole> roleManager;

		public PermissionController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) {
			this.userManager = userManager;
			this.roleManager = roleManager;
		}

		[HttpGet]
		public async Task<IActionResult> permissionAsync() {
			var loggedInUser = await userManager.GetUserAsync(User);
			int i = 1;

			foreach (var claims in User.Claims) {
				Console.WriteLine($"{loggedInUser.UserName} claim number: {i} claim: {claims}");
				i++;
			}

			var roles = roleManager.Roles.ToList();
			i = 0;
			foreach (var role in roles) {
				Console.WriteLine($"Role number {i}: {role}");
			}

			return View();
		}
	}
}