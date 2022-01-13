using Serilog;

namespace coveralls_uploader.Providers
{
    public class WindowsGitDataCommandLineProvider : GitDataCommandLineProvider
    {
        protected override string ArgumentsPrefix => "/C";
        protected override string CommandLineFileName => "cmd.exe";

        public WindowsGitDataCommandLineProvider(ILogger logger) : base(logger)
        {
        }
    }
}