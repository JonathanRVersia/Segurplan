using System.Collections.Generic;
using System.Linq;

namespace Segurplan.FrameworkExtensions.Query {
    public class CombinedSpecification<T> : Specification<T> {
        public CombinedSpecification(IEnumerable<ISpecification<T>> specifications) {
            Specifications = specifications;

            criteriaExpressions.AddRange(specifications.SelectMany(s => s.CriteriaExpressions));
            orderByExpressions.AddRange(specifications.SelectMany(s => s.OrderByExpressions));
            orderByDescendingExpressions.AddRange(specifications.SelectMany(s => s.OrderByDescendingExpressions));

            IsPagingEnabled = specifications.Any(s => s.IsPagingEnabled);

            if (IsPagingEnabled) {
                Skip = specifications.Where(s => s.IsPagingEnabled).Last().Skip;
                Take = specifications.Where(s => s.IsPagingEnabled).Last().Take;
            }
        }

        public IEnumerable<ISpecification<T>> Specifications { get; }
    }
}
