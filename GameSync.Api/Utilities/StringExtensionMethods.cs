using System.Text.RegularExpressions;

namespace GameSync.Api.Utilities;

/// <summary>
/// String extension methods.
/// </summary>
public static class StringExtensionMethods
{
    /// <summary>
    /// Sensitive data fields to sanitize.
    /// </summary>
    private readonly static string[] _sensitiveDataFields = { "password", "newPassword" };

    /// <summary>
    /// Sanitizing logs from sensitive data and endline characters.
    /// </summary>
    /// <param name="str">Input string.</param>
    /// <returns>Sanitized logs.</returns>
    public static string LogsSanitize(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return str;
        }

        foreach (var field in _sensitiveDataFields)
        {
            str = Regex.Replace(str, $@"""{field}""\s*:\s*""[^""]*""", $"\"{field}\":\"***\"");
        }

        return str.Replace("\n", string.Empty)
            .Replace("\r", string.Empty)
            .Replace("\t", string.Empty);
    }
}
