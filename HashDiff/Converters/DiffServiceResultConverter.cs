using HashDiff.Models;
using Microsoft.AspNetCore.Mvc;

namespace HashDiff.Converters;

/// <summary>
/// Maps results returned from business logic to HTTP resutls
/// </summary>
public static class DiffServiceResultConverter
{
    /// <summary>
    /// Converts <see cref="DiffServiceResult" to inheritors of <see cref="IActionResult"/> />
    /// </summary>
    /// <param name="diffServiceResult">Result returned from business logic</param>
    /// <returns>IActionResult that can be returned in the controller</returns>
    /// <exception cref="Exception">Thrown if new <see cref="ResultState"/> is not implemented</exception>
    public static IActionResult ToActionResult(DiffServiceResult diffServiceResult)
    {
        if (diffServiceResult.ResultState == ResultState.Ok)
            return (diffServiceResult.ObjectToReturn == null)
                ? new OkResult()
                : new OkObjectResult(diffServiceResult.ObjectToReturn);
        
        if (diffServiceResult.ResultState == ResultState.Created)
            return new CreatedAtActionResult(
                diffServiceResult.ActionName, 
                diffServiceResult.ControllerName, 
                diffServiceResult.RouterValues, 
                diffServiceResult.ObjectToReturn);
        
        if (diffServiceResult.ResultState == ResultState.BadRequest)
            return string.IsNullOrEmpty(diffServiceResult.Message)
                ? new BadRequestResult()
                : new BadRequestObjectResult(diffServiceResult.Message);
        
        if (diffServiceResult.ResultState == ResultState.NotFound)
            return string.IsNullOrEmpty(diffServiceResult.Message)
                ? new NotFoundResult()
                : new NotFoundObjectResult(diffServiceResult.Message);

        throw new Exception($"Result state {diffServiceResult.ResultState} is not implemented");
    }
}