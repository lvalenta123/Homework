using HashDiff.Repositories;
using HashDiff.Services;
using HashDiff.Tests.TestDoubles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HashDiff.Tests;

/// <summary>
/// Registers dependencies and substitutes test doubles where needed
/// </summary>
public class DIContainer
{
    private readonly ServiceProvider serviceProvider;

    public DIContainer()
    {
        var services = new ServiceCollection();
        
        services.AddScoped<IDiffService, DiffService>();
        services.AddSingleton<IDiffRepository, DiffTestRepository>();
        services.AddScoped(typeof(ILogger<>), typeof(NullLogger<>));
        services.AddScoped<IDiffComparer, DiffComparer>();
       
        serviceProvider = services.BuildServiceProvider(); 
    }

    public T GetService<T>()
    {
        var service = serviceProvider.GetService<T>();
        if (service == null)
            throw new Exception($"Type {typeof(T)} could not be resolved");
        return service;
    }
}