using Microsoft.Extensions.Logging;

namespace SistemaGestionCitas.Domain.Interfaces.Services;

public interface ILogger
{
    //Define los métodos que debe implementar el logger
    public IDisposable BeginScope<TState>(TState state);
    public bool IsEnabled(LogLevel logLevel);

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        string message = formatter(state, exception);
        Console.WriteLine($"[{"{logLevel}"}] {message}");
    }
}