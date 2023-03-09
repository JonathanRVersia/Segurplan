using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Segurplan.Core.Database.EntityTypeConfigurations.Identity {
  
    public class IdentityUserTokenEntityTypeConfiguration : IEntityTypeConfiguration<IdentityUserToken<int>> {
    
        public void Configure(EntityTypeBuilder<IdentityUserToken<int>> builder) {
        
            builder.ToTable("UserTokens");
        }
    }
}
