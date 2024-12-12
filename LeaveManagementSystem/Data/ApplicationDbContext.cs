using LeaveManagementSystem.Data.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

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

            //builder.ApplyConfiguration(new IdentityRoleConfiguration());
            //builder.ApplyConfiguration(new IdentityUserRoleConfiguration());
            //builder.ApplyConfiguration(new ApplicationUserConfiguration());
            //builder.ApplyConfiguration(new LeaveRequestStatusConfiguration());
            // instead of these four lines, we can achieve the same behaviour by the following one line
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }

        public DbSet<LeaveType> LeaveTypes { get; set; }

        public DbSet<LeaveAllocation> LeaveAllocations { get; set; }
        public DbSet<Period> Periods { get; set; }
        public DbSet<LeaveRequestStatus> LeaveRequestStatuses { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
    }
}
