using HashDiff.Database;
using HashDiff.Models;

namespace HashDiff.Repositories;

/// <inheritdoc cref="IDiffRepository"/>
public class DiffRepository : IDiffRepository
{
    private readonly DatabaseContext context;

    public DiffRepository(DatabaseContext context)
    {
        this.context = context;
    }

    public async Task<Diff> SaveLeftPartAsync(int id, string leftPart)
    {
        var diff = context.Diffs.FirstOrDefault(e => e.Id == id);
        if (diff?.LeftPart != null)
            throw new InvalidDataException($"Left part with Id {id} already exists");

        if (diff == null)
        {
            diff = new Diff() { Id = id, LeftPart = leftPart };
            context.Diffs.Add(diff);
        }
        else
        {
            diff.LeftPart = leftPart;
            context.Diffs.Update(diff);
        }

        await context.SaveChangesAsync();

        return diff;
    }

    public async Task<Diff> SaveRightPartAsync(int id, string rightPart)
    {
        var diff = context.Diffs.FirstOrDefault(e => e.Id == id);
        if (diff?.RightPart != null)
            throw new InvalidDataException($"Right part with Id {id} already exists");

        if (diff == null)
        {
            diff = new Diff() { Id = id, RightPart = rightPart };
            context.Diffs.Add(diff);
        }
        else
        {
            diff.RightPart = rightPart;
            context.Diffs.Update(diff);
        }

        await context.SaveChangesAsync();

        return diff;
    }

    public async Task<Diff?> GetDiffAsync(int id)
    {
        return await context.Diffs.FindAsync(id);
    }
}