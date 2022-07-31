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

        public BaseSpec<TModel> ExceptBy(IEnumerable<TModel> values, Func<TModel, object> predicate)
        {
            Query
                .PostProcessingAction(models => models.ExceptBy(values, predicate));

            return this;
        }
    }
}