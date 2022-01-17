using System;

namespace coveralls_uploader.Utilities
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string @string)
        {
            return string.IsNullOrEmpty(@string);
        }

        public static string TrimEndNewLine(this string @string)
        {
            return @string.IsNullOrEmpty() ? @string : @string.TrimEnd(Environment.NewLine.ToCharArray());
        }
    }
}