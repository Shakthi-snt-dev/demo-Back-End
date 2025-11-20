using Flowtap_Application.DtoModel;

namespace Flowtap_Application.DtoModel.Response;

public class StoreResponseDto
{
    public Guid Id { get; set; }
    public Guid AppUserId { get; set; }
    public string StoreName { get; set; } = string.Empty;
    public string? StoreType { get; set; }
    public string? StoreCategory { get; set; }
    public string? Phone { get; set; }
    public AddressDto? Address { get; set; }
    public bool EnablePOS { get; set; }
    public bool EnableInventory { get; set; }
    public string TimeZone { get; set; } = "UTC";
    public int EmployeeCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class StoreTypeResponseDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int StoreCount { get; set; }
}

public class StoreTypeDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

