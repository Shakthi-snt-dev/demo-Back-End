namespace Flowtap_Application.DtoModel.Response;

/// <summary>
/// Simple DTO for store dropdown/list items
/// </summary>
public class StoreListItemDto
{
    public Guid Id { get; set; }
    public string StoreName { get; set; } = string.Empty;
}

