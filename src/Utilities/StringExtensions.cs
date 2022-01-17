namespace coveralls_uploader.Utilities
{
    public static class StringExtensions
    {
        public static string NullIfEmpty(this string @string)
        {
            return @string == string.Empty ? null : @string;
        }
        
        public static bool IsNullOrEmpty(this string @string)
        {
            return string.IsNullOrEmpty(@string);
        }
    }
}