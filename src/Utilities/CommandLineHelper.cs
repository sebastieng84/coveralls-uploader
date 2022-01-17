using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Serilog;

namespace coveralls_uploader.Utilities
{
    [ExcludeFromCodeCoverage]
    public class CommandLineHelper
    {
        private readonly ILogger _logger;

        protected CommandLineHelper()
        {
        }
        
        public CommandLineHelper(ILogger logger)
        {
            _logger = logger;
        }

        public virtual bool TryRun(string command, out string output)
        {
            output = string.Empty;
            try
            {
                var process = new Process
                {
                    StartInfo = GetProcessStartInfo(command)
                };

                var standardOutput = new StringBuilder();
                process.OutputDataReceived += (_, args) => standardOutput.AppendLine(args.Data);

                _logger.Debug(
                    "Running command line: {FileName} {Arguments}",
                    process.StartInfo.FileName,
                    process.StartInfo.Arguments);
                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    _logger.Warning(
                        "Command line terminated with exit code {ExitCode}",
                        process.ExitCode);

                    return false;
                }

                output = standardOutput.ToString();
                _logger.Debug(
                    "Command line output: {CommandOutput}",
                    output);
                return true;
            }
            catch (Exception exception)
            {
                _logger.Error(
                    exception,
                    "An error occured");
                
                return false;
            }
        }

        private static (string prefix, string shell) GetCommandLineOptionsForPlatform()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var shell = Path.Join(
                    Path.DirectorySeparatorChar.ToString(),
                    "bin",
                    "bash");

                return ("-c", shell);
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var shell = Path.Join(
                    Path.DirectorySeparatorChar.ToString(),
                    "bin",
                    "zsh");

                return ("-c", shell);
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return ("-c", "cmd.exe");
            }

            throw new PlatformNotSupportedException();
        }
        
        private static ProcessStartInfo GetProcessStartInfo(string command)
        {
            var (prefix, shell) = GetCommandLineOptionsForPlatform();
            var arguments = $"{prefix} \"{command}\"";
            
            return new ProcessStartInfo(shell, arguments)
            {
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                StandardOutputEncoding = Encoding.UTF8,
                WorkingDirectory = Directory.GetCurrentDirectory()
            };
        }
    }
}