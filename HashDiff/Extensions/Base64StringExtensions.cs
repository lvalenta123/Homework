namespace HashDiff.Extensions;

/// <summary>
/// Class with extensions for string class. Enables basic operations with Base64 encoded strings
/// </summary>
public static class Base64StringExtensions
{
    /// <summary>
    /// Checks if string contains valid Base64 values
    /// </summary>
    /// <param name="base64string">String to check</param>
    /// <returns>True if string is valid Base64 encoded string</returns>
    public static bool IsValidBase64String(this string base64string)
    {
        Span<byte> bytes = new Span<byte>(new byte[base64string.Length]);
        return Convert.TryFromBase64String(base64string, bytes, out _);
    }
    
    /// <summary>
    /// Encodes string to Base64.
    /// I planned to use this method for unit tests.
    /// </summary>
    /// <param name="data">String to encode</param>
    /// <returns>Base64 encoded string</returns>
    public static string Base64Encode(this string data)
    {
        var inArray = System.Text.Encoding.UTF8.GetBytes(data);
        return Convert.ToBase64String(inArray);
    }
    
    /// <summary>
    /// Decodes Base64 to string
    /// </summary>
    /// <param name="base64string">String to decode</param>
    /// <returns>UTF-8 string decoded from Base64</returns>
    public static string Base64Decode(this string base64string)
    {
        var bytes = System.Convert.FromBase64String(base64string);
        return System.Text.Encoding.UTF8.GetString(bytes);
    }
}