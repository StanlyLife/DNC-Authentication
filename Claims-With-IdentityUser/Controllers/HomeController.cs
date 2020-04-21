﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Claims_With_IdentityUser.Models;

namespace Claims_With_IdentityUser.Controllers {

	public class HomeController : Controller {
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger) {
			_logger = logger;
		}

		public IActionResult Index() {
			ContainerModel model = new ContainerModel();
			return View(model);
		}

		#region NotAuthenticated

		public IActionResult loginRequired() {
			return View();
		}

		public IActionResult Denied() {
			return View();
		}

		#endregion NotAuthenticated

		#region errorView

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error() {
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		#endregion errorView
	}
}