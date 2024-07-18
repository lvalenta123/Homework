using HashDiff.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace HashDiff.Database;

public sealed class DatabaseContext : DbContext
{
    
    public DatabaseContext(DbContextOptions<DatabaseContext> dbContextOptions) : base(dbContextOptions)
    {
    }

    public DbSet<Diff> Diffs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Override auto-generating identity key to enable manual insertion of PK
        modelBuilder.Entity<Diff>()
            .Property(k => k.Id)
            .ValueGeneratedNever();
    }
}