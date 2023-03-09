using System.Collections.Generic;

namespace Segurplan.FrameworkExtensions.Query {
    public static class SpecificationExtensions {
        public static ISpecification<T> Combine<T>(this IEnumerable<ISpecification<T>> specifications) {
            return new CombinedSpecification<T>(specifications);
        }
    }
}
