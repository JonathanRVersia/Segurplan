using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Segurplan.FrameworkExtensions.Reflection;

namespace Segurplan.FrameworkExtensions.EntityFramework.Configurations {
    public class EntityTypeConfigurationCollection : IEnumerable<Type> {
        private readonly IEnumerable<Type> configurations;
        private readonly IServiceProvider serviceProvider;

        public EntityTypeConfigurationCollection(
            IServiceProvider serviceProvider,
            params Type[] configurations)
            : this(serviceProvider, configurations.AsEnumerable()) {
        }

        public EntityTypeConfigurationCollection(
            IServiceProvider serviceProvider,
            IEnumerable<Type> configurations) {
            this.serviceProvider = serviceProvider;
            this.configurations = configurations;
        }

        public void ApplyTo(ModelBuilder builder) {
            var delegateFactory = serviceProvider.GetService<DelegateFactory>();

            foreach (var configurationType in configurations) {
                var entityType = configurationType.GetTypeInfo().ImplementedInterfaces
                                              .Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                                              .GenericTypeArguments
                                              .Single();

                var applyConfigurationDelegateType = Expression.GetActionType(typeof(ModelBuilder), configurationType);

                var applyConfiguration = delegateFactory.CreateDelegate(applyConfigurationDelegateType, new DelegateFactory.DelegateDescription() {
                    MethodName = nameof(ModelBuilder.ApplyConfiguration),
                    GenericParameters = new[] { entityType },
                    ParameterTypes = new[] { configurationType },
                    TargetType = typeof(ModelBuilder),
                    Reusable = true
                });

                applyConfiguration.DynamicInvoke(builder, ActivatorUtilities.CreateInstance(serviceProvider, configurationType));
            }
        }

        public IEnumerator<Type> GetEnumerator() {
            return configurations.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return configurations.GetEnumerator();
        }
    }
}
