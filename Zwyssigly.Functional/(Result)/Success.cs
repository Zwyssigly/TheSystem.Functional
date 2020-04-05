namespace Zwyssigly.Functional
{
    public class Success<T>
    {
        public readonly T Value;

        internal Success(T value) => Value = value;
    }
}
