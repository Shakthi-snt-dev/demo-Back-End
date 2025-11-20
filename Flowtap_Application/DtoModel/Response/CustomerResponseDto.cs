using Flowtap_Application.DtoModel;

namespace Flowtap_Application.DtoModel.Response;

public class CustomerResponseDto
{
    public Guid Id { get; set; }
    public Guid StoreId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public AddressDto? Address { get; set; }
    public int TotalOrders { get; set; }
    public decimal TotalSpent { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

