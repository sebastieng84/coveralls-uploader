using coveralls_uploader.Models.Coveralls;

namespace coveralls_uploader.Providers
{
    public interface IGitDataProvider
    {
        Git Load(string commitSha);
    }
}