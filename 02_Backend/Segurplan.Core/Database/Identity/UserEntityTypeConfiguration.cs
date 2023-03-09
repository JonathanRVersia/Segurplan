using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Segurplan.Core.Domain.Identity;

namespace Segurplan.Core.Database.EntityTypeConfigurations.Identity {
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User> {
        public void Configure(EntityTypeBuilder<User> builder) {
            builder.ToTable("Users");
        }
    }
}
