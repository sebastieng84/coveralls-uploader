#nullable enable
namespace coveralls_uploader.Utilities
{
    public interface IEnvironmentWrapper
    {
        public string? GetEnvironmentVariable(string variable);
    }
}

