using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zwyssigly.Functional
{
    public static class ResultEnumerableExtensions
    {
        public static IEnumerable<TOk> SelectOk<TOk, TErr>(this IEnumerable<Result<TOk, TErr>> self)
        {
            return self.Where(r => r.IsOk).Select(r => r.UnwrapOrThrow());
        }

        public static IEnumerable<TErr> SelectErr<TOk, TErr>(this IEnumerable<Result<TOk, TErr>> self)
        {
            return self.Where(r => r.IsErr).Select(r => r.Err.UnwrapOrThrow());
        }
    }
}
