#nullable enable
namespace coveralls_uploader.Utilities.Wrappers
{
    public interface IEnvironment
    {
        public string? GetEnvironmentVariable(string variable);
    }
}