using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Segurplan.FrameworkExtensions.Query {
    public abstract class Specification<T> : ISpecification<T> {
        protected readonly List<Expression<Func<T, bool>>> criteriaExpressions;
        protected readonly List<Expression<Func<T, object>>> orderByExpressions;
        protected readonly List<Expression<Func<T, object>>> orderByDescendingExpressions;

        public IEnumerable<Expression<Func<T, bool>>> CriteriaExpressions => criteriaExpressions;
        public IEnumerable<Expression<Func<T, object>>> OrderByExpressions => orderByExpressions;
        public IEnumerable<Expression<Func<T, object>>> OrderByDescendingExpressions => orderByDescendingExpressions;

        public int Take { get; protected set; }
        public int Skip { get; protected set; }
        public bool IsPagingEnabled { get; protected set; }

        public Specification() {
            criteriaExpressions = new List<Expression<Func<T, bool>>>();
            orderByExpressions = new List<Expression<Func<T, object>>>();
            orderByDescendingExpressions = new List<Expression<Func<T, object>>>();
            IsPagingEnabled = false;
        }

        protected virtual Specification<T> Paginated(int skip, int take) {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;

            return this;
        }
        protected virtual Specification<T> OrderBy(Expression<Func<T, object>> orderByExpression) {
            orderByExpressions.Add(orderByExpression);

            return this;
        }
        protected virtual Specification<T> ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression) {
            orderByDescendingExpressions.Add(orderByDescendingExpression);

            return this;
        }

        protected virtual Specification<T> Criteria(Expression<Func<T, bool>> criteriaExpression) {
            criteriaExpressions.Add(criteriaExpression);

            return this;
        }
    }
}
