using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Claims_With_IdentityUser.Models;
using Microsoft.AspNetCore.Identity;

namespace Claims_With_IdentityUser.Controllers {

	public class HomeController : Controller {
		private readonly ILogger<HomeController> _logger;
		private readonly UserManager<IdentityUser> userManager;

		public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager) {
			_logger = logger;
			this.userManager = userManager;
		}

		public async Task<IActionResult> IndexAsync() {
			ContainerModel model = new ContainerModel();

			if (User.Identity.IsAuthenticated) {
				model.user = await userManager.GetUserAsync(User);
			}

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