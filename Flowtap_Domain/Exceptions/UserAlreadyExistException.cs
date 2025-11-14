namespace Flowtap_Domain.Exceptions;

public class UserAlreadyExistException : ApplicationException
{
    public UserAlreadyExistException(string email) : base($"User with email {email} already exists")
    {
        Email = email;
        EntityName = "User";
        Identifiers["Email"] = email;
    }

    public UserAlreadyExistException(string email, string username) : base($"User with email {email} or username {username} already exists")
    {
        Email = email;
        EntityName = "User";
        Identifiers["Email"] = email;
        if (!string.IsNullOrWhiteSpace(username))
        {
            Identifiers["Username"] = username;
        }
    }
}

