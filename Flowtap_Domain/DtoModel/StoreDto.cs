using System.ComponentModel.DataAnnotations;

namespace Flowtap_Domain.DtoModel;

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

public class StoreTypeDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class CreateStoreTypeRequestDto
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }
}

public class StoreTypeResponseDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int StoreCount { get; set; }
}

