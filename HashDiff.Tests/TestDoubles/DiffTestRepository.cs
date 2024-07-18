using HashDiff.Models;
using HashDiff.Repositories;

namespace HashDiff.Tests.TestDoubles;

/// <summary>
/// Test double for unit tests.
/// I didn't want to slow down tests by running Docker so I created this double. It's logic is very simple -> save an
/// object and if it exists throw an exception.
/// </summary>
public class DiffTestRepository : IDiffRepository
{
    private Dictionary<int, Diff> Rows = new();
        
    public Task<Diff> SaveLeftPartAsync(int id, string leftPart)
    {
        Rows.TryGetValue(id, out var diff);
        if (diff?.LeftPart != null)
            throw new InvalidDataException();
        diff ??= new Diff { Id = id };
        diff.LeftPart = leftPart;
        
        Rows[id] = diff;
        return Task.FromResult(diff);
    }

    public Task<Diff> SaveRightPartAsync(int id, string rightPart)
    {
        Rows.TryGetValue(id, out var diff);
        if (diff?.RightPart != null)
            throw new InvalidDataException();
        diff ??= new Diff { Id = id };
        diff.RightPart = rightPart;

        Rows[id] = diff;
        return Task.FromResult(diff);
    }

    public Task<Diff?> GetDiffAsync(int id)
    {
        Rows.TryGetValue(id, out var diff);
        return Task.FromResult(diff);
    }

    public void Clear()
    {
        Rows = new();
    }
}