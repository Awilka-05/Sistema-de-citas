using Microsoft.Extensions.Logging;

namespace SistemaGestionCitas.Infrastructure.Repositories.Logger;

public class SingletonLoggerProvider : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName)
    {
        if (categoryName.StartsWith("Microsoft.EntityFrameworkCore") ||
            categoryName.StartsWith("Microsoft."))
        {
            return new NullLogger(); // no escribe nada
        }

        return SingletonLogger.Instance;
    }

    public void Dispose() { }
}

// Esto es para que en nuestro Log solamente se guarden Log de nuestra APP
public class NullLogger : ILogger
{
    public IDisposable BeginScope<TState>(TState state) => null;
    public bool IsEnabled(LogLevel logLevel) => false;
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
        Func<TState, Exception, string> formatter) { }
}