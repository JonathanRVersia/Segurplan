using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Segurplan.FrameworkExtensions.Reflection {
    public class DelegateFactory {
        private readonly Dictionary<object, Delegate> cache;

        public DelegateFactory() {
            cache = new Dictionary<object, Delegate>();
        }

        public TDelegate CreateDelegate<TDelegate>(
            DelegateDescription delegateDescription,
            bool cacheDelegate = true,
            Func<DelegateDescription, object> buildCacheKey = null)
            where TDelegate : Delegate {
            return (TDelegate)CreateDelegate(typeof(TDelegate), delegateDescription, cacheDelegate, buildCacheKey);
        }

        public Delegate CreateDelegate(
            Type delegateType,
            DelegateDescription delegateDescription,
            bool cacheDelegate = true,
            Func<DelegateDescription, object> buildCacheKey = null) {
            var cacheKey = buildCacheKey?.Invoke(delegateDescription) ?? new {
                delegateDescription.MethodName,
                delegateDescription.ParameterTypes,
                delegateDescription.TargetType,
                delegateDescription.Reusable,
                delegateDescription.GenericParameters
            };

            if (cacheDelegate && cache.ContainsKey(cacheKey))
                return cache[cacheKey];

            Expression instance = null;

            if (delegateDescription.Reusable)
                instance = Expression.Parameter(delegateDescription.TargetType);
            else
                instance = Expression.Constant(delegateDescription.Instance);

            var parameters = delegateDescription.ParameterTypes.Any() ? delegateDescription.ParameterTypes.Select(p => Expression.Parameter(p)).ToArray() : null;

            Expression methodCall;

            if (delegateDescription.Instance != null || delegateDescription.Reusable)
                methodCall = Expression.Call(instance, delegateDescription.MethodName, delegateDescription.GenericParameters, parameters);
            else
                methodCall = Expression.Call(delegateDescription.TargetType, delegateDescription.MethodName, delegateDescription.GenericParameters, parameters);

            if (delegateDescription.Reusable)
                parameters = Enumerable.Concat(new[] { (ParameterExpression)instance }, parameters ?? Enumerable.Empty<ParameterExpression>()).ToArray();

            var methodCallDelegate = Expression.Lambda(delegateType, methodCall, parameters).Compile();

            if (cacheDelegate)
                cache.Add(cacheKey, methodCallDelegate);

            return methodCallDelegate;
        }

        public class DelegateDescription {
            public bool Reusable { get; set; }
            public string MethodName { get; set; }
            public object Instance { get; set; }
            public Type TargetType { get; set; }
            public IEnumerable<Type> ParameterTypes { get; set; } = Enumerable.Empty<Type>();
            public Type[] GenericParameters { get; set; }
        }
    }
}
