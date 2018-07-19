using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ATP.Common.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveDiacritics(this string s)
        {
            var normalizedString = s.Normalize(NormalizationForm.FormD);
            var output = new StringBuilder();

            foreach (var c in normalizedString.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark))
                output.Append(c);

            return output.ToString();
        }

        public static string ReplaceNewLine(this string text)
        {
            return text.Replace("\n", "</br>");
        }

        public static string ToCamelCase(this string s)
        {
            if (s == null)
                return "";

            if (s.Length < 2)
                return s.ToLowerInvariant();

            var output = Char.ToLowerInvariant(s[0]) + s.Substring(1);
            return output;
        }
    }
}
