using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using coveralls_uploader.Models.Coveralls;
using coveralls_uploader.Models.Coveralls.Git;
using coveralls_uploader.Parsers;

namespace coveralls_uploader.Services
{
    public class MainService 
    {
        private readonly SourceFileService _sourceFileService;
        private readonly CoverallsService _coverallsService;
        private readonly IParser _parser;

        public MainService(
            SourceFileService sourceFileService,
            CoverallsService coverallsService,
            IParser parser)
        {
            _sourceFileService = sourceFileService;
            _coverallsService = coverallsService;
            _parser = parser;
        }
        
        public async Task RunAsync()
        {
            var fileCoverages = _parser.Parse(@"C:\Users\sebas\Downloads\lcov.info");

            var sourceFiles = await Task.WhenAll(fileCoverages.Select(async file => await _sourceFileService.CreateAsync(file)));

            var job = new Job
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
            };

            await _coverallsService.UploadAsync(job);
        }
    }
}