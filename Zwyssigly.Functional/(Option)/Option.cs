using System;

namespace Zwyssigly.Functional
{
    public interface IOption
    {
        TResult Match<TResult>(Func<object, TResult> onSome, Func<TResult> onNone);
        void Match(Action<object> onSome, Action onNone);
        object UnwrapOrDefault();
    }

    public struct Option<T> : IOption
    {
        private T _value;
        private bool _hasValue;

        public bool IsSome => _hasValue;
        public bool IsNone => !_hasValue;

        public Option(T value) : this()
        {
            if (value != null)
            {
                _value = value;
                _hasValue = true;
            }
        }

        public void Match(Action<T> onSome, Action onNone)
        {
            if (_hasValue)
                onSome(_value);
            else onNone();
        }

        public TResult Match<TResult>(Func<T, TResult> onSome, Func<TResult> onNone)
        {
            return _hasValue
                ? onSome(_value)
                : onNone();
        }

        public T UnwrapOrThrow()
        {
            return Match(some => some, () => throw new UnwrapException("Can not unwrap none!"));
        }

        public T UnwrapOrDefault() => Match(some => some, () => default);

        public T UnwrapOr(Func<T> onNone) => Match(some => some, () => onNone());

        public T UnwrapOr(T none) => Match(some => some, () => none);

        public Option<T> OrThen(Func<Option<T>> onNone) => Match(Option.Some, onNone);

        public Option<TResult> Map<TResult>(Func<T, TResult> onSome) => Match(x => Option.Some(onSome(x)), () => Option.None());

        TResult IOption.Match<TResult>(Func<object, TResult> onSome, Func<TResult> onNone) => Match(some2 => onSome(some2), () => onNone());

        void IOption.Match(Action<object> onSome, Action onNone) => Match(some2 => onSome(some2), () => onNone());

        object IOption.UnwrapOrDefault() => UnwrapOrDefault();

        public static implicit operator Option<T>(None _) => default;

        public override string ToString() => Match(some => some.ToString(), () => "[none]");

        public Result<T, TFailure> ToResult<TFailure>(Func<TFailure> onNone)
        {
            return Match(
                some => Result.Success<T, TFailure>(some), 
                () => Result.Failure<T, TFailure>(onNone())
            );
        }

        public void IfSome(Action<T> onSome) => Match(some2 => onSome(some2), () => { });
        public void IfNone(Action onNone) => Match(_ => { }, onNone);

        public Option<TResult> AndThen<TResult>(Func<T, Option<TResult>> onSome)
        {
            return Match(s => onSome(s), () => Option.None<TResult>());
        }
    }

    public static class Option
    {
        public static Option<T> Some<T>(T value) => new Option<T>(value);

        public static None None() => Functional.None.Value;

        public static Option<T> None<T>() => default;

        public static Option<T> AsOption<T>(this TryParseDelegate<T> self, string s)
        {
            return self(s, out var value) ? Some(value) : None();
        }
    }

    public delegate bool TryParseDelegate<T>(string s, out T result);
}
