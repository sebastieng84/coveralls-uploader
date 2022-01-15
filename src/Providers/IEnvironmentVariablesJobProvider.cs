using coveralls_uploader.Models.Coveralls;

namespace coveralls_uploader.Providers
{
    public interface IEnvironmentVariablesJobProvider
    {
        public string ServiceName { get; }
        Job Load();
    }
}