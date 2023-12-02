namespace AdventOfCode2023.Day1;

public static class StringExtensions
{
    public static string ReverseString(this string text)
    {
        var array = text.ToCharArray();
        Array.Reverse(array);
        return new string(array);
    }
}