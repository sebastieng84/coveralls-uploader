using System;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.IO;
using System.Threading.Tasks;
using coveralls_uploader.Models;
using coveralls_uploader.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using Serilog.Events;

namespace coveralls_uploader.Commands
{
    public sealed class UploadCommand : RootCommand
    {
        public UploadCommand()
        {
            // TODO: Add description
            AddOption(
                new Option<FileInfo>(
                    new[] {"--input", "-i"},
                    "File path to the code coverage report.")
                {
                    Arity = ArgumentArity.ExactlyOne,
                    IsRequired = true
                }.ExistingOnly());
            AddOption(new Option<string>(
                new[] {"--token", "-t"},
                "The repository token.")
            {
                Arity = ArgumentArity.ZeroOrOne,
            });
            AddOption(new Option<bool>(
                new[] {"--source", "-s"},
                "Include source file content in the request."));

            AddOption(new Option<bool>(
                new[] {"--verbose", "-v"},
                "Show verbose output."));

            Handler = CommandHandler.Create<CommandOptions, IHost>(Run);
        }

        private static async Task<int> Run(CommandOptions commandOptions, IHost host)
        {
            var mainService = host.Services.GetRequiredService<MainService>();
            var logger = host.Services.GetRequiredService<ILogger>();
            
            if (commandOptions.Verbose)
            {
                host.Services
                    .GetRequiredService<LoggingLevelSwitch>()
                    .MinimumLevel = LogEventLevel.Verbose;
            }
            logger.LogDebug("lol");
            try
            {
                await mainService.RunAsync(commandOptions);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "An unhandled error has occured!");
                return -1;
            }

            return 0;
        }
    }
}