using System.ComponentModel.DataAnnotations;
using Flowtap_Domain.SharedKernel.ValueObjects;

namespace Flowtap_Domain.BoundedContexts.Store.Entities;

public class StoreSettings
{
    [Key]
    public Guid Id { get; set; }

    public Guid StoreId { get; set; }

    // Basic Information
    [MaxLength(320)]
    [EmailAddress]
    public string? StoreEmail { get; set; }

    [MaxLength(200)]
    public string? AlternateName { get; set; }

    [MaxLength(500)]
    public string? StoreLogoUrl { get; set; }

    // Contact Information
    [MaxLength(20)]
    public string? Mobile { get; set; }

    [MaxLength(500)]
    public string? Website { get; set; }

    // Settings
    public bool EnablePOS { get; set; } = true;

    public bool EnableInventory { get; set; } = true;

    [MaxLength(100)]
    public string TimeZone { get; set; } = "UTC";

    [MaxLength(20)]
    public string TimeFormat { get; set; } = "12h"; // "12h" or "24h"

    [MaxLength(50)]
    public string Language { get; set; } = "en";

    [MaxLength(10)]
    public string DefaultCurrency { get; set; } = "USD";

    [MaxLength(50)]
    public string PriceFormat { get; set; } = "$0.00";

    [MaxLength(20)]
    public string DecimalFormat { get; set; } = "2"; // Decimal places

    // Sales Tax
    public bool ChargeSalesTax { get; set; } = false;

    [MaxLength(100)]
    public string? DefaultTaxClass { get; set; }

    public decimal TaxPercentage { get; set; } = 0;

    // Business Information
    [MaxLength(100)]
    public string? RegistrationNumber { get; set; }

    [MaxLength(10)]
    public string? StartTime { get; set; } // Business hours start time

    [MaxLength(10)]
    public string? EndTime { get; set; } // Business hours end time

    public Address? DefaultAddress { get; set; }

    // API Key
    [MaxLength(500)]
    public string? ApiKey { get; set; }

    public DateTime? ApiKeyCreatedAt { get; set; }

    // Accounting
    [MaxLength(50)]
    public string AccountingMethod { get; set; } = "Cash Basis"; // "Cash Basis" or "Accrual Basis"

    // Company Email
    [MaxLength(320)]
    [EmailAddress]
    public string? CompanyEmail { get; set; }

    public bool CompanyEmailVerified { get; set; } = false;

    // Email Notifications
    public bool EmailNotifications { get; set; } = true;

    // Security
    public bool RequireTwoFactorForAllUsers { get; set; } = false;

    // Restocking Fee
    public bool ChargeRestockingFee { get; set; } = false;

    // Deposit
    public decimal DiagnosticBenchFee { get; set; } = 0;

    public bool ChargeDepositOnRepairs { get; set; } = false;

    // Lock Screen
    public int LockScreenTimeoutMinutes { get; set; } = 15;

    // Other
    public string? BusinessHoursJson { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

