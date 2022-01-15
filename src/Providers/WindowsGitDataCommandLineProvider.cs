using coveralls_uploader.Utilities;
using Serilog;

namespace coveralls_uploader.Providers
{
    public class WindowsGitDataCommandLineProvider : GitDataCommandLineProvider
    {
        protected override string ArgumentsPrefix => "/C";
        protected override string CommandLineFileName => "cmd.exe";

        public WindowsGitDataCommandLineProvider(
            ILogger logger,
            ProcessFactory processFactory) : base(logger, processFactory)
        {
        }
    }
}