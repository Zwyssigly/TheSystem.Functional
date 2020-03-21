namespace Zwyssigly.Functional
{
    public class Success<TOk>
    {
        public readonly TOk Value;

        public Success(TOk value) => Value = value;
    }
}
