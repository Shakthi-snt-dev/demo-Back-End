using System.ComponentModel.DataAnnotations;

namespace Flowtap_Application.DtoModel.Request;

public class UpgradeToSubscriptionRequestDto
{
    [Required(ErrorMessage = "Plan name is required")]
    public string PlanName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Billing interval is required")]
    public string BillingInterval { get; set; } = "Monthly"; // "Monthly" or "Yearly"

    [Range(1, int.MaxValue, ErrorMessage = "Max stores must be greater than 0")]
    public int MaxStores { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Price cannot be negative")]
    public decimal PricePerMonth { get; set; }

    public string? PaymentProvider { get; set; }

    public string? ExternalSubscriptionId { get; set; }
}

