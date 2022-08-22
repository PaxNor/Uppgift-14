using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Uppgift_14.Models;

namespace Uppgift_14.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) {
        }

        public DbSet<GymClass> GymClass => Set<GymClass>(); 

        // Setting up composite-key consisting of ApplicationUserId and GymClassId. Not defined manually in the class!
        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUserGymClass>().HasKey(k => new { k.ApplicationUserId, k.GymClassId } );
        }
    }
}