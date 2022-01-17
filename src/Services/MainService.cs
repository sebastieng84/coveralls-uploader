using System.Threading.Tasks;
using coveralls_uploader.Models;
using coveralls_uploader.Parsers;
using coveralls_uploader.Providers;
using Serilog;

namespace coveralls_uploader.Services
{
    public class MainService
    {
        private readonly SourceFileService _sourceFileService;
        private readonly CoverallsService _coverallsService;
        private readonly IParser _parser;
        private readonly ILogger _logger;
        private readonly EnvironmentVariablesJobProviderFactory _environmentVariablesJobProviderFactory;
        private readonly GitDataCommandLineProvider _gitDataCommandLineProvider;

        public MainService()
        {
        }

        public MainService(
            SourceFileService sourceFileService,
            CoverallsService coverallsService,
            IParser parser,
            ILogger logger,
            EnvironmentVariablesJobProviderFactory environmentVariablesJobProviderFactory,
            GitDataCommandLineProvider gitDataCommandLineProvider)
        {
            _sourceFileService = sourceFileService;
            _coverallsService = coverallsService;
            _parser = parser;
            _logger = logger;
            _environmentVariablesJobProviderFactory = environmentVariablesJobProviderFactory;
            _gitDataCommandLineProvider = gitDataCommandLineProvider;
        }

        public virtual async Task RunAsync(CommandOptions commandOptions)
        {
            _logger.Information(
                "Parsing the input file {Input}",
                commandOptions.Input);

            var fileCoverages = _parser.Parse(commandOptions.Input);

            _logger.Information("Converting FileCoverage to SourceFile...");
            var sourceFiles = await _sourceFileService.CreateManyAsync(fileCoverages, commandOptions);

            _logger.Information("Fetching job data...");

            var environmentVariablesJobProvider = _environmentVariablesJobProviderFactory.Create();
            var job = environmentVariablesJobProvider.Load();
            
            job.Git.Merge(_gitDataCommandLineProvider.Load(job.CommitSha));

            job.SourceFiles = sourceFiles;
            job.RepositoryToken = commandOptions.Token ?? job.RepositoryToken;

            await _coverallsService.UploadAsync(job);
        }
    }
}