using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace RoleBasedAuth.Controllers {

	public class LogoutController : Controller {
		private readonly SignInManager<IdentityUser> signInManager;

		public LogoutController(SignInManager<IdentityUser> signInManager) {
			this.signInManager = signInManager;
		}

		public async Task<IActionResult> logoutAsync() {
			await signInManager.SignOutAsync();
			return RedirectToAction("index", "Home");
		}
	}
}