using System.ComponentModel.DataAnnotations;
using Flowtap_Application.DtoModel;

namespace Flowtap_Application.DtoModel.Request;

public class CreateStoreRequestDto
{
    [Required]
    public Guid AppUserId { get; set; }

    [Required, MaxLength(200)]
    public string StoreName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? StoreType { get; set; }

    [MaxLength(100)]
    public string? StoreCategory { get; set; }

    [MaxLength(20)]
    public string? Phone { get; set; }

    public AddressDto? Address { get; set; }

    public bool? EnablePOS { get; set; } = true;
    public bool? EnableInventory { get; set; } = true;
    public string? TimeZone { get; set; } = "UTC";
}

public class UpdateStoreRequestDto
{
    [MaxLength(200)]
    public string? StoreName { get; set; }

    [MaxLength(100)]
    public string? StoreType { get; set; }

    [MaxLength(100)]
    public string? StoreCategory { get; set; }

    [MaxLength(20)]
    public string? Phone { get; set; }

    public AddressDto? Address { get; set; }

    public bool? EnablePOS { get; set; }
    public bool? EnableInventory { get; set; }
    public string? TimeZone { get; set; }
}

public class CreateStoreTypeRequestDto
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }
}

