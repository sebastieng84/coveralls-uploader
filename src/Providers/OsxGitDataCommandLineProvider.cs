using System.IO;
using System.IO.Abstractions;
using coveralls_uploader.Utilities;
using Serilog;

namespace coveralls_uploader.Providers
{
    public class OsxGitDataCommandLineProvider : GitDataCommandLineProvider
    {
        protected override string ArgumentsPrefix => "-c";

        protected override string CommandLineFileName => Path.Join(
            Path.DirectorySeparatorChar.ToString(),
            "bin",
            "zsh");

        public OsxGitDataCommandLineProvider(
            ILogger logger,
            ProcessFactory processFactory) : base(logger, processFactory)
        {
        }
    }
}