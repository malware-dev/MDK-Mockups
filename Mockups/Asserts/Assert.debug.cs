using System;
using VRageMath;

namespace IngameScript.Mockups.Asserts
{
    public static class Assert
    {
        [Obsolete("Use " + nameof(Assert) + "." + nameof(True))]
        public static void That(bool condition, string message) => True(condition, message);

        public static void Fail(string message)
        {
            throw new AssertionException(message);
        }

        public static void True(bool condition, string message)
        {
            if (!condition)
                throw new AssertionException(message);
        }

        public static void False(bool condition, string message)
        {
            if (condition)
                throw new AssertionException(message);
        }

        public static void Equals<T>(T expected, T actual, string message)
            where T: IEquatable<T>
        {
            if (expected == null)
                throw new ArgumentNullException(nameof(expected));

            if (!expected.Equals(actual))
                throw new AssertionException($"Expected <{expected}> actual <{actual}>. {message}");
        }

        public static void Null(object actual, string message)
        {
            if (actual != null)
                throw new AssertionException($"Value is null. {message}");
        }

        public static void NotNull(object actual, string message)
        {
            if (actual == null)
                throw new AssertionException($"Value is null. {message}");
        }
    }
}
