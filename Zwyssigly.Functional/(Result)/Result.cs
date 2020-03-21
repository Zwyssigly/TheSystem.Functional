using System;

namespace Zwyssigly.Functional
{
    public class Result<TOk, TErr>
    {
        private readonly TOk _ok;
        private readonly TErr _err;
        private readonly bool _isOk;

        public Option<TOk> Ok => Match(Option.Some, _ => None.Value);
        public Option<TErr> Err => Match(_ => None.Value, Option.Some);

        public bool IsOk => _isOk;
        public bool IsErr => !_isOk;

        private Result(TOk ok, TErr err, bool isOk)
        {
            _isOk = isOk;

            if (_isOk) _ok = ok;
            else _err = err;
        }

        internal static Result<TOk, TErr> Success(TOk ok)
        {
            return new Result<TOk, TErr>(ok, default, true);
        }

        internal static Result<TOk, TErr> Failure(TErr err)
        {
            return new Result<TOk, TErr>(default, err, false);
        }

        public TResult Match<TResult>(Func<TOk, TResult> ok, Func<TErr, TResult> err)
        {
            return _isOk
                ? ok(_ok)
                : err(_err);
        }

        public void Match(Action<TOk> ok, Action<TErr> err)
        {
            if (_isOk) ok(_ok);
            else err(_err);
        }

        public Result<TResult, TErr> MapOk<TResult>(Func<TOk, TResult> ok)
        {
            return Match(
                ok2 => Result<TResult, TErr>.Success(ok(ok2)),
                err => Result<TResult, TErr>.Failure(err));
        }

        public TOk UnwrapOrThrow()
        {
            return Match(ok => ok, err => throw new UnwrapException(err.ToString()));
        }

        public Result<TResult, TErr> AndThen<TResult>(Func<TOk, Result<TResult, TErr>> ok)
        {
            return Match(o => ok(o), err => Result<TResult, TErr>.Failure(err));
        }

        public static implicit operator Result<TOk, TErr>(Success<TOk> ok)
        {
            return Success(ok.Value);
        }

        public static implicit operator Result<TOk, TErr>(Failure<TErr> err)
        {
            return Failure(err.Value);
        }

        public void IfOk(Action<TOk> ok) => Match(ok2 => ok(ok2), _ => { });
        public void IfErr(Action<TErr> err) => Match(_ => { }, err2 => err(err2));
    }

    public static class Result
    {
        private static readonly Success<Unit> _unitSuccess = new Success<Unit>(Zwyssigly.Functional.Unit.Value);

        public static Success<TOk> Success<TOk>(TOk ok)
            => new Success<TOk>(ok);

        public static Result<TOk, TErr> Success<TOk, TErr>(TOk ok)
            => Result<TOk, TErr>.Success(ok);

        public static Failure<TErr> Failure<TErr>(TErr err)
            => new Failure<TErr>(err);

        public static Result<TOk, TErr> Failure<TOk, TErr>(TErr err)
            => Result<TOk, TErr>.Failure(err);

        public static Success<Unit> Unit() => _unitSuccess;

        public static Result<Unit, TErr> Unit<TErr>() => Result<Unit, TErr>.Success(Zwyssigly.Functional.Unit.Value);
    }
}
