using HashDiff.Converters;
using HashDiff.Models;
using HashDiff.Repositories;
using HashDiff.Services;
using Microsoft.AspNetCore.Mvc;

namespace HashDiff.Controllers;

/// <summary>
/// Main controller, manages the diffs and it's parts
/// </summary>
[ApiController]
[Route("v1/diff")]
public class DiffContollerV1: ControllerBase
{
    private readonly IDiffService diffService;

    public DiffContollerV1(IDiffService diffService)
    {
        this.diffService = diffService;
    }

    /// <summary>
    /// Creates left part of the diff
    /// </summary>
    /// <param name="id">Id of diff</param>
    /// <param name="body">Base64 encoded JSON with left part</param>
    /// <returns>201 if object is saved, 400 if there is an error in the data</returns>
    [HttpPost, Route("{id}/left")]
    [Consumes("application/custom")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> PostLeftAsync(int id, [FromBody] string body)
    {
        var result = await diffService.SaveLeftPartAsync(id, body);
        if (result.ResultState == ResultState.Created)
        {
            result.ActionName = nameof(GetLeft);
            result.ControllerName = nameof(DiffContollerV1);
            result.RouterValues = new { id };
        }
        return DiffServiceResultConverter.ToActionResult(result);
    }
    
    /// <summary>
    /// Retrieves left part of the diff.
    /// Endpoint provided to comply with recommended rules for endpoints that return 201.
    /// </summary>
    /// <param name="id">Id of created left part</param>
    /// <returns>Diff with left part</returns>
    [HttpGet, Route("{id}/left")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetLeft(int id)
    {
        return DiffServiceResultConverter.ToActionResult(await diffService.GetLeftPartAsync(id));
    }
    
    /// <summary>
    /// Creates right part of diff
    /// </summary>
    /// <param name="id">Id of the diff</param>
    /// <param name="body">Base64 encoded JSON with right part</param>
    /// <returns>201 if object is saved, 400 if there is an error in the data</returns>
    [HttpPost, Route("{id}/right")]
    [Consumes("application/custom")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> PostRightAsync(int id, [FromBody] string body)
    {
        var result = await diffService.SaveRightPartAsync(id, body);
        if (result.ResultState == ResultState.Created)
        {
            result.ActionName = nameof(GetRight);
            result.ControllerName = nameof(DiffContollerV1);
            result.RouterValues = new { id };
        }
        return DiffServiceResultConverter.ToActionResult(result);
    }
    
    /// <summary>
    /// Retrieves right part of the diff.
    /// Endpoint provided to comply with recommended rules for endpoints that return 201.
    /// </summary>
    /// <param name="id">Id of created right part</param>
    /// <returns>Diff with right part</returns>
    [HttpGet, Route("{id}/right")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetRight(int id)
    {
        return DiffServiceResultConverter.ToActionResult(await diffService.GetRightPartAsync(id));
    }
    
    /// <summary>
    /// Get diff result between left and right part.
    /// Result can be a message or an array containing differing indexes as well as differing characters
    /// </summary>
    /// <param name="id">Id of diff</param>
    /// <returns>200 with the diff result</returns>
    [HttpGet, Route("{id}")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetDiffAsync(int id)
    {
        return DiffServiceResultConverter.ToActionResult(await diffService.GetDiff(id));
    }
}