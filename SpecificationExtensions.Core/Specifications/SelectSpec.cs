using System;
using System.Linq.Expressions;
using Ardalis.Specification;

namespace SpecificationExtensions.Core.Specifications
{
    public class SelectSpec<TModel, TResult> : Specification<TModel, TResult>
    {
        public SelectSpec(ISpecification<TModel> spec, Expression<Func<TModel, TResult>> selector)
        {
            foreach (var whereExpression in spec.WhereExpressions)
            {
                Query.Where(whereExpression.Filter);
            }

            foreach (var orderExpression in spec.OrderExpressions)
            {
                Query.OrderBy(orderExpression.KeySelector);
            }

            if (spec.PostProcessingAction != null)
            {
                Query.PostProcessingAction(spec.PostProcessingAction);
            }

            if (spec.Skip.HasValue)
            {
                Query.Skip(spec.Skip.Value);
            }

            if (spec.Take.HasValue)
            {
                Query.Take(spec.Take.Value);
            }

            Query.Select(selector);
        }
    }
}