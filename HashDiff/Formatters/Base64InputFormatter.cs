namespace HashDiff.Formatters;

using Microsoft.AspNetCore.Mvc.Formatters;

/// <summary>
/// Input formatted that processes incoming requests with Content-Type of application/custom
/// </summary>
public class Base64InputFormatter : InputFormatter
{
    public Base64InputFormatter()
    {
        SupportedMediaTypes.Add("application/custom");
    }
    
    public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
    {
        using var reader = new StreamReader(context.HttpContext.Request.Body);
        var base64str = await reader.ReadToEndAsync();
        return await InputFormatterResult.SuccessAsync(base64str);
    }
    
    protected override bool CanReadType(Type type)
    {
        return type == typeof(string);
    }
}