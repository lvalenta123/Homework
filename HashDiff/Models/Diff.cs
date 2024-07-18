using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HashDiff.Models;

/// <summary>
/// Representation of a diff components
/// </summary>
public class Diff
{
    /// <summary>
    /// ID of the diff
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    
    /// <summary>
    /// Left part of the diff
    /// </summary>
    public string? LeftPart { get; set; }
    
    /// <summary>
    /// Right part of the diff
    /// </summary>
    public string? RightPart { get; set; }
}