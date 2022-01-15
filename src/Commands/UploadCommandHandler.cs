using System;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;
using coveralls_uploader.Models;
using coveralls_uploader.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace coveralls_uploader.Commands
{
    public class UploadCommandHandler : ICommandHandler
    {
        private readonly IHost _host;

        public FileInfo Input { get; set; }
        public bool Source { get; set; }
        public bool Verbose { get; set; }
        public string Token { get; set; }

        public UploadCommandHandler(IHost host)
        {
            _host = host;
        }

        public async Task<int> InvokeAsync(InvocationContext context)
        {
            var mainService = _host.Services.GetRequiredService<MainService>();
            var logger = _host.Services.GetRequiredService<ILogger>();

            try
            {
                if (Verbose)
                {
                    _host.Services
                        .GetRequiredService<LoggingLevelSwitch>()
                        .MinimumLevel = LogEventLevel.Verbose;
                }

                await mainService.RunAsync(new CommandOptions(
                    Input,
                    Source,
                    Token));
            }
            catch (Exception exception)
            {
                logger.Error(exception, "An unhandled error has occured!");
                return -1;
            }

            return 0;
        }
    }
}