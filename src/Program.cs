using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using coveralls_uploader.Commands;
using coveralls_uploader.Utilities;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;
using Serilog.Events;

var loggingLevelSwitch = new LoggingLevelSwitch
{
    MinimumLevel = LogEventLevel.Information
};

return await new CommandLineBuilder(new UploadCommand())
    .UseHost(_ => Host.CreateDefaultBuilder(),
        host =>
        {
            host.AddServices(loggingLevelSwitch)
                .UseSerilog();
            host.UseCommandHandler<UploadCommand, UploadCommandHandler>();
        })
    .ConfigureSerilog(loggingLevelSwitch)
    .UseDefaults()
    .Build()
    .InvokeAsync(args);