using System.Threading.Tasks;
using coveralls_uploader.JobProviders;
using coveralls_uploader.Models;
using coveralls_uploader.Parsers;
using Microsoft.Extensions.Logging;

namespace coveralls_uploader.Services
{
    public class MainService 
    {
        private readonly SourceFileService _sourceFileService;
        private readonly CoverallsService _coverallsService;
        private readonly IParser _parser;
        private readonly ILogger _logger;
        private readonly EnvironmentVariablesJobProviderFactory _environmentVariablesJobProviderFactory;

        public MainService(
            SourceFileService sourceFileService,
            CoverallsService coverallsService,
            IParser parser,
            ILogger logger,
            EnvironmentVariablesJobProviderFactory environmentVariablesJobProviderFactory)
        {
            _sourceFileService = sourceFileService;
            _coverallsService = coverallsService;
            _parser = parser;
            _logger = logger;
            _environmentVariablesJobProviderFactory = environmentVariablesJobProviderFactory;
        }
        
        public async Task RunAsync(CommandOptions commandOptions)
        {
            _logger.LogInformation(
                "Parsing the input file {input}", 
                commandOptions.Input);
        
            var fileCoverages = _parser.Parse(commandOptions.Input);

            _logger.LogInformation("Converting FileCoverage to SourceFile...");
            var sourceFiles = await _sourceFileService.CreateManyAsync(fileCoverages, commandOptions);

            _logger.LogInformation("Fetching job data...");
            var environmentVariablesJobProvider = _environmentVariablesJobProviderFactory.Create();
            var job = environmentVariablesJobProvider.Load();
            job.SourceFiles = sourceFiles;
            job.RepositoryToken = commandOptions.Token ?? job.RepositoryToken;
        
            await _coverallsService.UploadAsync(job);
        }
    }
}