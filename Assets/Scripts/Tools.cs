using System;
using System.Collections.Generic;

namespace Tools
{
    public static class StrExtensions
    {

        public static string ToUpperFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        public static string ToLowerFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            char[] a = s.ToCharArray();
            a[0] = char.ToLower(a[0]);
            return new string(a);
        }

        public static bool Contains(this string source, string value, StringComparison comp)
        {
            return source?.IndexOf(value, comp) >= 0;
        }

    }

    public static class DicExtensions
    {
        public static int RemoveAll<T, T2>(this Dictionary<T, T2> dicctionary, IEnumerable<T> keys)
        {
            int count = 0;

            foreach (var key in keys)
                if (dicctionary.Remove(key))
                    count++;

            return count;
        }
    }

    public static class ListExtensions
    {
        public static int RemoveAll<T>(this List<T> list, IEnumerable<T> keys)
        {
            int count = 0;

            foreach (var key in keys)
                if (list.Remove(key))
                    count++;

            return count;
        }
    }
}