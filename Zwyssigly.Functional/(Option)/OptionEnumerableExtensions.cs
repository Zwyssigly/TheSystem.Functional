using System;
using System.Collections.Generic;
using System.Linq;

namespace Zwyssigly.Functional
{
    public static class OptionEnumerableExtensions
    {
        public static Option<T> FirstOrNone<T>(this IEnumerable<T> self)
        {
            var array = self.Take(1).ToArray();

            return array.Length > 0
                ? Option.Some(array[0])
                : Option.None<T>();
        }
    }
}
