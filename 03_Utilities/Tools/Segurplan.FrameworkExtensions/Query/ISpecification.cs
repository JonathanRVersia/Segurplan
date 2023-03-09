using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Segurplan.FrameworkExtensions.Query {
    public interface ISpecification<TModel> {
        IEnumerable<Expression<Func<TModel, bool>>> CriteriaExpressions { get; }
        IEnumerable<Expression<Func<TModel, object>>> OrderByExpressions { get; }
        IEnumerable<Expression<Func<TModel, object>>> OrderByDescendingExpressions { get; }

        int Take { get; }
        int Skip { get; }
        bool IsPagingEnabled { get; }
    }
}
