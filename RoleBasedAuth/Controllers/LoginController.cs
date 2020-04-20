using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RoleBasedAuth.Models;

namespace RoleBasedAuth.Controllers {

	public class LoginController : Controller {

		[HttpGet]
		public IActionResult login() {
			return View();
		}

		[HttpPost]
		public IActionResult login(UserModel model) {
			if (ModelState.IsValid) {
				Console.WriteLine("valid");
			} else {
				Console.WriteLine("invalid");
			}
			return View(model);
		}
	}
}