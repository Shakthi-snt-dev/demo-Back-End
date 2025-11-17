using System.ComponentModel.DataAnnotations;
using Flowtap_Domain.SharedKernel.ValueObjects;
using Flowtap_Domain.SharedKernel.Enums;

namespace Flowtap_Domain.DtoModel;

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

public class SettingsResponseDto
{
    public GeneralSettingsDto General { get; set; } = new GeneralSettingsDto();
    public InventorySettingsDto Inventory { get; set; } = new InventorySettingsDto();
    public NotificationSettingsDto Notifications { get; set; } = new NotificationSettingsDto();
    public PaymentSettingsDto Payment { get; set; } = new PaymentSettingsDto();
    public SecuritySettingsDto Security { get; set; } = new SecuritySettingsDto();
}

public class UserProfileDto
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? AccessPin { get; set; }
    public string? Language { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public Guid? DefaultStoreId { get; set; }
    public string? Phone { get; set; }
    public string? Mobile { get; set; }
    public Address? Address { get; set; }
    public bool EnableTwoFactor { get; set; } = false;
    public UserType UserType { get; set; } = UserType.AppUser;
}

public class StoreSettingsDto
{
    public string? BusinessName { get; set; }
    public string? StoreEmail { get; set; }
    public string? AlternateName { get; set; }
    public string? StoreLogoUrl { get; set; }
    public string? Phone { get; set; }
    public string? Mobile { get; set; }
    public string? Website { get; set; }
    public Address? Address { get; set; }
    public string TimeZone { get; set; } = "UTC";
    public string TimeFormat { get; set; } = "12h";
    public string Language { get; set; } = "en";
    public string DefaultCurrency { get; set; } = "USD";
    public string PriceFormat { get; set; } = "$0.00";
    public string DecimalFormat { get; set; } = "2";
    public bool ChargeSalesTax { get; set; } = false;
    public string? DefaultTaxClass { get; set; }
    public decimal TaxPercentage { get; set; } = 0;
    public string? RegistrationNumber { get; set; }
    public string? StartTime { get; set; }
    public string? EndTime { get; set; }
    public Address? DefaultAddress { get; set; }
    public string? ApiKey { get; set; }
    public string AccountingMethod { get; set; } = "Cash Basis";
    public string? CompanyEmail { get; set; }
    public bool CompanyEmailVerified { get; set; } = false;
    public bool EmailNotifications { get; set; } = true;
    public bool RequireTwoFactorForAllUsers { get; set; } = false;
    public bool ChargeRestockingFee { get; set; } = false;
    public decimal DiagnosticBenchFee { get; set; } = 0;
    public bool ChargeDepositOnRepairs { get; set; } = false;
    public int LockScreenTimeoutMinutes { get; set; } = 15;
}

public class GeneralSettingsDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string TimeZone { get; set; } = "UTC";
    public string Currency { get; set; } = "USD";
    public string DateFormat { get; set; } = "MM/DD/YYYY";
}

public class InventorySettingsDto
{
    public bool EnableInventoryTracking { get; set; } = true;
    public bool SerializedInventory { get; set; } = false;
    public bool LowStockAlerts { get; set; } = true;
    public int DefaultLowStockThreshold { get; set; } = 20;
    public bool AutoDeductStockOnSale { get; set; } = true;
    public bool AllowNegativeStock { get; set; } = false;
    public bool EnablePurchaseOrders { get; set; } = true;
    public string StockDisplayFormat { get; set; } = "quantity";
    public string InventoryCountFrequency { get; set; } = "monthly";
    public string AutoReorder { get; set; } = "off";
    public bool EnableStockTransfers { get; set; } = true;
    public bool RequireTransferApproval { get; set; } = true;
    public bool RequireTransferReason { get; set; } = true;
}

public class NotificationSettingsDto
{
    public bool EmailNotifications { get; set; } = true;
    public bool SmsNotifications { get; set; } = false;
    public bool LowStockAlerts { get; set; } = true;
    public bool NewOrderNotifications { get; set; } = true;
    public bool TicketUpdates { get; set; } = true;
    public bool DailySalesReport { get; set; } = false;
    public bool MarketingUpdates { get; set; } = false;
    public bool GoogleReviewsAutomation { get; set; } = true;
    public int ReviewRequestDelayHours { get; set; } = 24;
    public string DailyDigestTime { get; set; } = "18:00";
    public string? SmsSenderId { get; set; }
}

public class PaymentSettingsDto
{
    public bool AcceptCash { get; set; } = true;
    public bool AcceptCard { get; set; } = true;
    public bool AcceptDigitalWallets { get; set; } = true;
    public bool EnableRDPaymentWidget { get; set; } = true;
    public string DefaultPaymentProvider { get; set; } = "rd";
    public decimal DefaultTaxRate { get; set; } = 10;
    public decimal CardSurcharge { get; set; } = 0;
    public bool EnableTips { get; set; } = true;
    public decimal TipOption1 { get; set; } = 10;
    public decimal TipOption2 { get; set; } = 15;
    public decimal TipOption3 { get; set; } = 20;
    public bool AllowPartialPayments { get; set; } = true;
    public string ReceiptFooterMessage { get; set; } = "Thank you for choosing RepairPOS!";
}

public class SecuritySettingsDto
{
    public bool TwoFactorEnabled { get; set; } = false;
}

