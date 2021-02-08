namespace Tools
{
    public static class StrTools
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

    }

}
