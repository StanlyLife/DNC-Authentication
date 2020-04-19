using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authentication.Controllers {

	public class HomeController : Controller {

		public IActionResult Index() {
			return View();
		}

		[Authorize]
		public IActionResult Secret() {
			return View();
		}

		public IActionResult Authenticate() {
			//Claims are equal to information about an identity
			var myClaims = new List<Claim>() {
				new Claim(ClaimTypes.Name, "stian"),
				new Claim(ClaimTypes.Email, "stianhave@hotmail.com"),
				new Claim("ThisApp.Says", "stian created this"),
			};

			var licenceClaims = new List<Claim>() {
				new Claim(ClaimTypes.Name, "stian have"),
				new Claim("Driverslicence", "B"),
			};

			//An identity is a set of "Information" and "authentication type"
			var StianIdentity = new ClaimsIdentity(myClaims, "my Identity");
			var LicenceIdentity = new ClaimsIdentity(licenceClaims, "Licence Identity");

			//Collection of identities which has a collection of claims
			var userPrincipal = new ClaimsPrincipal(new[] { StianIdentity, LicenceIdentity });

			HttpContext.SignInAsync(userPrincipal);

			return RedirectToAction("index");
		}
	}
}