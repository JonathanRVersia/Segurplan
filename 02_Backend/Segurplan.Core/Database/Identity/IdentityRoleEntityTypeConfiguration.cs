using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Segurplan.Core.Database.EntityTypeConfigurations.Identity {
    public class IdentityRoleEntityTypeConfiguration : IEntityTypeConfiguration<IdentityRole<int>> {
        public void Configure(EntityTypeBuilder<IdentityRole<int>> builder) {
            builder.ToTable("Roles");
        }
    }
}
