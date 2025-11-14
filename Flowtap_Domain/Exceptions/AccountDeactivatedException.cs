namespace Flowtap_Domain.Exceptions;

public class AccountDeactivatedException : ApplicationException
{
    public AccountDeactivatedException(string email) : base($"Account with email {email} has been deactivated")
    {
        Email = email;
        EntityName = "UserAccount";
        Identifiers["Email"] = email;
    }
}

