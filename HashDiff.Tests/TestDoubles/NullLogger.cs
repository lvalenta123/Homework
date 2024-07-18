using Microsoft.Extensions.Logging;

namespace HashDiff.Tests.TestDoubles;

/// <summary>
/// Test double for ILogger<T>
/// </summary>
/// <typeparam name="T"></typeparam>
public class NullLogger<T> : ILogger<T>
{
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) { }

    public bool IsEnabled(LogLevel logLevel) => false;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
}