using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using coveralls_uploader.Models.Coveralls;
using Serilog;

namespace coveralls_uploader.Providers
{
    public abstract class GitDataCommandLineProvider : IGitDataProvider
    {
        private readonly ILogger _logger;
        
        protected abstract string ArgumentsPrefix { get; }
        protected abstract string CommandLineFileName { get; }
        
        protected GitDataCommandLineProvider(ILogger logger)
        {
            _logger = logger;
        }

        public Git Load(string commitSha)
        {
            return new Git
            {
                Head = GetHead(commitSha),
                Branch = GetBranch(),
                Remotes = GetRemotes()
            }; 
        }

        private string GetBranch()
        {
            const string branchPattern = @"(?m)^\* (\w+)";
            const string gitBranchCommand = "git branch";

            if (!TryRunCommandLine(gitBranchCommand, out var commandOutput))
            {
                return null;
            }
            
            var match = Regex.Match(commandOutput, branchPattern);
            
            return match.Success ? match.Groups[1].Value : null;
        }

        private Head GetHead(string commitSha)
        {
            const char separator = '|';
            var gitShowCommand =
                $"git show -q --pretty=\"\"%an{separator}%ae{separator}%cn{separator}%ce{separator}%s\"\" {commitSha}";

            _logger.Debug("Running the following command: {Command}", gitShowCommand);
            if (!TryRunCommandLine(gitShowCommand, out var commandOutput))
            {
                return null;
            }
            
            _logger.Debug("Command output: {CommandOutput}", commandOutput);

            var values = commandOutput.Split('|');

            var head = new Head
            {
                Id = commitSha,
                AuthorName = values.ElementAtOrDefault(0),
                AuthorEmail = values.ElementAtOrDefault(1),
                CommitterName = values.ElementAtOrDefault(2),
                CommitterEmail = values.ElementAtOrDefault(3),
                Message = values.ElementAtOrDefault(4),
            };
            _logger.Debug("Head: @{Head}", head);
            return head;
        }

        private IList<Remote> GetRemotes()
        {
            IList<Remote> remotes = new List<Remote>();
            
            const string gitRemoteCommand = "git remote -v";
            const string remotePattern = @"(?m)^(\w+)\s+(.*)\s+\(push\)";

            if (!TryRunCommandLine(gitRemoteCommand, out var commandOutput))
            {
                return remotes;
            }

            var matches = Regex.Matches(commandOutput, remotePattern);

            return matches
                .Select(x => new Remote(x.Groups[1].Value, x.Groups[2].Value))
                .ToList();
        }

        private bool TryRunCommandLine(string arguments, out string output)
        {
            var process = new Process();
            process.StartInfo = new(CommandLineFileName, $"{ArgumentsPrefix} \"{arguments}\"")
            {
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WorkingDirectory = Directory.GetCurrentDirectory()
            };
            
            var standardOutput = new StringBuilder();
            process.OutputDataReceived += (_, args) => standardOutput.AppendLine(args.Data);
                
            try
            {
                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit();
            }
            catch (Exception)
            {
                output = string.Empty;
                return false;
            }

            output = standardOutput.ToString();
            return true;
        }
    }
}