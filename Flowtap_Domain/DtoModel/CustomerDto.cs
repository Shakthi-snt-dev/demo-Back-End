using System.ComponentModel.DataAnnotations;

namespace Flowtap_Domain.DtoModel;

public class CreateCustomerRequestDto
{
    // StoreId is optional - will be extracted from JWT token if not provided
    public Guid StoreId { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(320)]
    [EmailAddress]
    public string? Email { get; set; }

    [MaxLength(20)]
    public string? Phone { get; set; }

    public AddressDto? Address { get; set; }
}

public class UpdateCustomerRequestDto
{
    [MaxLength(200)]
    public string? Name { get; set; }

    [MaxLength(320)]
    [EmailAddress]
    public string? Email { get; set; }

    [MaxLength(20)]
    public string? Phone { get; set; }

    public AddressDto? Address { get; set; }

    [MaxLength(50)]
    public string? Status { get; set; }
}

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

public class AddressDto
{
    public string StreetNumber { get; set; } = string.Empty;
    public string StreetName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
}

