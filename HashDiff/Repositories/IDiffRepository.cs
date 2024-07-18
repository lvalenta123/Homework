using HashDiff.Models;

namespace HashDiff.Repositories;

/// <summary>
/// Repository for Diff model
/// </summary>
public interface IDiffRepository
{
    /// <summary>
    /// Saves left part of the diff
    /// </summary>
    /// <param name="id">Id of diff</param>
    /// <param name="leftPart">Left part of the diff</param>
    /// <returns>Created object</returns>
    Task<Diff> SaveLeftPartAsync(int id, string leftPart);
    
    
    /// <summary>
    /// Saves right part of the diff
    /// </summary>
    /// <param name="id">Id of diff</param>
    /// <param name="rightPart">Right part of the diff</param>
    /// <returns>Created object</returns>
    Task<Diff> SaveRightPartAsync(int id, string rightPart);
    
    
    /// <summary>
    /// Returns whole diff with right and left part
    /// </summary>
    /// <param name="id">Id of diff</param>
    /// <returns>Diff</returns>
    Task<Diff?> GetDiffAsync(int id);
}