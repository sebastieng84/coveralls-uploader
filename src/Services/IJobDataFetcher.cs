using coveralls_uploader.Models;
using coveralls_uploader.Models.Coveralls;

namespace coveralls_uploader.Services;

public interface IJobDataFetcher
{
    Job Fetch(CommandOptions commandOptions);
}