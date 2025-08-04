namespace AccountService.Api.Exceptions;

public class UnauthrorizedException : Exception
{
    public UnauthrorizedException()
    {
    }

    public UnauthrorizedException(string message) : base(message)
    {
    }

    public UnauthrorizedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}

