namespace Flowtap_Domain.Exceptions;

public class EntityNotFoundException : ApplicationException
{
    public EntityNotFoundException(string entityName, string identifierKey, string identifierValue) 
        : base($"{entityName} with {identifierKey} '{identifierValue}' not found")
    {
        EntityName = entityName;
        Identifiers[identifierKey] = identifierValue;
    }

    public EntityNotFoundException(string entityName, Guid id) 
        : this(entityName, "Id", id.ToString())
    {
    }
}

