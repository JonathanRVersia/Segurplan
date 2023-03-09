using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Segurplan.FrameworkExtensions.EntityFramework.Query;
using Segurplan.FrameworkExtensions.Query;

namespace Microsoft.EntityFrameworkCore {
    public static class SpecificationQueryableExtensions {
        public static Task<QueryResult<T>> RunSpecification<T>(this IQueryable<T> query, params ISpecification<T>[] specifications) {
            return RunSpecification(query, specifications?.AsEnumerable());
        }

        public static Task<QueryResult<T>> RunSpecification<T>(this IQueryable<T> query, IEnumerable<ISpecification<T>> specifications) {
            return RunSpecification(query, specifications?.Combine());
        }

        public static async Task<QueryResult<T>> RunSpecification<T>(this IQueryable<T> query, ISpecification<T> specification) {
            if (specification == null) {
                return new QueryResult<T>() {
                    Results = await query.ToListAsync()
                };
            }

            var queryResult = new QueryResult<T>();

            if (specification.IsPagingEnabled) {
                var countQuery = ApplyCriteria(query, specification);
                queryResult.TotalCount = await countQuery.CountAsync();
                queryResult.PageSize = specification.Take;
                queryResult.SkippedRows = specification.Skip;
                queryResult.IsPaginated = true;
            }

            queryResult.Results = await ApplySpecification(query, specification).ToListAsync();

            return queryResult;
        }

        /// <summary>
        /// Use when OrderBy or Criteria statements are applied on List with nested List of objects
        /// </summary>
        public static QueryResult<T> RunSpecificationSync<T>(this IQueryable<T> query, IEnumerable<ISpecification<T>> specifications) {
            return RunSpecificationSync(query, specifications?.Combine());
        }

        public static QueryResult<T> RunSpecificationSync<T>(this IQueryable<T> query, ISpecification<T> specification) {
            if (specification == null) {
                return new QueryResult<T>() {
                    Results = query.ToList()
                };
            }

            var queryResult = new QueryResult<T>();

            if (specification.IsPagingEnabled) {
                var countQuery = ApplyCriteria(query, specification);
                queryResult.TotalCount = countQuery.Count();
                queryResult.PageSize = specification.Take;
                queryResult.SkippedRows = specification.Skip;
                queryResult.IsPaginated = true;
            }

            queryResult.Results = ApplySpecification(query, specification).ToList();

            return queryResult;
        }

        public static IQueryable<T> ApplySpecification<T>(this IQueryable<T> query, params ISpecification<T>[] specifications) {
            return ApplySpecification(query, specifications?.Combine());
        }

        public static IQueryable<T> ApplySpecification<T>(this IQueryable<T> query, IEnumerable<ISpecification<T>> specifications) {
            return ApplySpecification(query, specifications?.AsEnumerable());
        }

        public static IQueryable<T> ApplySpecification<T>(this IQueryable<T> query, ISpecification<T> specification) {
            if (specification == null)
                return query;

            var filterQuery = query;

            filterQuery = ApplyCriteria(filterQuery, specification);
            filterQuery = ApplyOrderBy(filterQuery, specification);
            filterQuery = ApplyPagination(filterQuery, specification);

            return filterQuery;
        }

        public static IQueryable<T> ApplyPagination<T>(IQueryable<T> query, ISpecification<T> specification) {
            if (!specification.IsPagingEnabled)
                return query;

            return query.Skip(specification.Skip)
                        .Take(specification.Take);
        }

        public static IQueryable<T> ApplyOrderBy<T>(IQueryable<T> query, ISpecification<T> specification) {
            IOrderedQueryable<T> orderedQuery = null;

            ApplyOrderBy();
            ApplyOrderByDescending();

            return orderedQuery ?? query;

            void ApplyOrderBy() {
                if (specification.OrderByExpressions == null)
                    return;

                foreach (var orderBy in specification.OrderByExpressions) {
                    if (orderedQuery == null)
                        orderedQuery = query.OrderBy(orderBy);
                    else
                        orderedQuery = orderedQuery.ThenBy(orderBy);
                }
            }

            void ApplyOrderByDescending() {
                if (specification.OrderByDescendingExpressions == null)
                    return;

                foreach (var orderByDescending in specification.OrderByDescendingExpressions) {
                    if (orderedQuery == null)
                        orderedQuery = query.OrderByDescending(orderByDescending);
                    else
                        orderedQuery = orderedQuery.ThenByDescending(orderByDescending);
                }
            }
        }

        public static IQueryable<T> ApplyCriteria<T>(IQueryable<T> query, ISpecification<T> specification) {
            if (specification.CriteriaExpressions == null)
                return query;

            var filteredQuery = query;
            foreach (var criteria in specification.CriteriaExpressions) {
                filteredQuery = filteredQuery.Where(criteria);
            }

            return filteredQuery;
        }
    }
}
