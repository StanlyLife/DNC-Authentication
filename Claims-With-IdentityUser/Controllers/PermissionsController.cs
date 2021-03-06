﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Claims_With_IdentityUser.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Claims_With_IdentityUser.Controllers {

	public class PermissionsController : Controller {
		private readonly UserManager<IdentityUser> userManager;
		private readonly SignInManager<IdentityUser> signinManage;

		public PermissionsController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signinManage) {
			this.userManager = userManager;
			this.signinManage = signinManage;
		}

		public async Task<IActionResult> RemoveClaimAsync(ContainerModel model) {
			if (User.Identity.IsAuthenticated) {
				var result = await ClaimRemoval(model);
				if (result)
					await signinManage.RefreshSignInAsync(await userManager.FindByNameAsync(model.username));
			} else {
				TempData["Message"] = "You need to be signed in to remove claims";
			}
			return RedirectToAction("index", "home");
		}

		private async Task<bool> ClaimRemoval(ContainerModel model) {
			IdentityUser user = await userManager.FindByNameAsync(model.username);
			var claimToAdd = new Claim(model.claimName, model.claimValue);
			var userClaims = userManager.GetClaimsAsync(user).Result;
			Claim claimToRemove = null;
			bool hasClaim = false;
			foreach (var uc in userClaims) {
				if (uc.Type == claimToAdd.Type) {
					hasClaim = true;

					//Cannot have two claims with different values now
					claimToRemove = uc;

					Console.WriteLine(uc);
				} else {
					Console.WriteLine($"claim: [({uc.Type}),({uc.Value})] != userClaim[({claimToAdd.Type}),({claimToAdd.Value})]");
				}
			}

			if (hasClaim) {
				await userManager.RemoveClaimAsync(user, claimToRemove);
				TempData["Message"] = "Claim Removed!";
				Console.WriteLine("Claim Removed!");
				return true;
			} else {
				TempData["Message"] = $"user {model.username} does not have a claim with type {model.claimName} and value {model.claimValue}";
				Console.WriteLine($"user {model.username} does not have a claim with type {model.claimName} and value {model.claimValue}");
				return false;
			}
		}

		public async Task<IActionResult> SetClaimAsync(ContainerModel model) {
			if (User.Identity.IsAuthenticated) {
				IdentityUser user = await userManager.FindByNameAsync(model.username);

				var claimToAdd = new Claim(model.claimName, model.claimValue);

				await ClaimRemoval(model);

				await userManager.AddClaimAsync(user, new Claim(model.claimName, model.claimValue));
				await signinManage.RefreshSignInAsync(await userManager.FindByNameAsync(model.username));
				TempData["Message"] = "Claim ADDED!";
			} else {
				TempData["Message"] = "You need to be signed in to set claims";
			}
			return RedirectToAction("index", "home");
		}
	}
}