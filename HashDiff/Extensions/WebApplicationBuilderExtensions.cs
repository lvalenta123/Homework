using HashDiff.Database;
using HashDiff.Repositories;
using HashDiff.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace HashDiff.Extensions;

/// <summary>
/// Extension class to wrap logic around WebApplicationBuilder.
/// I don't want to have long Program class
/// </summary>
public static class WebApplicationBuilderExtensions
{
    /// <summary>
    /// Registers DI
    /// </summary>
    /// <param name="builder"></param>
    public static void RegisterDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IDiffRepository, DiffRepository>();
        builder.Services.AddScoped<IDiffService, DiffService>();
        builder.Services.AddScoped<IDiffComparer, DiffComparer>();
    }
    
    /// <summary>
    /// Sets up DB connection
    /// </summary>
    /// <param name="builder"></param>
    public static void AddDbConnection(this WebApplicationBuilder builder)
    {
        var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
        var dbName = Environment.GetEnvironmentVariable("DB_NAME");
        var dbPasswordFile = Environment.GetEnvironmentVariable("DB_SA_PASSWORD_FILE");
        var dbPassword = "";
        if(!string.IsNullOrEmpty(dbPasswordFile))
            dbPassword = File.ReadAllText(dbPasswordFile);
        else
            dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
        var connectionString = $"Data Source={dbHost};Initial Catalog={dbName};User ID=sa;Password={dbPassword}";
        builder.Services.AddDbContext<DatabaseContext>(opt =>
        {
            opt.UseSqlServer(connectionString);
        });
    }
}