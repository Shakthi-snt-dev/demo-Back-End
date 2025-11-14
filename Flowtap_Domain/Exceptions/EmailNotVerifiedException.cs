namespace Flowtap_Domain.Exceptions;

public class EmailNotVerifiedException : ApplicationException
{
    public EmailNotVerifiedException(string email) : base($"Email {email} has not been verified")
    {
        Email = email;
        EntityName = "UserAccount";
        Identifiers["Email"] = email;
    }
}

