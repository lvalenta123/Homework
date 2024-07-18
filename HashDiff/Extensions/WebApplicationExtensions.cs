using HashDiff.Database;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace HashDiff.Extensions;

/// <summary>
/// Extension class for WebApplication.
/// Again, so that Program class is not too long.
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Sets up DB schema
    /// </summary>
    /// <param name="app"></param>
    public static async Task EnsureDBCreation(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        await using var context = scope.ServiceProvider.GetService<DatabaseContext>();
        var databaseCreator = context.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
        if(databaseCreator != null)
        {
            await databaseCreator.EnsureCreatedAsync();

        }
    }
}