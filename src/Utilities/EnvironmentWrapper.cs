namespace coveralls_uploader.Utilities;

public class EnvironmentWrapper : IEnvironmentWrapper
{
    public string? GetEnvironmentVariable(string variable)
    {
        return Environment.GetEnvironmentVariable(variable);
    }
}