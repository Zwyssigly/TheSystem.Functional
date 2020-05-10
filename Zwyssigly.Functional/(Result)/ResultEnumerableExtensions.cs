using System.Collections.Generic;
using System.Linq;

namespace Zwyssigly.Functional
{
    public static class ResultEnumerableExtensions
    {
        public static IEnumerable<TSuccess> SelectSuccess<TSuccess, TFailure>(this IEnumerable<Result<TSuccess, TFailure>> self)
        {
            return self.Where(r => r.IsSuccess).Select(r => r.UnwrapOrThrow());
        }

        public static IEnumerable<TFailure> SelectFailure<TSuccess, TFailure>(this IEnumerable<Result<TSuccess, TFailure>> self)
        {
            return self.Where(r => r.IsFailure).Select(r => r.Failure.UnwrapOrThrow());
        }

        public static Result<IReadOnlyList<TSuccess>, TFailure> Railway<TSuccess, TFailure>(this IEnumerable<Result<TSuccess, TFailure>> self)
        {
            var list = new List<TSuccess>();

            foreach (var result in self)
            {
                if (result.IsFailure)
                    return result.MapSuccess<IReadOnlyList<TSuccess>>(_ => list);

                result.IfSuccess(list.Add);
            }

            return Result.Success<IReadOnlyList<TSuccess>, TFailure>(list);
        }
    }
}
