using System.IO;
using System.IO.Abstractions;
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

        public OsxGitDataCommandLineProvider(ILogger logger) : base(logger)
        {
        }
    }
}