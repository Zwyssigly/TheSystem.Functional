using System;
using System.ComponentModel;
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

        public static async Task<Result<TResult, TFailure>> MapSuccess<TSuccess, TFailure, TResult>(
            this Task<Result<TSuccess, TFailure>> self,
            Func<TSuccess, TResult> onSuccess)
        {
            var result = await self.ConfigureAwait(false);
            return result.MapSuccess(onSuccess);
        }

        public static async Task<Result<TResult, TFailure>> AndThenAsync<TSuccess, TFailure, TResult>(
            this Task<Result<TSuccess, TFailure>> self, 
            Func<TSuccess, Task<Result<TResult, TFailure>>> onSuccess)
        {
            var result = await self.ConfigureAwait(false);
            return await result.AndThenAsync(onSuccess).ConfigureAwait(false);
        }

        public static async Task<Result<TResult, TFailure>> AndThen<TSuccess, TFailure, TResult>(
            this Task<Result<TSuccess, TFailure>> self,
            Func<TSuccess, Result<TResult, TFailure>> onSuccess)
        {
            var result = await self.ConfigureAwait(false);
            return result.AndThen(onSuccess);
        }

        public static Task<Result<TResult, TFailure>> AndThenAsync<TSuccess, TFailure, TResult>(
            this Result<TSuccess, TFailure> self,
            Func<TSuccess, Task<Result<TResult, TFailure>>> onSuccess)
        {
            return self.Match(
                s => onSuccess(s),
                f => Task.FromResult<Result<TResult, TFailure>>(Result.Failure(f))
            );
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

        public static async Task<TResult> Match<TSuccess, TFailure, TResult>(
            this Task<Result<TSuccess, TFailure>> self,
            Func<TSuccess, TResult> onSuccess,
            Func<TFailure, TResult> onFailure)
        {
            var result = await self.ConfigureAwait(false);
            return result.Match(onSuccess, onFailure);
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

        public static async Task Match<TSuccess, TFailure>(
            this Task<Result<TSuccess, TFailure>> self,
            Action<TSuccess> onSuccess,
            Action<TFailure> onFailure)
        {
            var result = await self.ConfigureAwait(false);
            result.Match(onSuccess, onFailure);
        }

        public static async Task IfSuccessAsync<TSuccess, TFailure>(
            this Task<Result<TSuccess, TFailure>> self, 
            Func<TSuccess, Task> onSuccess)
        {
            var result = await self.ConfigureAwait(false);
            await result.IfSuccessAsync(onSuccess);
        }

        public static async Task IfSuccess<TSuccess, TFailure>(
            this Task<Result<TSuccess, TFailure>> self,
            Action<TSuccess> onSuccess)
        {
            var result = await self.ConfigureAwait(false);
            result.IfSuccess(onSuccess);
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

        public static async Task IfFailure<TSuccess, TFailure>(
            this Task<Result<TSuccess, TFailure>> self,
            Action<TFailure> onFailure)
        {
            var result = await self.ConfigureAwait(false);
            result.IfFailure(onFailure);
        }

        public static Task IfFailureAsync<TSuccess, TFailure>(
            this Result<TSuccess, TFailure> self, 
            Func<TFailure, Task> onFailure)
        {
            return self.Match(_ => Task.FromResult(0), e => onFailure(e));
        }
    }
}
