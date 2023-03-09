
using System;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Segurplan.FrameworkExtensions.EntityFramework.ValueGenerators;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders {
    public static class ValueGeneratorExtensions {
        public static PropertyBuilder<TProperty> HasValueGenerator<TProperty, TGenerator>(this PropertyBuilder<TProperty> builder, ValueGeneratorBehavior valueGeneratorBehavior)
            where TGenerator : ValueGenerator {
            builder.Metadata.AddAnnotation(nameof(ValueGeneratorBehavior), valueGeneratorBehavior);
            builder.HasValueGenerator<TGenerator>();

            return builder;
        }

        public static PropertyBuilder<TProperty> HasValueGenerator<TProperty>(this PropertyBuilder<TProperty> builder, Func<IProperty, IEntityType, ValueGenerator<TProperty>> factory, ValueGeneratorBehavior valueGeneratorBehavior) {
            builder.Metadata.AddAnnotation(nameof(ValueGeneratorBehavior), valueGeneratorBehavior);
            builder.HasValueGenerator(factory);

            return builder;
        }
    }
}
