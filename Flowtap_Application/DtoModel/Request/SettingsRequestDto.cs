using System.ComponentModel.DataAnnotations;
using Flowtap_Domain.SharedKernel.ValueObjects;
using Flowtap_Domain.SharedKernel.Enums;

namespace Flowtap_Application.DtoModel.Request;

// User Profile Update DTO
public class UpdateUserProfileRequestDto
{
    [MaxLength(50)]
    public string? Username { get; set; }

    [MaxLength(320)]
    [EmailAddress]
    public string? Email { get; set; }

    [MaxLength(10)]
    public string? AccessPin { get; set; }

    [MaxLength(50)]
    public string? Language { get; set; }

    public string? ProfilePictureUrl { get; set; }

    public Guid? DefaultStoreId { get; set; }

    [MaxLength(20)]
    public string? Phone { get; set; }

    [MaxLength(20)]
    public string? Mobile { get; set; }

    public Address? Address { get; set; }

    public bool? EnableTwoFactor { get; set; }

    public UserType? UserType { get; set; }
}

// Store Settings Update DTO
public class UpdateStoreSettingsRequestDto
{
    [MaxLength(200)]
    public string? BusinessName { get; set; }

    [MaxLength(320)]
    [EmailAddress]
    public string? StoreEmail { get; set; }

    [MaxLength(200)]
    public string? AlternateName { get; set; }

    public string? StoreLogoUrl { get; set; }

    [MaxLength(20)]
    public string? Phone { get; set; }

    [MaxLength(20)]
    public string? Mobile { get; set; }

    [MaxLength(500)]
    public string? Website { get; set; }

    public Address? Address { get; set; }

    [MaxLength(100)]
    public string? TimeZone { get; set; }

    [MaxLength(20)]
    public string? TimeFormat { get; set; }

    [MaxLength(50)]
    public string? Language { get; set; }

    [MaxLength(10)]
    public string? DefaultCurrency { get; set; }

    [MaxLength(50)]
    public string? PriceFormat { get; set; }

    [MaxLength(20)]
    public string? DecimalFormat { get; set; }

    public bool? ChargeSalesTax { get; set; }

    [MaxLength(100)]
    public string? DefaultTaxClass { get; set; }

    public decimal? TaxPercentage { get; set; }

    [MaxLength(100)]
    public string? RegistrationNumber { get; set; }

    [MaxLength(10)]
    public string? StartTime { get; set; }

    [MaxLength(10)]
    public string? EndTime { get; set; }

    public Address? DefaultAddress { get; set; }

    [MaxLength(500)]
    public string? ApiKey { get; set; }

    [MaxLength(50)]
    public string? AccountingMethod { get; set; } // "Cash Basis" or "Accrual Basis"

    [MaxLength(320)]
    [EmailAddress]
    public string? CompanyEmail { get; set; }

    public bool? CompanyEmailVerified { get; set; }

    public bool? EmailNotifications { get; set; }

    public bool? RequireTwoFactorForAllUsers { get; set; }

    public bool? ChargeRestockingFee { get; set; }

    public decimal? DiagnosticBenchFee { get; set; }

    public bool? ChargeDepositOnRepairs { get; set; }

    public int? LockScreenTimeoutMinutes { get; set; }
}

public class UpdateGeneralSettingsRequestDto
{
    [MaxLength(200)]
    public string? FirstName { get; set; }

    [MaxLength(200)]
    public string? LastName { get; set; }

    [MaxLength(320)]
    [EmailAddress]
    public string? Email { get; set; }

    [MaxLength(20)]
    public string? Phone { get; set; }

    [MaxLength(100)]
    public string? TimeZone { get; set; }

    [MaxLength(10)]
    public string? Currency { get; set; }

    [MaxLength(50)]
    public string? DateFormat { get; set; }
}

