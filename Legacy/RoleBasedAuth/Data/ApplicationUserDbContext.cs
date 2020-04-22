using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoleBasedAuth.Data {

	public class ApplicationUserDbContext : IdentityDbContext {

		public DbSet<IdentityUser> UserContext { get; set; }
		public ApplicationUserDbContext(DbContextOptions<ApplicationUserDbContext> options) : base(options) {
			Database.EnsureCreated();
		}
	}
}