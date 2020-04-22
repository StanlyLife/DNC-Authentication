using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityExample.Data {

	//IdentityDBContext contains all the user tables
	public class AppDbContext : IdentityDbContext<IdentityUser> {

		//Add ctor
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
		}
	}
}