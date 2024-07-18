using HashDiff.Extensions;
using Newtonsoft.Json;

namespace HashDiff.Models;

/// <summary>
/// Represents JSON that is provided in the encoded Base64 string
/// </summary>
public class DiffPostRequest
{
    /// <summary>
    /// Diff part to be stored
    /// </summary>
    public string Input { get; set; }

    /// <summary>
    /// Creates new instance of <see cref="DiffPostRequest"/> from Base64 string
    /// </summary>
    /// <param name="base64">Base64 string containing JSON with input field</param>
    /// <returns>New instnace of <see cref="DiffPostRequest"/></returns>
    /// <exception cref="ArgumentException">Thrown if Base64 is not valid or there is no JSON with input field in the provided string</exception>
    public static DiffPostRequest CreateFromBase64(string base64)
    {
        return JsonConvert.DeserializeObject<DiffPostRequest>(base64.Base64Decode()) 
               ?? throw new ArgumentException($"Provided string is not Base64 encoded or it does not contain {nameof(DiffPostRequest)} instance");
    }
}