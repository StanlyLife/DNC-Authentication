using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using RoleBasedAuth.Data;
using RoleBasedAuth.Models;

namespace RoleBasedAuth.Controllers {

	public class PermissionController : Controller {
		private readonly UserManager<IdentityUser> userManager;
		private readonly RoleManager<IdentityRole> roleManager;

		public PermissionController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) {
			this.userManager = userManager;
			this.roleManager = roleManager;
		}

		[HttpGet]
		public IActionResult permission() {
			return View();
		}

		public IActionResult ButtonClick() {
			return RedirectToAction("permission");
		}

		public async Task<IActionResult> getClaimsAsync() {
			var loggedInUser = await userManager.GetUserAsync(User);
			int i = 1;

			foreach (var claims in User.Claims) {
				Console.WriteLine($"{loggedInUser.UserName} claim number: {i} claim: {claims}");
				i++;
			}

			return RedirectToAction("permission");
		}

		public IActionResult getAllRoles() {
			Console.WriteLine();
			var roles = roleManager.Roles.ToList();
			int i = 0;
			foreach (var role in roles) {
				Console.WriteLine($"Role number {i}: {role}");
				i++;
			}
			return RedirectToAction("permission");
		}

		public async Task<IActionResult> getUserRolesAsync() {
			Console.WriteLine();
			var roles = roleManager.Roles.ToList();
			var loggedInUser = await userManager.FindByNameAsync(User.Identity.Name.ToString());
			int i = 0;
			foreach (var ApplicationRole in roles) {
				//https://stackoverflow.com/questions/40151224/user-isinrole-returns-nothing-in-asp-net-core-repository-pattern-implemented
				//IsInRole checks cookie and not against database!
				var role = ApplicationRole.ToString();

				if (/*User.IsInRole(role.ToString())*/
					await userManager.IsInRoleAsync(loggedInUser, role)
					) {
					Console.WriteLine($"{User.Identity.Name} is in Role number {i}: {role}");
					i++;
				} else {
					Console.WriteLine($"{User.Identity.Name} is NOT in Role number {i}: {role}");
					i++;
				}
			}
			return RedirectToAction("permission");
		}

		[HttpPost]
		public async Task<IActionResult> CreateRolesAsync(PermissionModel model) {
			if (!await roleManager.RoleExistsAsync(model.role)) {
				await CreateOneRoleAsync(model.role);
			}
			return RedirectToAction("permission");
		}

		public async Task<IActionResult> AddUserToRoleAsync(PermissionModel model) {
			var userToAppendRole = await userManager.FindByNameAsync(model.user);
			if (!User.IsInRole(model.role.ToString())) { /*CHECK IF User is in role*/
				var roles = roleManager.Roles.ToList();
				bool roleExists = false;
				string roleToAppendUser = model.role;
				foreach (var role in roles) { /*CHECK IF Role exists*/
					if (role.ToString().ToLower() == model.role.ToLower()) {
						roleExists = true;
						roleToAppendUser = role.ToString();
					}
				}
				if (!roleExists) { /*IF NOT EXISTS Create role*/
					var result = await CreateOneRoleAsync(model.role);
					roleToAppendUser = result.ToString();
				}

				/*Add User to role*/
				Console.WriteLine($"Added user to role {roleToAppendUser}");
				await userManager.AddToRoleAsync(userToAppendRole, roleToAppendUser);
			} else {
				/*If user is already in role, return error*/
				Console.WriteLine("user is already in role");
				ModelState.AddModelError("All", $"{userToAppendRole} is already in {model.role}");
			}
			return RedirectToAction("permission");
		}

		public async Task<IdentityResult> CreateOneRoleAsync(string roleName) {
			var role = new IdentityRole();
			role.Name = roleName;
			return await roleManager.CreateAsync(role);
		}
	}
}