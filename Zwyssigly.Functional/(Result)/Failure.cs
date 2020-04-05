namespace Zwyssigly.Functional
{
    public class Failure<T>
    {
        public readonly T Value;

        internal Failure(T value) => Value = value;
    }
}
