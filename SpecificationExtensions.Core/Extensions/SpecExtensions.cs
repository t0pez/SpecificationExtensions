using System;
using System.Linq.Expressions;
using Ardalis.Specification;
using SpecificationExtensions.Core.Specifications;

namespace SpecificationExtensions.Core.Extensions
{
    public static class SpecExtensions
    {
        public static Specification<TModel, TResult> Select<TModel, TResult>(
            this Specification<TModel> spec, Expression<Func<TModel, TResult>> selector)
        {
            var selectSpec = new SelectSpec<TModel, TResult>(spec, selector);

            return selectSpec;
        }
    }
}