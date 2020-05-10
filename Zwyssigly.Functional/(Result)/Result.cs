using System;

namespace Zwyssigly.Functional
{
    public class Result<TSuccess, TFailure>
    {
        private readonly TSuccess _success;
        private readonly TFailure _failure;
        private readonly bool _isSuccess;

        public Option<TSuccess> Success => Match(Option.Some, _ => None.Value);
        public Option<TFailure> Failure => Match(_ => None.Value, Option.Some);

        public bool IsSuccess => _isSuccess;
        public bool IsFailure => !_isSuccess;

        private Result(TSuccess success, TFailure failure, bool isSuccess)
        {
            _isSuccess = isSuccess;

            if (_isSuccess) _success = success;
            else _failure = failure;
        }

        internal static Result<TSuccess, TFailure> FromSuccess(TSuccess success)
        {
            return new Result<TSuccess, TFailure>(success, default, true);
        }

        internal static Result<TSuccess, TFailure> FromFailure(TFailure failure)
        {
            return new Result<TSuccess, TFailure>(default, failure, false);
        }

        public TResult Match<TResult>(Func<TSuccess, TResult> onSuccess, Func<TFailure, TResult> onFailure)
        {
            return _isSuccess
                ? onSuccess(_success)
                : onFailure(_failure);
        }

        public void Match(Action<TSuccess> onSuccess, Action<TFailure> onFailure)
        {
            if (_isSuccess) onSuccess(_success);
            else onFailure(_failure);
        }

        public Result<TResult, TFailure> MapSuccess<TResult>(Func<TSuccess, TResult> onSuccess)
        {
            return Match(
                success => Result<TResult, TFailure>.FromSuccess(onSuccess(success)),
                failure => Result<TResult, TFailure>.FromFailure(failure));
        }

        public Result<TSuccess, TResult> MapFailure<TResult>(Func<TFailure, TResult> onFailure)
        {
            return Match(
                success => Result<TSuccess, TResult>.FromSuccess(success),
                failure => Result<TSuccess, TResult>.FromFailure(onFailure(failure)));
        }

        public Result<TSuccessResult, TFailureResult> Map<TSuccessResult, TFailureResult>(Func<TSuccess, TSuccessResult> onSuccess, Func<TFailure, TFailureResult> onFailure)
        {
            return Match(
                success => Result<TSuccessResult, TFailureResult>.FromSuccess(onSuccess(success)),
                failure => Result<TSuccessResult, TFailureResult>.FromFailure(onFailure(failure)));
        }

        public TSuccess UnwrapOrThrow()
        {
            return Match(success => success, failure => throw new UnwrapException(failure.ToString()));
        }

        public TSuccess UnwrapOr(Func<TFailure, TSuccess> onFailure)
        {
            return Match(success => success, failure => onFailure(failure));
        }

        public Result<TResult, TFailure> AndThen<TResult>(Func<TSuccess, Result<TResult, TFailure>> onSuccess)
        {
            return Match(s => onSuccess(s), f => Result<TResult, TFailure>.FromFailure(f));
        }

        public static implicit operator Result<TSuccess, TFailure>(Success<TSuccess> success)
        {
            return FromSuccess(success.Value);
        }

        public static implicit operator Result<TSuccess, TFailure>(Failure<TFailure> failure)
        {
            return FromFailure(failure.Value);
        }

        public void IfSuccess(Action<TSuccess> onSuccess) => Match(success => onSuccess(success), _ => { });
        public void IfFailure(Action<TFailure> onFailure) => Match(_ => { }, failure => onFailure(failure));
    }

    public static class Result
    {
        private static readonly Success<Unit> _unitSuccess = new Success<Unit>(Functional.Unit.Value);

        public static Success<TSuccess> Success<TSuccess>(TSuccess success)
            => new Success<TSuccess>(success);

        public static Result<TSuccess, TFailure> Success<TSuccess, TFailure>(TSuccess success)
            => Result<TSuccess, TFailure>.FromSuccess(success);

        public static Failure<TFailure> Failure<TFailure>(TFailure failure)
            => new Failure<TFailure>(failure);

        public static Result<TSuccess, TFailure> Failure<TSuccess, TFailure>(TFailure failure)
            => Result<TSuccess, TFailure>.FromFailure(failure);

        public static Success<Unit> Unit() => _unitSuccess;

        public static Result<Unit, TFailure> Unit<TFailure>() => 
            Result<Unit, TFailure>.FromSuccess(Functional.Unit.Value);
    }
}
