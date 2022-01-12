using System.IO.Abstractions;
using Serilog;

namespace coveralls_uploader.Providers
{
    public class WindowsGitDataCommandLineProvider : GitDataCommandLineProvider
    {
        protected override string ArgumentsPrefix => "/c";
        protected override string CommandLineFileName => "cmd";

        public WindowsGitDataCommandLineProvider(ILogger logger) : base(logger)
        {
        }
    }
}