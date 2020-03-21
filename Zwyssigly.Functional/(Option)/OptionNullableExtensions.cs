
namespace Zwyssigly.Functional
{
    public static class OptionNullableExtensions
    {
        public static T? ToNullable<T>(this Option<T> self) where T : struct
        {
            return self.Match(some => (T?)some, () => default(T?));
        }

        public static Option<T> ToOption<T>(this T? self) where T : struct
        {
            return self.HasValue ? Option.Some(self.Value) : Option.None();
        }
    }
}