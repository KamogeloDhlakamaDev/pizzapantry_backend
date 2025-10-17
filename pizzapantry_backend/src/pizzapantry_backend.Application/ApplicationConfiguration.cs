using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace pizzapantry_backend.Application;

public static class ApplicationConfiguration
{
    public static void AddApplication(this IServiceCollection services)
    {
        var serilogFile = new ConfigurationBuilder()
            .AddJsonFile("serilog.setup.json")
            .Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(serilogFile)
            .CreateLogger();

        services.AddSingleton(Log.Logger);
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(dispose: true);
        });
    }
}