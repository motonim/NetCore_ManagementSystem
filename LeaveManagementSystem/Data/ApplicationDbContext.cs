using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> // meaning ApplicationDbContext is an inheritance from IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole 
                { 
                    Id = "145c0029-5f74-4c22-bd70-d2fe2cfc5b7e",
                    Name = "Employee",
                    NormalizedName = "EMPLOYEE"
                }, 
                new IdentityRole 
                {
                    Id = "df88a034-71a6-476e-b25e-08a9059d4b2f",
                    Name = "Supervisor",
                    NormalizedName = "SUPERVISOR"
                }, 
                new IdentityRole 
                {
                    Id = "3033f938-c243-4cb6-ac28-27874b32687f",
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                }
               );

            var hasher = new PasswordHasher<ApplicationUser>();

            builder.Entity<ApplicationUser>()
                .HasData(new ApplicationUser
                { 
                    Id = "680a1fe0-9f6b-4bb8-acf3-a720ca5a35d8",
                    Email = "admin@localhost.com",
                    NormalizedEmail = "ADMIN@LOCALHOST.COM",
                    NormalizedUserName = "ADMIN@LOCALHOST.COM",
                    UserName = "admin@localhost.com",
                    PasswordHash = hasher.HashPassword(null, "P@ssword1"),
                    EmailConfirmed = true,
                    FirstName = "Default first name",
                    LastName = "Default last name",
                    DateOfBirth = new DateOnly(1955, 12, 15)
                });

            builder.Entity<IdentityUserRole<string>>()
                .HasData(
                    new IdentityUserRole<string>
                    {
                        RoleId = "3033f938-c243-4cb6-ac28-27874b32687f",
                        UserId = "680a1fe0-9f6b-4bb8-acf3-a720ca5a35d8"
                    }
                );
        }

        public DbSet<LeaveType> LeaveTypes { get; set; }
    }
}
