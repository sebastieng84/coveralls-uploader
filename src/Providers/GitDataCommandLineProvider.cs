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

            return match.Success ? match.Value.Trim() : null;
        }

        public Head GetHead(string commitSha)
        {
            const string pattern = 
                @"commit ([^\s]*).*\r?\n.*\r?\n?Author: (.*?) <([^>]*)>\r?\nCommit: (.*?) <([^>]*)>\r?\n\r?\n\s*([^\r\n]*)";
            var gitShowCommand = $"git show -q --pretty=full {commitSha}";

            if (!_commandLineHelper.TryRun(gitShowCommand, out var commandOutput))
            {
                return null;
            }

            var match = Regex.Match(commandOutput, pattern);
            if (!match.Success)
            {
                return null;
            }
            
            var head = new Head
            {
                Id = match.Groups.ElementAtOrDefault<Group>(1)?.Value,
                AuthorName = match.Groups.ElementAtOrDefault<Group>(2)?.Value,
                AuthorEmail = match.Groups.ElementAtOrDefault<Group>(3)?.Value,
                CommitterName = match.Groups.ElementAtOrDefault<Group>(4)?.Value,
                CommitterEmail = match.Groups.ElementAtOrDefault<Group>(5)?.Value,
                Message = match.Groups.ElementAtOrDefault<Group>(6)?.Value
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