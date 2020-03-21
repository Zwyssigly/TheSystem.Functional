namespace Zwyssigly.Functional
{
    public class Failure<TErr>
    {
        public readonly TErr Value;

        public Failure(TErr value) => Value = value;
    }
}
