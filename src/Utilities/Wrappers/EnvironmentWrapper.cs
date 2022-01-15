using System.Diagnostics.CodeAnalysis;

#nullable enable

namespace coveralls_uploader.Utilities.Wrappers
{
    [ExcludeFromCodeCoverage]
    public class EnvironmentWrapper : IEnvironment
    {
        public string? GetEnvironmentVariable(string variable)
        {
            return System.Environment.GetEnvironmentVariable(variable);
        }
    }
}