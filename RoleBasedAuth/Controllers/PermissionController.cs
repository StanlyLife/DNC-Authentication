using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RoleBasedAuth.Controllers {

	public class PermissionController : Controller {

		public IActionResult permission() {
			return View();
		}
	}
}