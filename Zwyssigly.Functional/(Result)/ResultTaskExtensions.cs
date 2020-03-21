using System;
using System.Threading.Tasks;

namespace Zwyssigly.Functional
{
    public static class ResultTaskExtensions
    {
        public static async Task<Result<TResult, TErr>> AndThen<TOk, TErr, TResult>(this Task<Result<TOk, TErr>> self, Func<TOk, Task<Result<TResult, TErr>>> ok)
        {
            return await (await self).Match(
                o => ok(o),
                err => Task.FromResult<Result<TResult, TErr>>(Result.Failure(err)));
        }
    }
}
