namespace AccountService.Api.Exceptions;

public class UnprocessableException : Exception
{
    public UnprocessableException()
    {
    }

    public UnprocessableException(string message) : base(message)
    {
    }

    public UnprocessableException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
