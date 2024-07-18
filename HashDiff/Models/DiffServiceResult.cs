namespace HashDiff.Models;

/// <summary>
/// Represents result returned from business logic.
/// Special object so that <see cref="DiffService" doesn't return IActionResult/>
/// </summary>
public class DiffServiceResult
{
    public ResultState ResultState { get; set; }
    public string? Message { get; set; }
    public object? ObjectToReturn { get; set; }
    public string? ControllerName { get; set; }
    public string? ActionName { get; set; }
    public object? RouterValues { get; set; }
}

public enum ResultState
{
    Ok,
    Created,
    BadRequest,
    NotFound
}