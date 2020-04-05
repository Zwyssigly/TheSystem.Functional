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

        public static Option<T> FirstOrNone<T>(this IEnumerable<T> self, Func<T, bool> predicate)
        {
            var array = self.Where(predicate).Take(1).ToArray();

            return array.Length > 0
                ? Option.Some(array[0])
                : Option.None<T>();
        }

        public static Option<T> SingleOrNone<T>(this IEnumerable<T> self)
        {
            var array = self.Take(2).ToArray();
            if (array.Length > 1)
                throw new InvalidOperationException("Sequence contains more than one matching element.");

            return array.Length > 0
                ? Option.Some(array[0])
                : Option.None<T>();
        }

        public static Option<T> SingleOrNone<T>(this IEnumerable<T> self, Func<T, bool> predicate)
        {
            var array = self.Where(predicate).Take(2).ToArray();
            if (array.Length > 1)
                throw new InvalidOperationException("Sequence contains more than one matching element.");

            return array.Length > 0
                ? Option.Some(array[0])
                : Option.None<T>();
        }

        public static Option<TValue> GetValueOrOption<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> self, TKey key)
        {
            return self.TryGetValue(key, out var value)
                ? Option.Some(value)
                : Option.None<TValue>();
        }
    }
}
