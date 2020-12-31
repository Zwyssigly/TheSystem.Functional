using NUnit.Framework;

namespace Zwyssigly.Functional
{
    public class Tests
    {

        [Test]
        public void EqualityTest()
        {
            var noneOfInt = Option.None<int>();
            var noneOfString = Option.None<int>();

            var int1 = Option.Some(1);
            var int1copy = Option.Some(1);
            var str1 = Option.Some("1");
            var int2 = Option.Some(2);

            CheckEquality(noneOfInt, int1, false);
            CheckEquality(int2, int1, false);
            CheckEquality(int1, int1, true);
            CheckEquality(int1copy, int1, true);
            CheckEquality(noneOfInt, noneOfInt, true);

            CheckObjectEquality(noneOfInt, noneOfString, true);
            CheckObjectEquality(int1, str1, false);
        }

        private void CheckEquality<T>(Option<T> a, Option<T> b, bool expected)
        {
            Assert.AreEqual(a == b, expected);
            Assert.AreEqual(a != b, !expected);

            Assert.AreEqual(a.Equals(b), expected);
            Assert.AreEqual(b.Equals(a), expected);

            Assert.AreEqual(a.GetHashCode() == b.GetHashCode(), expected);
        }

        private void CheckObjectEquality(object a, object b, bool expected)
        {
            Assert.AreEqual(a.Equals(b), expected);
            Assert.AreEqual(b.Equals(a), expected);

            Assert.AreEqual(a.GetHashCode() == b.GetHashCode(), expected);
        }
    }
}

