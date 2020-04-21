using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Claims_With_IdentityUser.Models {

	public class LogRegModel {

		[NotMapped]
		public string username { get; set; }

		[NotMapped]
		public string password { get; set; }
	}
}