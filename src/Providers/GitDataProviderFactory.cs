using System;
using System.IO.Abstractions;
using System.Runtime.InteropServices;
using Serilog;

namespace coveralls_uploader.Providers
{
    public class GitDataProviderFactory
    {
        private readonly ILogger _logger;

        public GitDataProviderFactory(ILogger logger)
        {
            _logger = logger;
        }
        
        public IGitDataProvider Create()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return new LinuxGitDataCommandLineProvider(_logger);
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return new OsxGitDataCommandLineProvider(_logger);
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return new WindowsGitDataCommandLineProvider(_logger);
            }

            throw new ArgumentOutOfRangeException(nameof(OSPlatform));
        }
    }
}