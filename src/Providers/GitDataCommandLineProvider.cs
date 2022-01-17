using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using coveralls_uploader.Models.Coveralls;
using coveralls_uploader.Utilities;
using Serilog;

namespace coveralls_uploader.Providers
{
    public class GitDataCommandLineProvider : IGitDataProvider
    {
        private readonly ILogger _logger;
        private readonly CommandLineHelper _commandLineHelper;

        public GitDataCommandLineProvider(ILogger logger, CommandLineHelper commandLineHelper)
        {
            _logger = logger;
            _commandLineHelper = commandLineHelper;
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

        public string GetBranch()
        {
            const string pattern = @"(?m)^(.+)$";
            const string gitBranchCommand = "git branch --show-current";

            _commandLineHelper.TryRun(gitBranchCommand, out var commandOutput);
            var match = Regex.Match(commandOutput, pattern);

            return match.Success ? match.Value.Trim().TrimEndNewLine() : null;
        }

        public Head GetHead(string commitSha)
        {
            const string separator = "-|::|-";
            var gitShowCommand =
                $"git show -q --pretty=\"\"\"%H{separator}%an{separator}%ae{separator}%cn" +
                $"{separator}%ce{separator}%s\"\"\" {commitSha}";

            if (!_commandLineHelper.TryRun(gitShowCommand, out var commandOutput))
            {
                return null; lol
            }

            var values = commandOutput.Split(separator);
            var head = new Head
            {
                Id = commitSha ?? values.ElementAtOrDefault(0),
                AuthorName = values.ElementAtOrDefault(1),
                AuthorEmail = values.ElementAtOrDefault(2),
                CommitterName = values.ElementAtOrDefault(3),
                CommitterEmail = values.ElementAtOrDefault(4),
                Message = values.ElementAtOrDefault(5).TrimEndNewLine()
            };

            return head;
        }

        public IList<Remote> GetRemotes()
        {
            const string gitRemoteCommand = "git remote -v";
            const string remotePattern = @"(?m)^([^\s]+)\s+([^\s]+)\s+\(push\)";

            if (!_commandLineHelper.TryRun(gitRemoteCommand, out var commandOutput))
            {
                return new List<Remote>();;
            }

            var matches = Regex.Matches(commandOutput, remotePattern);

            return matches
                .Select(x => new Remote(x.Groups[1].Value, x.Groups[2].Value))
                .ToList();
        }
    }
}