using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using System.IO.Abstractions;
using coveralls_uploader;
using coveralls_uploader.JobProviders;
using coveralls_uploader.Parsers;
using coveralls_uploader.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

return await new CommandLineBuilder(new UploadCommand())
    .UseHost(_ => Host.CreateDefaultBuilder(),
        host =>
        {
            host.ConfigureServices(services =>
            {
                services
                    .AddSingleton<MainService>()
                    .AddSingleton<IFileSystem, FileSystem>()
                    .AddSingleton<IParser, LcovParser>()
                    .AddSingleton<IEnvironmentVariablesJobProvider, GitHubActionsJobProvider>()
                    .AddSingleton<CoverallsService>()
                    .AddSingleton<SourceFileService>()
                    .AddTransient<ILogger, Logger<UploadCommand>>();
            });
            host.ConfigureLogging((_, loggingBuilder) =>
            {
                loggingBuilder
                    .ClearProviders()
                    .AddConsole()
                    .AddFilter("Microsoft", LogLevel.None)
                    .SetMinimumLevel(LogLevel.Trace);
            });
        })
    .UseDefaults()
    .Build()
    .InvokeAsync(args);
    
    