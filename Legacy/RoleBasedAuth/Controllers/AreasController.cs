﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RoleBasedAuth.Controllers {

	public class AreasController : Controller {

		public IActionResult Index() {
			return View();
		}

		[Authorize(Roles = "admin")]
		public IActionResult AdminPage() {
			return View();
		}

		[Authorize(Policy = "Claim.DoB")]
		public IActionResult DoBPage() {
			return View();
		}
	}
}