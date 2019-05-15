using System;
using System.Collections;
using System.Collections.Generic;

namespace CSPlaypen.Extensions
{
    public static class IEnumerableExtensions
    {
        public static void Print<T>(this IEnumerable<T> enumerable, string name="")
        {
            var result = "";
            if (!string.IsNullOrEmpty(name))
            {
                result += $"'{name}': ";
            }
            result += $"{{\n    {string.Join(",\n    ", enumerable)}\n}}";

            Console.WriteLine(result);
        }
    }
}