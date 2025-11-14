namespace Flowtap_Domain.Exceptions;

public class UnauthorizedException : ApplicationException
{
    public UnauthorizedException(string message) : base(message)
    {
    }

    public UnauthorizedException(string message, string email) : base(message)
    {
        Email = email;
    }
}

