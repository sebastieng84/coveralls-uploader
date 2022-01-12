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
        private readonly GitDataProviderFactory _gitDataProviderFactory;

        public MainService()
        {
        }
        
        public MainService(
            SourceFileService sourceFileService,
            CoverallsService coverallsService,
            IParser parser,
            ILogger logger,
            EnvironmentVariablesJobProviderFactory environmentVariablesJobProviderFactory,
            GitDataProviderFactory gitDataProviderFactory)
        {
            _sourceFileService = sourceFileService;
            _coverallsService = coverallsService;
            _parser = parser;
            _logger = logger;
            _environmentVariablesJobProviderFactory = environmentVariablesJobProviderFactory;
            _gitDataProviderFactory = gitDataProviderFactory;
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
            
            var gitDataProvider = _gitDataProviderFactory.Create();
            job.Git.Merge(gitDataProvider.Load(job.CommitSha));
            
            job.SourceFiles = sourceFiles;
            job.RepositoryToken = commandOptions.Token ?? job.RepositoryToken;
        
            await _coverallsService.UploadAsync(job);
        }
    }
}