using System;

namespace Zwyssigly.Functional
{
    public interface IOption
    {
        TResult Match<TResult>(Func<object, TResult> some, Func<TResult> none);
        void Match(Action<object> some, Action none);
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

        public void Match(Action<T> some, Action none)
        {
            if (_hasValue)
                some(_value);
            else none();
        }

        public TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none)
        {
            return _hasValue
                ? some(_value)
                : none();
        }

        public T UnwrapOrThrow()
        {
            return Match(some => some, () => throw new UnwrapException("Can not unwrap none!"));
        }

        public T UnwrapOrDefault() => Match(some => some, () => default);

        public T UnwrapOr(Func<T> none) => Match(some => some, () => none());

        public Option<T> OrThen(Func<Option<T>> none) => Match(Option.Some, none);

        public Option<TResult> Map<TResult>(Func<T, TResult> some) => Match(x => Option.Some(some(x)), () => Option.None());

        TResult IOption.Match<TResult>(Func<object, TResult> some, Func<TResult> none) => Match(some2 => some(some2), () => none());

        void IOption.Match(Action<object> some, Action none) => Match(some2 => some(some2), () => none());

        object IOption.UnwrapOrDefault() => UnwrapOrDefault();

        public static implicit operator Option<T>(None _) => default;

        public override string ToString() => Match(some => some.ToString(), () => "[none]");

        public void IfSome(Action<T> some) => Match(some2 => some(some2), () => { });
    }

    public static class Option
    {
        public static Option<T> Some<T>(T value) => new Option<T>(value);

        public static None None() => Zwyssigly.Functional.None.Value;

        public static Option<T> None<T>() => default;

        public static Option<T> AsOption<T>(this TryParseDelegate<T> self, string s)
        {
            return self(s, out var value) ? Some(value) : None();
        }
    }

    public delegate bool TryParseDelegate<T>(string s, out T result);
}