public class UpdateInventorySettingsRequestDto
{
    public bool? EnableInventoryTracking { get; set; }
    public bool? SerializedInventory { get; set; }
    public bool? LowStockAlerts { get; set; }
    public int? DefaultLowStockThreshold { get; set; }
    public bool? AutoDeductStockOnSale { get; set; }
    public bool? AllowNegativeStock { get; set; }
    public bool? EnablePurchaseOrders { get; set; }
    public string? StockDisplayFormat { get; set; }
    public string? InventoryCountFrequency { get; set; }
    public string? AutoReorder { get; set; }
    public bool? EnableStockTransfers { get; set; }
    public bool? RequireTransferApproval { get; set; }
    public bool? RequireTransferReason { get; set; }
}

public class UpdateNotificationSettingsRequestDto
{
    public bool? EmailNotifications { get; set; }
    public bool? SmsNotifications { get; set; }
    public bool? LowStockAlerts { get; set; }
    public bool? NewOrderNotifications { get; set; }
    public bool? TicketUpdates { get; set; }
    public bool? DailySalesReport { get; set; }
    public bool? MarketingUpdates { get; set; }
    public bool? GoogleReviewsAutomation { get; set; }
    public int? ReviewRequestDelayHours { get; set; }
    public string? DailyDigestTime { get; set; }
    public string? SmsSenderId { get; set; }
}

public class UpdatePaymentSettingsRequestDto
{
    public bool? AcceptCash { get; set; }
    public bool? AcceptCard { get; set; }
    public bool? AcceptDigitalWallets { get; set; }
    public bool? EnableRDPaymentWidget { get; set; }
    public string? DefaultPaymentProvider { get; set; }
    public decimal? DefaultTaxRate { get; set; }
    public decimal? CardSurcharge { get; set; }
    public bool? EnableTips { get; set; }
    public decimal? TipOption1 { get; set; }
    public decimal? TipOption2 { get; set; }
    public decimal? TipOption3 { get; set; }
    public bool? AllowPartialPayments { get; set; }
    public string? ReceiptFooterMessage { get; set; }
}

public class UpdateSecuritySettingsRequestDto
{
    [MaxLength(200)]
    public string? CurrentPassword { get; set; }

    [MaxLength(200)]
    public string? NewPassword { get; set; }

    [MaxLength(200)]
    public string? ConfirmPassword { get; set; }

    public bool? EnableTwoFactor { get; set; }
}

// Check User Type DTOs
public class CheckUserTypeRequestDto
{
    [Required]
    [EmailAddress]
    [MaxLength(320)]
    public string Email { get; set; } = string.Empty;
}

// Update AppUser Profile DTO
public class UpdateAppUserProfileRequestDto
{
    [MaxLength(200)]
    public string? FirstName { get; set; }

    [MaxLength(200)]
    public string? LastName { get; set; }

    [MaxLength(200)]
    public string? Password { get; set; }

    [MaxLength(10)]
    public string? AccessPin { get; set; }

    public Address? Address { get; set; }
}

// AppUser Profile Settings DTOs (flat structure)
public class AppUserProfileRequestDto
{
    [MaxLength(50)]
    public string? Username { get; set; }

    [MaxLength(320)]
    [EmailAddress]
    public string? Email { get; set; }

    [MaxLength(50)]
    public string? Language { get; set; }

    [MaxLength(20)]
    public string? Phone { get; set; }

    [MaxLength(20)]
    public string? Mobile { get; set; }

    [MaxLength(50)]
    public string? StreetNumber { get; set; }

    [MaxLength(200)]
    public string? StreetName { get; set; }

    [MaxLength(100)]
    public string? City { get; set; }

    [MaxLength(100)]
    public string? State { get; set; }

    [MaxLength(100)]
    public string? Country { get; set; }

    [MaxLength(20)]
    public string? PostalCode { get; set; }

    public Guid? DefaultStoreId { get; set; }

    public bool? EnableTwoFactor { get; set; }
}

