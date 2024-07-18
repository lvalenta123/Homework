namespace HashDiff.Services;

/// <summary>
/// Compares diff parts and determines if and how they differ
/// </summary>
public interface IDiffComparer
{
    /// <summary>
    /// Checks if left and right part of diff are of same lenght and whether they are equal
    /// </summary>
    /// <param name="leftPart">Left part of diff</param>
    /// <param name="rightPart">Right part of diff</param>
    /// <returns>Result of comparison as either string of as an array of indexes with differing characters</returns>
    DiffComparerResult CompareParts(string leftPart, string rightPart);
}