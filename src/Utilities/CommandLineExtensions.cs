using System.CommandLine.Builder;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace coveralls_uploader.Utilities
{
    public static class CommandLineExtensions
    {
        public static CommandLineBuilder ConfigureSerilog(
            this CommandLineBuilder builder,
            LoggingLevelSwitch loggingLevelSwitch)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(loggingLevelSwitch)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            return builder;
        }
    }
}