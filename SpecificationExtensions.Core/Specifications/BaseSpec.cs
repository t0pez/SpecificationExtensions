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
    }
}