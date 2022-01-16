namespace coveralls_uploader.Utilities
{
    public static class StringExtensions
    {
        public static string NullIfEmpty(this string value)
        {
            return value == string.Empty ? null : value;
        }
    }
}