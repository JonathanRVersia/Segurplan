using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Segurplan.Core.Domain.Audit;

namespace Segurplan.Infrastructure.EntityFramework.SqlServer {
    public static class SqlAuditableExtensions {
        public static EntityTypeBuilder<T> HasSqlAudit<T>(this EntityTypeBuilder<T> builder)
            where T : class, IAuditable {
            builder.Property(s => s.Creado).HasValueGenerator((Type)null);

            builder.Property(s => s.Creado)
                   .HasDefaultValueSql("SYSDATETIMEOFFSET()")
                   .ValueGeneratedOnAdd();

            return builder;
        }
    }
}
