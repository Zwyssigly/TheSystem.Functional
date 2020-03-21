using System;
using System.Collections.Generic;
using System.Text;

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

        public TResult Match<TResult>(Func<TA, TResult> a, Func<TB, TResult> b)
            => _discriminant switch
            {
                Discriminant.A => a(_a),
                Discriminant.B => b(_b),
                _ => throw new InvalidOperationException()
            };

        public void Match(Action<TA> a, Action<TB> b)
        {
            switch (_discriminant)
            {
                case Discriminant.A: a(_a); break;
                case Discriminant.B: b(_b); break;
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
