using System.Text.RegularExpressions;


public static class StringExtensions
{
    public static string SubstringFromToIncluding(this string s, char startChar, char endChar)
    {
        int startSubstringIndex = s.IndexOf(startChar);
        int endSubstringIndex = s.IndexOf(endChar);
        int length = endSubstringIndex - startSubstringIndex + 1;
        return s.Substring(startSubstringIndex, length);
    }

    public static string SubstringFromToExcluding(this string s, char startChar, char endChar)
    {
        int startSubstringIndex = s.IndexOf(startChar);
        int endSubstringIndex = s.IndexOf(endChar);
        int length = endSubstringIndex - startSubstringIndex - 1;
        return s.Substring(startSubstringIndex + 1, length);
    }


    /// <summary>
    /// Inserts a space before every capital letter.
    /// </summary>
    /// <param name="src">A string wouthout spaces.</param>
    /// <param name="canContainAcronyms">Tries to recognize acyronyms to avoid splitting them. This may cause issues when an acronym is either lead or followed by a one-letter word (e.g. I, a).</param>
    /// <returns>A string with spaces.</returns>
    public static string InsertSpaces(this string src, bool canContainAcronyms = false)
    {
        if (canContainAcronyms)
        {
            return Regex.Replace(src, @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0");
        }
        else
        {
            return Regex.Replace(Regex.Replace(src, @"(\p{L})(\p{Lu})", "$1 $2"), @"(\p{L})(\p{Lu})", "$1 $2");
        }
    }
}