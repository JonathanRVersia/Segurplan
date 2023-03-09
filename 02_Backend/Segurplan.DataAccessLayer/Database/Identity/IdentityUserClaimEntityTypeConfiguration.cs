using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Segurplan.Core.Database.EntityTypeConfigurations.Identity {
   
    public class IdentityUserClaimEntityTypeConfiguration : IEntityTypeConfiguration<IdentityUserClaim<int>> {
    
        public void Configure(EntityTypeBuilder<IdentityUserClaim<int>> builder) {
        
            builder.ToTable("UserClaims");
        }
    }
}
