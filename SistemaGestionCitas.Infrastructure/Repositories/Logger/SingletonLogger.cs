using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace SistemaGestionCitas.Infrastructure.Services.Logger;

public class SingletonLogger : ILogger
{
    private static SingletonLogger? _instance;
    private static readonly object LockInstance = new object();
    private static readonly object LockFile = new object();
    private readonly string _filePath;
    private readonly string _connectionString;

    private SingletonLogger(string connectionString)
    {
        Directory.CreateDirectory("Logs");
        _filePath = Path.Combine("Logs", "log.txt");
        _connectionString = connectionString;
    }

    public static SingletonLogger Instance(string connectionString)
    {
        if (_instance == null)
        {
            lock (LockInstance)
            {
                if (_instance == null)
                {
                    _instance = new SingletonLogger(connectionString);
                }
            }
        }

        return _instance;
    }


    public IDisposable? BeginScope<TState>(TState state) => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        string message = formatter(state, exception);
        string logLine = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{logLevel}] {message}";

        //Guardo en archivo
        lock (LockFile)
        {
            File.AppendAllText(_filePath, logLine + Environment.NewLine);
            if (exception != null)
            {
                File.AppendAllText(_filePath, $"Exception: {exception.Message}{Environment.NewLine}");
            }
        }
    }
}
