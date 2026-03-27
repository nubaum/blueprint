using Blueprint.Abstractions.Application.Workspace;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.SystemConsole.Themes;

namespace Blueprint;

public static class IHostBuilderExtensions
{
    public static IHostBuilder ConfigureLogging(this IHostBuilder builder) =>
        builder
        .UseSerilog((_, services, x) =>
            {
                x.Enrich.WithExceptionDetails();
                x.WriteTo.Console(theme: AnsiConsoleTheme.Code);
                x.WriteTo.File(path: services.GetRequiredService<IPathProvider>().GetLoggFileName(), rollingInterval: RollingInterval.Day);
            });
}
