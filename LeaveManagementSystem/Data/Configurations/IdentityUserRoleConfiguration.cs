using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Data.Configurations
{
    public class IdentityUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Microsoft.AspNetCore.Identity.IdentityUserRole<string>> builder)
        {
            builder.HasData(
                    new IdentityUserRole<string>
                    {
                        RoleId = "3033f938-c243-4cb6-ac28-27874b32687f",
                        UserId = "680a1fe0-9f6b-4bb8-acf3-a720ca5a35d8"
                    }
                );
        }
    }
}
