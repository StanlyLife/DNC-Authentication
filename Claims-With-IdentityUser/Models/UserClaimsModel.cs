using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Claims_With_IdentityUser.Models {

	public class UserClaimsModel {

		[NotMapped]
		public IEnumerable<string> username { get; set; }

		[NotMapped]
		public IEnumerable<IEnumerable<string>> claimsList { get; set; }
	}
}