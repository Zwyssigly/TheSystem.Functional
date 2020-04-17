using System;
using System.Threading.Tasks;

namespace Zwyssigly.Functional
{
    public static class ResultTaskExtensions
    {
        public static async Task<Result<TResult, TFailure>> MapSuccessAsync<TSuccess, TFailure, TResult>(
            this Task<Result<TSuccess, TFailure>> self,
            Func<TSuccess, Task<TResult>> onSuccess)
        {
            var result = await self.ConfigureAwait(false);
            return await result.Match(
                async s => Result.Success<TResult, TFailure>(await onSuccess(s).ConfigureAwait(false)),
                f => Task.FromResult(Result.Failure<TResult, TFailure>(f))
            ).ConfigureAwait(false);
        }

        public static async Task<Result<TResult, TFailure>> AndThenAsync<TSuccess, TFailure, TResult>(
            this Task<Result<TSuccess, TFailure>> self, 
            Func<TSuccess, Task<Result<TResult, TFailure>>> onSuccess)
        {
            var result = await self.ConfigureAwait(false);
            return await result.Match(
                s => onSuccess(s),
                f => Task.FromResult<Result<TResult, TFailure>>(Result.Failure(f))
            ).ConfigureAwait(false);
        }

        public static async Task<TResult> MatchAsync<TSuccess, TFailure, TResult>(
            this Task<Result<TSuccess, TFailure>> self, 
            Func<TSuccess, Task<TResult>> onSuccess,
            Func<TFailure, Task<TResult>> onFailure)
        {
            var result = await self.ConfigureAwait(false);
            return await result.Match(
                s => onSuccess(s),
                f => onFailure(f)
            ).ConfigureAwait(false);
        }

        public static async Task MatchAsync<TSuccess, TFailure>(
            this Task<Result<TSuccess, TFailure>> self,
            Func<TSuccess, Task> onSuccess,
            Func<TFailure, Task> onFailure)
        {
            var result = await self.ConfigureAwait(false);
            await result.Match(
                s => onSuccess(s),
                f => onFailure(f)
            ).ConfigureAwait(false);
        }

        public static async Task IfSuccessAsync<TSuccess, TFailure>(
            this Task<Result<TSuccess, TFailure>> self, 
            Func<TSuccess, Task> onSuccess)
        {
            var option = await self.ConfigureAwait(false);
            await option.IfSuccessAsync(onSuccess);
        }

        public static Task IfSuccessAsync<TSuccess, TFailure>(
            this Result<TSuccess, TFailure> self, 
            Func<TSuccess, Task> onSuccess)
        {
            return self.Match(s => onSuccess(s), _ => Task.FromResult(0));
        }

        public static async Task IfFailureAsync<TSuccess, TFailure>(
            this Task<Result<TSuccess, TFailure>> self, 
            Func<TFailure, Task> onFailure)
        {
            var option = await self.ConfigureAwait(false);
            await option.IfFailureAsync(onFailure);
        }

        public static Task IfFailureAsync<TSuccess, TFailure>(
            this Result<TSuccess, TFailure> self, 
            Func<TFailure, Task> onFailure)
        {
            return self.Match(_ => Task.FromResult(0), e => onFailure(e));
        }
    }
}
