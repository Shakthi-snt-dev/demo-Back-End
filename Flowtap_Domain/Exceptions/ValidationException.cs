namespace Flowtap_Domain.Exceptions;

public class ValidationException : ApplicationException
{
    public Dictionary<string, string[]> ValidationErrors { get; set; } = new Dictionary<string, string[]>();

    public ValidationException(string message) : base(message)
    {
    }

    public ValidationException(string message, Dictionary<string, string[]> validationErrors) : base(message)
    {
        ValidationErrors = validationErrors;
    }
}

