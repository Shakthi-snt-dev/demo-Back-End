using FlowTap.Api.Models;

namespace FlowTap.Api.Services;

public interface ISettingsService
{
    Task<StoreDto> GetStoreSettingsAsync();
    Task<Store> UpdateStoreSettingsAsync(UpdateStoreRequest request);
    Task<BusinessSettingsDto> GetBusinessSettingsAsync();
    Task<BusinessSettings> UpdateBusinessSettingsAsync(UpdateBusinessSettingsRequest request);
}

public class UpdateStoreRequest
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? Zip { get; set; }
    public string? TimeZone { get; set; }
    public string? Currency { get; set; }
    public decimal? TaxRate { get; set; }
}

public class UpdateBusinessSettingsRequest
{
    public string? BusinessName { get; set; }
    public string? LogoUrl { get; set; }
    public Guid? DefaultStoreId { get; set; }
    public string? TimeFormat { get; set; }
    public string? Language { get; set; }
    public string? DefaultCurrency { get; set; }
    public bool? TaxIncluded { get; set; }
    public string? AccountingMethod { get; set; }
}

public class StoreDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? Zip { get; set; }
    public string? TimeZone { get; set; }
    public string? Currency { get; set; }
    public decimal TaxRate { get; set; }
}

public class BusinessSettingsDto
{
    public Guid Id { get; set; }
    public string BusinessName { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public Guid? DefaultStoreId { get; set; }
    public string TimeFormat { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public string DefaultCurrency { get; set; } = string.Empty;
    public bool TaxIncluded { get; set; }
    public string AccountingMethod { get; set; } = string.Empty;
}

