using coveralls_uploader.Models;
using coveralls_uploader.Parsers;
using Microsoft.Extensions.Logging;

namespace coveralls_uploader.Services;

public class MainService 
{
    private readonly SourceFileService _sourceFileService;
    private readonly CoverallsService _coverallsService;
    private readonly IParser _parser;
    private readonly IJobDataFetcher _jobDataFetcher;
    private readonly ILogger _logger;

    public MainService(
        SourceFileService sourceFileService,
        CoverallsService coverallsService,
        IParser parser,
        IJobDataFetcher jobDataFetcher,
        ILogger logger)
    {
        _sourceFileService = sourceFileService;
        _coverallsService = coverallsService;
        _parser = parser;
        _jobDataFetcher = jobDataFetcher;
        _logger = logger;
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
        var job = _jobDataFetcher.Fetch(commandOptions);
        job.SourceFiles = sourceFiles;
        
        await _coverallsService.UploadAsync(job);
    }
}

/*new Job
      {
          RepositoryToken = "DQyGHUKIznRy7m0DLDpiduTVFSeA9NQZE",
          SourceFiles = sourceFiles.ToList(),
          CommitSha = "6b762439fc709702f2c15c0936a7293092d9e28b",
          ServiceName = "jenkins",
          ServiceNumber = "16",
          RunAt = "2020-12-15 10:45:00 -0800",
          GitInformation = new GitInformation()
          {
              Head = new Head
              {
                  Id = "6b762439fc709702f2c15c0936a7293092d9e28b",
                  AuthorName = "Sébastien Girard",
                  AuthorEmail = "sebastien.girard@amilia.com",
                  CommitterName = "Sébastien Girard",
                  CommitterEmail = "sebastien.girard@amilia.com",
                  Message = "Fix upload-artifact path"
              },
              Branch = "origin/master",
              Remotes = new List<Remote>
              {
                  new("origin", "https://github.com/sebastieng84/coveralls-uploader.git")
              }
          }
      };*/