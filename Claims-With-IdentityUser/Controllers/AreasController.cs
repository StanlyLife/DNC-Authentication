using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Claims_With_IdentityUser.Controllers {

	public class AreasController : Controller {

		[Authorize(Policy = "myWebsiteGenderPolicy")]
		public IActionResult GenderRequired() {
			return View();
		}

		[Authorize(Policy = "myWebsiteAgePolicy")]
		public IActionResult TwentyOne() {
			return View();
		}
	}
}