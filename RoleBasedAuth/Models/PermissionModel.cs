using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoleBasedAuth.Models {

	public class PermissionModel {
		public string role { get; set; }
		public string user { get; set; }
	}
}