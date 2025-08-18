using Microsoft.Extensions.Logging;

namespace SistemaGestionCitas.Infrastructure.Repositories.Logger;

public class SingletonLogger : ILogger
{
    // Instancia de la clase
    private static SingletonLogger _instance;
    private readonly string _filePath;
    private static readonly object _lockInstance = new object();
    private static readonly object _lockFile = new object();

    // Constructor privado
    private SingletonLogger()
    {
        Directory.CreateDirectory("Logs");
        _filePath = Path.Combine("Logs", "Application.log");
    }
    
    // Propiedad pública para acceder al singleton
    public static SingletonLogger Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lockInstance)
                {
                    if (_instance == null)
                        _instance = new SingletonLogger();
                }
            }
            return _instance;
        }
    }

    public IDisposable BeginScope<TState>(TState state) => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
        Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel)) return;

        string message = formatter(state, exception);
        string logLine = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{logLevel}] {message}";

        lock (_lockFile)
        {
            System.IO.File.AppendAllText(_filePath, logLine + Environment.NewLine);
            if (exception != null)
                System.IO.File.AppendAllText(_filePath, exception + Environment.NewLine);
        }
    }
}