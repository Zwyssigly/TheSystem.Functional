using System;
using System.Threading.Tasks;

namespace Zwyssigly.Functional
{
    public static class OptionTaskExtensions
    {
        public static async Task<Option<TResult>> AndThenAsync<T, TResult>(
            this Task<Option<T>> self, Func<T, Task<Option<TResult>>> onSome)
        {
            var option = await self.ConfigureAwait(false);
            return await option.Match(
                s => onSome(s),
                () => Task.FromResult(Option.None<TResult>())).ConfigureAwait(false);
        }

        public static async Task<Option<TResult>> AndThen<T, TResult>(
            this Task<Option<T>> self, Func<T, Option<TResult>> onSome)
        {
            var option = await self.ConfigureAwait(false);
            return option.AndThen(onSome);                
        }

        public static Task<Option<TResult>> AndThenAsync<T, TResult>(
            this Option<T> self, Func<T, Task<Option<TResult>>> onSome)
        {
            return self.Match(
                s => onSome(s),
                () => Task.FromResult(Option.None<TResult>()));
        }

        public static async Task<TResult> MatchAsync<T, TResult>(
            this Task<Option<T>> self, Func<T, Task<TResult>> onSome, Func<Task<TResult>> onNone)
        {
            var option = await self.ConfigureAwait(false);
            return await option.Match(
                s => onSome(s),
                () => onNone()).ConfigureAwait(false);
        }

        public static async Task<TResult> Match<T, TResult>(
            this Task<Option<T>> self, Func<T, TResult> onSome, Func<TResult> onNone)
        {
            var option = await self.ConfigureAwait(false);
            return option.Match(onSome, onNone);
        }

        public static async Task MatchAsync<T>(
            this Task<Option<T>> self, Func<T, Task> onSome, Func<Task> onNone)
        {
            var option = await self.ConfigureAwait(false);
            await option.Match(
                s => onSome(s),
                () => onNone()).ConfigureAwait(false);
        }

        public static async Task Match<T>(
            this Task<Option<T>> self, Action<T> onSome, Action onNone)
        {
            var option = await self.ConfigureAwait(false);
            option.Match(onSome, onNone);
        }

        public static async Task IfSomeAsync<T>(this Task<Option<T>> self, Func<T, Task> onSome)
        {
            var option = await self.ConfigureAwait(false);
            await option.IfSomeAsync(onSome);
        }

        public static async Task IfSome<T>(this Task<Option<T>> self, Action<T> onSome)
        {
            var option = await self.ConfigureAwait(false);
            option.IfSome(onSome);
        }

        public static Task IfSomeAsync<T>(this Option<T> self, Func<T, Task> onSome)
        {
            return self.Match(s => onSome(s), () => Task.FromResult(0));
        }

        public static async Task IfNoneAsync<T>(this Task<Option<T>> self, Func<Task> onNone)
        {
            var option = await self.ConfigureAwait(false);
            await option.IfNoneAsync(onNone);
        }

        public static async Task IfSome<T>(this Task<Option<T>> self, Action onNone)
        {
            var option = await self.ConfigureAwait(false);
            option.IfNone(onNone);
        }

        public static Task IfNoneAsync<T>(this Option<T> self, Func<Task> onNone)
        {
            return self.Match(_ => Task.FromResult(0), () => onNone());
        }
    }
}
