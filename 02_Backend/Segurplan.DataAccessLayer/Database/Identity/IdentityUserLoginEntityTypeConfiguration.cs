using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Segurplan.Core.Database.EntityTypeConfigurations.Identity {
 
    public class IdentityUserLoginEntityTypeConfiguration : IEntityTypeConfiguration<IdentityUserLogin<int>> {
    
        public void Configure(EntityTypeBuilder<IdentityUserLogin<int>> builder) {
        
            builder.ToTable("UserLogins");
        }
    }
}
