using System;
using System.Collections.Generic;
using Ardalis.Specification;
using SpecificationExtensions.Core.Extensions;

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

        public BaseSpec<TModel> ExceptBy<TKey>(IEnumerable<TModel> values, Func<TModel, TKey> predicate)
        {
            foreach (var value in values)
            {
                Query
                    .Where(model => predicate(model).Equals(predicate(value)) == false);
            }

            return this;
        }
        
        public BaseSpec<TModel> ExceptBy<TKey>(IEnumerable<TKey> values, Func<TModel, TKey> predicate)
        {
            foreach (var value in values)
            {
                Query
                    .Where(model => predicate(model).Equals(value) == false);
            }
            
            return this;
        }
    }
}