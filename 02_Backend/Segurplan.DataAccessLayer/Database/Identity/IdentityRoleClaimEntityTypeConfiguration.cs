using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Segurplan.Core.Database.EntityTypeConfigurations.Identity {
  
    public class IdentityRoleClaimEntityTypeConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<int>> {
    
        public void Configure(EntityTypeBuilder<IdentityRoleClaim<int>> builder) {
        
            builder.ToTable("RoleClaims");
        }
    }
}
