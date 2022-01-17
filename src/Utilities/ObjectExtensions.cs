using System.Collections.Generic;

namespace coveralls_uploader.Utilities
{
    public static class ObjectExtensions
    {
        public static IList<T> InList<T>(this T item)
        {
            return new List<T> { item };
        }
    }
}