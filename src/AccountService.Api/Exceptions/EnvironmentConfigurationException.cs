namespace AccountService.Api.Exceptions;

public class EnvironmentConfigurationException : Exception
{
    public EnvironmentConfigurationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public EnvironmentConfigurationException(string? message) : base(message)
    {
    }

    public EnvironmentConfigurationException()
    {
    }
}