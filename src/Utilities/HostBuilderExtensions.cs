using System.Diagnostics.CodeAnalysis;
using System.IO.Abstractions;
using System.Net.Http;
using coveralls_uploader.Commands;
using coveralls_uploader.JobProviders;
using coveralls_uploader.Parsers;
using coveralls_uploader.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace coveralls_uploader.Utilities
{
    [ExcludeFromCodeCoverage]
    public static class HostBuilderExtensions
    {
        public static IHostBuilder AddServices(
            this IHostBuilder hostBuilder, 
            LoggingLevelSwitch loggingLevelSwitch)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services
                    .AddSingleton(Log.Logger)
                    .AddSingleton(typeof(LoggingLevelSwitch), loggingLevelSwitch)
                    .AddTransient<IFileSystem, FileSystem>()
                    .AddTransient<IParser, LcovParser>()
                    .AddTransient<IEnvironmentVariablesJobProvider, GitHubJobProvider>()
                    .AddTransient<IEnvironmentWrapper, EnvironmentWrapper>()
                    .AddTransient<MainService>()
                    .AddTransient<CoverallsService>()
                    .AddTransient<SourceFileService>()
                    .AddTransient<HttpClient>()
                    .AddTransient<EnvironmentVariablesJobProviderFactory>()
                    .AddTransient<JenkinsJobProvider>()
                    .AddTransient<GitHubJobProvider>();
            });

            return hostBuilder;
        }
    }
}