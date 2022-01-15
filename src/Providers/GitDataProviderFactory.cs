using System;
using System.Runtime.InteropServices;
using coveralls_uploader.Utilities.Wrappers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace coveralls_uploader.Providers
{
    public class GitDataProviderFactory
    {
        private readonly IHost _host;
        private readonly IRuntimeInformation _runtimeInformation;

        public GitDataProviderFactory(IHost host, IRuntimeInformation runtimeInformation)
        {
            _host = host;
            _runtimeInformation = runtimeInformation;
        }

        public IGitDataProvider Create()
        {
            if (_runtimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return _host.Services.GetRequiredService<LinuxGitDataCommandLineProvider>();
            }

            if (_runtimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return _host.Services.GetRequiredService<OsxGitDataCommandLineProvider>();
            }

            if (_runtimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return _host.Services.GetRequiredService<WindowsGitDataCommandLineProvider>();
            }

            throw new PlatformNotSupportedException();
        }
    }
}