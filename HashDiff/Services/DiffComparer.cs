namespace HashDiff.Services;


/// <inheritdoc cref="IDiffComparer"/>
public class DiffComparer : IDiffComparer
{
    public DiffComparerResult CompareParts(string leftPart, string rightPart)
    {
        if (leftPart.Length == rightPart.Length) 
        {
            if (string.Equals(leftPart, rightPart, StringComparison.Ordinal))
                return new DiffComparerResult { Result = "Inputs were equal" };
            
            return new DiffComparerResult { Diffs = GetDiffs(leftPart, rightPart) };
        } 
            
        return new DiffComparerResult { Result = "Inputs are of different size" };
    }

    private DiffIndexes[] GetDiffs(string leftPart, string rightPart)
    {
        var leftChars = leftPart.ToCharArray();
        var rightChars = rightPart.ToCharArray();
	
        if(leftChars.Length != rightChars.Length)
            throw new InvalidDataException($"{nameof(leftPart)} and {nameof(rightPart)} are not of the same length");

        // I like LINQ and I believe this one is readable
        return Enumerable.Range(0, leftChars.Length)
            .Select(e => new
                { index = e, left = leftChars[e], right = rightChars[e], areTheSame = leftChars[e] == rightChars[e] })
            .Where(e => !e.areTheSame)
            .Select(e => new DiffIndexes
            {
                Index = e.index,
                LeftChar = e.left,
                RightChar = e.right
            })
            .ToArray();
    }
}

/// <summary>
/// Represent diff comparison result as a string or as an array of indexes with differing characters
/// </summary>
public class DiffComparerResult
{
    public string? Result { get; set; }
    public DiffIndexes[]? Diffs { get; set; }
}

/// <summary>
/// Represents an index with differing characters on the left and on the right
/// </summary>
public class DiffIndexes
{
    public int Index { get; set; }
    public char LeftChar { get; set; }
    public char RightChar { get; set; }
}