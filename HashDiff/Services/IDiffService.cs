using HashDiff.Models;

namespace HashDiff.Services;

/// <summary>
/// Handles controller inputs and decides returing objects
/// </summary>
public interface IDiffService
{
    /// <summary>
    /// Manages POST /v1/diff/{id}/left
    /// </summary>
    /// <param name="id">Id of diff</param>
    /// <param name="data">Base64 encoded string to be processed</param>
    /// <returns>201 if data are saved successfully or 400 if there is an error in the data</returns>
    Task<DiffServiceResult> SaveLeftPartAsync(int id, string data);
    
    /// <summary>
    /// Manages GET /v1/diff/{id}/left
    /// </summary>
    /// <param name="id">Id of created object</param>
    /// <returns>200 Created object from POST request or 404 if object does not exist</returns>
    Task<DiffServiceResult> GetLeftPartAsync(int id);
    
    /// <summary>
    /// Manages POST /v1/diff/{id}/right
    /// </summary>
    /// <param name="id">Id of diff</param>
    /// <param name="data">Base64 encoded string to be processed</param>
    /// <returns>201 if data are saved successfully or 400 if there is an error in the data</returns>
    Task<DiffServiceResult> SaveRightPartAsync(int id, string data);
    
    /// <summary>
    /// Manages GET /v1/diff/{id}/right
    /// </summary>
    /// <param name="id">Id of created object</param>
    /// <returns>200 Created object from POST request or 404 if object does not exist</returns>
    Task<DiffServiceResult> GetRightPartAsync(int id);
    
    /// <summary>
    /// Manages GET /v1/diff/{id}
    /// </summary>
    /// <param name="id">Id of compared diff</param>
    /// <returns>200 if comparison is successfull or 404 if data are not ready</returns>
    Task<DiffServiceResult> GetDiff(int id);
}