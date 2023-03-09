using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Segurplan.Core.Database.EntityTypeConfigurations.Identity {
    public class IdentityUserRoleEntityTypeConfiguration : IEntityTypeConfiguration<IdentityUserRole<int>> {
        public void Configure(EntityTypeBuilder<IdentityUserRole<int>> builder) {
            builder.ToTable("UserRoles");
        }
    }
}
