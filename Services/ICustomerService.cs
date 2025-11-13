using FlowTap.Api.Models;

namespace FlowTap.Api.Services;

public interface ICustomerService
{
    Task<List<CustomerDto>> GetCustomersAsync(string? search);
    Task<Customer> CreateCustomerAsync(CreateCustomerRequest request);
    Task<Customer> UpdateCustomerAsync(Guid id, UpdateCustomerRequest request);
    Task<List<CustomerHistoryDto>> GetCustomerHistoryAsync(Guid customerId);
}

public class CreateCustomerRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public Guid? StoreId { get; set; }
}

public class UpdateCustomerRequest
{
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
}

public class CustomerDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public Guid? StoreId { get; set; }
    public int LoyaltyPoints { get; set; }
    public decimal TotalSpent { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CustomerHistoryDto
{
    public Guid Id { get; set; }
    public string ReferenceType { get; set; } = string.Empty;
    public Guid? ReferenceId { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

