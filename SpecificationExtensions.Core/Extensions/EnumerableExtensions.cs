using System;
using System.Collections.Generic;
using System.Linq;

namespace SpecificationExtensions.Core.Extensions
{
    internal static class EnumerableExtensions
    {
        public static IEnumerable<TSource> ExceptBy<TSource, TKey>(this IEnumerable<TSource> source,
                                                                   IEnumerable<TSource> exceptItems,
                                                                   Func<TSource, TKey> predicate)
        {
            var keys = new HashSet<TKey>(exceptItems.Select(predicate));

            foreach (var element in source)
            {
                var key = predicate(element);

                if (keys.Add(key) == false)
                {
                    continue;
                }

                yield return element;
            }
        }
    }
}