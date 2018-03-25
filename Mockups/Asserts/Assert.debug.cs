namespace IngameScript.Mockups.Asserts
{
    public static class Assert
    {
        public static void That(bool condition, string message)
        {
            if (!condition)
                throw new AssertionException(message);
        }
    }
}
