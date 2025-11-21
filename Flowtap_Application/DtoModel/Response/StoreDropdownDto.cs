namespace Flowtap_Application.DtoModel.Response;

/// <summary>
/// Simple DTO for store dropdown selection
/// </summary>
public class StoreDropdownDto
{
    public Guid Id { get; set; }
    public string StoreName { get; set; } = string.Empty;
}

