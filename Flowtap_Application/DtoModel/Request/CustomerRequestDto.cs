using System.ComponentModel.DataAnnotations;
using Flowtap_Application.DtoModel;

namespace Flowtap_Application.DtoModel.Request;

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

