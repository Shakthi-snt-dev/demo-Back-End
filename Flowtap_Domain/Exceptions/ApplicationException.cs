namespace Flowtap_Domain.Exceptions;

public class ApplicationException : Exception
{
    public string Email { get; set; } = string.Empty;
    public string Service { get; set; } = string.Empty;
    public Dictionary<string, string> Identifiers { get; set; } = new Dictionary<string, string>();
    public string EntityName { get; set; } = string.Empty;
    public Exception? UnderlyingError { get; set; }

    public ApplicationException() : base()
    {
    }

    public ApplicationException(string message) : base(message)
    {
    }

    public ApplicationException(string message, Exception innerException) : base(message, innerException)
    {
        UnderlyingError = innerException;
    }

    public string GetMessage()
    {
        if (Identifiers.Count == 0 && string.IsNullOrEmpty(Email))
        {
            return Message ?? base.Message ?? string.Empty;
        }

        var message = string.IsNullOrEmpty(EntityName) ? "Entity" : EntityName;
        message += " with ";

        var identifierParts = new List<string>();
        foreach (var (key, value) in Identifiers)
        {
            identifierParts.Add($"{key} => {value}");
        }

        if (identifierParts.Count > 0)
        {
            message += string.Join(", ", identifierParts);
        }

        if (!string.IsNullOrEmpty(Email))
        {
            message += $" for user - {Email}";
        }

        return message;
    }

    public override string ToString()
    {
        return GetMessage();
    }
}

