using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace RoleBasedAuth.Models {

	public class PermissionModel {

		public PermissionModel() {
			counter = 0;
		}

		public string role { get; set; }
		public string user { get; set; }

		[IgnoreDataMember]
		public IEnumerable<IdentityUser> userList { get; set; }

		[IgnoreDataMember]
		public IEnumerable<IEnumerable<string>> rolesList { get; set; }

		[IgnoreDataMember]
		public int counter { get; set; }

		[IgnoreDataMember]
		public string loggedInUser { get; set; }
	}
}