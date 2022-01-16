using System.Diagnostics.CodeAnalysis;
using System.IO.Abstractions;
using System.Net.Http;
using coveralls_uploader.Parsers;
using coveralls_uploader.Providers;
using coveralls_uploader.Services;
using coveralls_uploader.Utilities.Wrappers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;

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
                    .AddSingleton<CommandLineHelper>()
                    .AddTransient<IFileSystem, FileSystem>()
                    .AddTransient<IParser, LcovParser>()
                    .AddTransient<IEnvironmentVariablesJobProvider, GitHubJobProvider>()
                    .AddTransient<IEnvironment, EnvironmentWrapper>()
                    .AddTransient<IRuntimeInformation, RuntimeInformationWrapper>()
                    .AddTransient<MainService>()
                    .AddTransient<CoverallsService>()
                    .AddTransient<SourceFileService>()
                    .AddTransient<HttpClient>()
                    .AddTransient<EnvironmentVariablesJobProviderFactory>()
                    .AddTransient<JenkinsJobProvider>()
                    .AddTransient<GitHubJobProvider>()
                    .AddTransient<GitDataCommandLineProvider>();
            });

            return hostBuilder;
        }
    }
}