using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Claims_With_IdentityUser.Models {

	public class ContainerModel {

		public ContainerModel() {
			counter = 0;
		}

		[NotMapped]
		public LogRegModel Lrm { get; set; }

		[NotMapped]
		public UserClaimsModel Ucm { get; set; }

		[NotMapped]
		public int counter { get; set; }
	}
}