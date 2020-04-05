using System;

namespace Zwyssigly.Functional
{
    public class Either<TA, TB>
    {
        private readonly TA _a;
        private readonly TB _b;
        private readonly Discriminant _discriminant;

        public Option<TA> A => _discriminant == Discriminant.A ? Option.Some(_a) : Option.None();
        public Option<TB> B => _discriminant == Discriminant.B ? Option.Some(_b) : Option.None();

        public Either(TA a)
        {
            _a = a;
            _discriminant = Discriminant.A;
        }

        public Either(TB b)
        {
            _b = b;
            _discriminant = Discriminant.B;
        }

        public TResult Match<TResult>(Func<TA, TResult> onA, Func<TB, TResult> onB)
            => _discriminant switch
            {
                Discriminant.A => onA(_a),
                Discriminant.B => onB(_b),
                _ => throw new InvalidOperationException()
            };

        public void Match(Action<TA> onA, Action<TB> onB)
        {
            switch (_discriminant)
            {
                case Discriminant.A: onA(_a); break;
                case Discriminant.B: onB(_b); break;
            }
        }

        public static implicit operator Either<TA, TB>(TA a) => new Either<TA, TB>(a);
        public static implicit operator Either<TA, TB>(TB b) => new Either<TA, TB>(b);
    }

    public enum Discriminant
    {
        A, B
    }
}
