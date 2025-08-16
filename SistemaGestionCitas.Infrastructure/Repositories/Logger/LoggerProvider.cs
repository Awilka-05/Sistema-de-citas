using Microsoft.Extensions.Logging;
using SistemaGestionCitas.Infrastructure.Services.Logger;
using ILogger = SistemaGestionCitas.Domain.Interfaces.Services.ILogger;

namespace SistemaGestionCitas.Application.Services;

public class LoggerProvider : ILoggerProvider
{
    private readonly string _connectionString;
    
public LoggerProvider(string connectionString)
    {
        _connectionString = connectionString;
    }

    public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
    {
        return SingletonLogger.Instance(_connectionString);   
    }
    public void Dispose() { }
}