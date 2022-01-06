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

namespace coveralls_uploader
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
                "Include source file content in the request.")
            {
                Arity = ArgumentArity.ZeroOrOne
            });

            Handler = CommandHandler.Create<CommandOptions, IHost>(Run);
        }

        private static async Task<int> Run(CommandOptions commandOptions, IHost host)
        {
            var mainService = host.Services.GetRequiredService<MainService>();
            var logger = host.Services.GetRequiredService<ILogger>();
            
            try
            {
                await mainService.RunAsync(commandOptions);
            }
            catch (Exception exception)
            {
                logger.LogError("An error has occured: {message}", exception.Message);
                return -1;
            }

            return 0;
        }
    }
}