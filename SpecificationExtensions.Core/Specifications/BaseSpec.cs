using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ardalis.Specification;

namespace SpecificationExtensions.Core.Specifications
{
    public class BaseSpec<TModel> : Specification<TModel>
    {
        public BaseSpec<TModel> EnablePaging(int skip, int take)
        {
            Query
                .Skip(skip)
                .Take(take);

            return this;
        }

        public BaseSpec<TModel> ExceptBy<TKey>(IEnumerable<TKey> values, Expression<Func<TModel, TKey>> selector)
            {
                foreach (var value in values)
                {
                    var lambda = GetNotEqualExpression(selector, value);
        
                    Query
                        .Where(lambda);
                }
                
                return this;
            }
            
            public BaseSpec<TModel> ExceptBy<TKey>(IEnumerable<TModel> models, Expression<Func<TModel, TKey>> selector)
            {
                var compiledSelector = selector.Compile();
                var values = models.Select(compiledSelector);
        
                ExceptBy(values, selector);
                
                return this;
            }
        
            private Expression<Func<TModel, bool>> GetNotEqualExpression<TKey>(Expression<Func<TModel, TKey>> predicate, TKey value)
            {
                var parameterExpression = predicate.Parameters.Single();
                var constantExpression = Expression.Constant(value);
                var notEqualExpression = Expression.NotEqual(predicate.Body, constantExpression);
                var lambda = Expression.Lambda<Func<TModel, bool>>(notEqualExpression, parameterExpression);
                
                return lambda;
            }
    }
}