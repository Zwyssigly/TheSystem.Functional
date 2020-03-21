using System;
using System.Threading.Tasks;

namespace Zwyssigly.Functional
{
    public static class OptionTaskExtensions
    {
        public static async Task<TResult> Match<T, TResult>(this Task<Option<T>> self, Func<T, Task<TResult>> some, Func<Task<TResult>> none)
        {
            return await (await self).Match(
                s => some(s),
                () => none());
        }
    }
}
