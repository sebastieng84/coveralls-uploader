using System.IO;
using coveralls_uploader.Utilities;
using Serilog;

namespace coveralls_uploader.Providers
{
    public class LinuxGitDataCommandLineProvider : GitDataCommandLineProvider
    {
        protected override string ArgumentsPrefix => "-c";

        protected override string CommandLineFileName => Path.Join(
            Path.DirectorySeparatorChar.ToString(),
            "bin",
            "bash");

        public LinuxGitDataCommandLineProvider(
            ILogger logger,
            ProcessFactory processFactory) : base(logger, processFactory)
        {
        }
    }
}