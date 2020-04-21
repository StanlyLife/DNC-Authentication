using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RoleBasedAuth.AuthorizationRequirements {

	public class CustomRequiredClaim : IAuthorizationRequirement {//This is a request

		public CustomRequiredClaim(string myClaimType) {
			this.myClaimType = myClaimType;
		}

		public string myClaimType { get; }
	}

	public class CustomRequiredClaimHandler : AuthorizationHandler<CustomRequiredClaim> { //This handles the request

		protected override Task HandleRequirementAsync(
			AuthorizationHandlerContext context,
			CustomRequiredClaim requirement
			) {
			var hasClaim = context.User.Claims.Any(x => x.Type == requirement.myClaimType);
			if (hasClaim) {
				context.Succeed(requirement);
			}

			return Task.CompletedTask;
		}
	}

	public static class AuthorizationPolicyBuilderExtensions {

		public static AuthorizationPolicyBuilder RequireCustomClaim(this AuthorizationPolicyBuilder builder, string claimType) {
			builder.AddRequirements(new CustomRequiredClaim(claimType));
			return builder;
		}
	}
}