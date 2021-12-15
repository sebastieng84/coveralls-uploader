using System.IO.Abstractions;
using coveralls_uploader.Parsers;
using coveralls_uploader.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) => services
        .AddHostedService<MainHostedService>()
        .AddSingleton<IFileSystem, FileSystem>()
        .AddSingleton<SourceFileService>()
        .AddSingleton<IParser, LcovParser>()
        .AddSingleton<CoverallsService>())
    .Build()
    .RunAsync();




