// See https://aka.ms/new-console-template for more information

using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using coveralls_uploader.Models.Coveralls;
using coveralls_uploader.Models.Coveralls.Git;
using coveralls_uploader.Parsers;
using coveralls_uploader.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) => services
        .AddSingleton<IFileSystem, FileSystem>()
        .AddSingleton<SourceFileService>())
    .Build();
using IServiceScope serviceScope = host.Services.CreateScope();

var parser = new LcovParser();
var sourceFileService = serviceScope.ServiceProvider.GetRequiredService<SourceFileService>();
var coverallsService = new CoverallsService();

var fileCoverages = parser.Parse(@"C:\Users\sebas\Downloads\lcov.info");

var sourceFiles = await Task.WhenAll(fileCoverages.Select(async file => await sourceFileService.CreateAsync(file)));

var job = new Job
{
    RepositoryToken = "DQyGHUKIznRy7m0DLDpiduTVFSeA9NQZE",
    SourceFiles = sourceFiles.ToList(),
    CommitSha = "41b12011f0b0274652287c694c607c056ee177b0",
    ServiceName = "jenkins",
    ServiceNumber = "1374",
    RunAt = "2020-11-23 10:45:00 -0800",
    GitInformation = new GitInformation()
    {
        Head = new Head
        {
            Id = "41b12011f0b0274652287c694c607c056ee177b0",
            AuthorName = "Sébastien Girard",
            AuthorEmail = "sebastien.girard@amilia.com",
            CommitterName = "Sébastien Girard",
            CommitterEmail = "sebastien.girard@amilia.com",
            Message = "Help"
        },
        Branch = "origin/dev",
        Remotes = new List<Remote>
        {
            new("origin", "git@github.com:AmiliaApp/Amilia.git")
        }
    }
};

await coverallsService.UploadAsync(job);

await host.RunAsync();



