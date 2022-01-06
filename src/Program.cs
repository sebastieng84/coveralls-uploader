using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using System.IO.Abstractions;
using System.Net.Http;
using coveralls_uploader;
using coveralls_uploader.JobProviders;
using coveralls_uploader.Parsers;
using coveralls_uploader.Services;
using coveralls_uploader.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RestSharp;

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
    
    