namespace coveralls_uploader.Utilities;

public class EnvironmentWrapper : IEnvironmentWrapper
{
    public virtual string? GetEnvironmentVariable(string variable)
    {
        return Environment.GetEnvironmentVariable(variable);
    }
}