namespace Flowtap_Domain.Exceptions;

public class InvalidOperationException : ApplicationException
{
    public InvalidOperationException(string message) : base(message)
    {
    }

    public InvalidOperationException(string message, string entityName) : base(message)
    {
        EntityName = entityName;
    }

    public InvalidOperationException(string message, string entityName, Dictionary<string, string> identifiers) : base(message)
    {
        EntityName = entityName;
        Identifiers = identifiers;
    }
}

