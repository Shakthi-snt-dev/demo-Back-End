namespace FlowTap.Api.Models;

public class BusinessSettings
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string BusinessName { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public Guid? DefaultStoreId { get; set; }
    public Store? DefaultStore { get; set; }
    public string TimeFormat { get; set; } = "12h";
    public string Language { get; set; } = "en";
    public string DefaultCurrency { get; set; } = "USD";
    public bool TaxIncluded { get; set; } = false;
    public string AccountingMethod { get; set; } = "Cash";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

