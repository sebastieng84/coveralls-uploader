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
            Description = @"Parse and upload a lcov code coverage report and to Coveralls.io";

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
        }
    }
}