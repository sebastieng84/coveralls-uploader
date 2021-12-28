using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using System.IO.Abstractions;
using coveralls_uploader;
using coveralls_uploader.Parsers;
using coveralls_uploader.Services;
using Microsoft.Extensions.DependencyInjection;

var builder = new CommandLineBuilder(new UploadCommand());
var parser = builder
    .UseDefaults()
    .UseHost(host =>
    {
        host.ConfigureServices((_, services) =>
        {
            services
                .AddTransient<MainService>()
                .AddSingleton<IFileSystem, FileSystem>()
                .AddSingleton<SourceFileService>()
                .AddSingleton<IParser, LcovParser>()
                .AddSingleton<CoverallsService>();
        });

        host.UseCommandHandler<UploadCommand, UploadCommandHandler>();
    })
    .Build();

return await parser.InvokeAsync(args);
    
    