using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Claims_With_IdentityUser.Authorization {

	public class MyPolicyRequirements : IAuthorizationRequirement {
		/**/

		public class MinimumAgeRequirement : MyPolicyRequirements {
			public int MinimumAge { get; }

			public MinimumAgeRequirement(int minimumAge) {
				MinimumAge = minimumAge;
			}
		}

		/**/

		/**/

		public class MinimumAgeHandler : AuthorizationHandler<MinimumAgeRequirement> {

			protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
														   MinimumAgeRequirement requirement) {
				if (!context.User.HasClaim(c => c.Type == "age")) {
					return Task.CompletedTask;
				}

				if (context.User.HasClaim(c => c.Type == "age" && Int32.Parse(c.Value) > 20)) {
					context.Succeed(requirement);
				}

				return Task.CompletedTask;
			}
		}

		/**/
	}
}