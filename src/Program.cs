using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using System.IO.Abstractions;
using System.Net.Http;
using coveralls_uploader.Commands;
using coveralls_uploader.JobProviders;
using coveralls_uploader.Parsers;
using coveralls_uploader.Services;
using coveralls_uploader.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using ILogger = Microsoft.Extensions.Logging.ILogger;

var loggingLevelSwitch = new LoggingLevelSwitch
{
    MinimumLevel = LogEventLevel.Information
};

return await new CommandLineBuilder(new UploadCommand())
    .UseHost(_ => Host.CreateDefaultBuilder(),
        host =>
        {
            host.ConfigureServices(services =>
            {
                services
                    .AddSingleton(typeof(LoggingLevelSwitch), loggingLevelSwitch)
                    .AddSingleton<MainService>()
                    .AddSingleton<IFileSystem, FileSystem>()
                    .AddSingleton<IParser, LcovParser>()
                    .AddSingleton<IEnvironmentVariablesJobProvider, GitHubJobProvider>()
                    .AddSingleton<CoverallsService>()
                    .AddSingleton<SourceFileService>()
                    .AddSingleton<IEnvironmentWrapper, EnvironmentWrapper>()
                    .AddTransient<ILogger, Logger<UploadCommand>>()
                    .AddTransient<HttpClient>()
                    .AddTransient<EnvironmentVariablesJobProviderFactory>()
                    .AddTransient<JenkinsJobProvider>()
                    .AddTransient<GitHubJobProvider>();
            });
            host.UseSerilog();
        })
    .ConfigureSerilog(loggingLevelSwitch)
    .UseDefaults()
    .Build()
    .InvokeAsync(args);