using Flowtap_Application.Interfaces;
using Flowtap_Domain.DtoModel;
using Flowtap_Domain.SharedKernel.Enums;
using Microsoft.Extensions.Logging;

namespace Flowtap_Application.Services;

public class FormConfigurationService : IFormConfigurationService
{
    private readonly ILogger<FormConfigurationService> _logger;

    public FormConfigurationService(ILogger<FormConfigurationService> logger)
    {
        _logger = logger;
    }

    public async Task<FormConfigurationDto> GetFormConfigurationAsync(string formId, Guid? appUserId = null, Guid? storeId = null)
    {
        return formId.ToLower() switch
        {
            "user-profile" => await GetUserProfileFormConfigurationAsync(appUserId ?? Guid.Empty),
            "store-settings" => await GetStoreSettingsFormConfigurationAsync(storeId ?? Guid.Empty),
            "employee" => await GetEmployeeFormConfigurationAsync(storeId),
            _ => throw new ArgumentException($"Unknown form ID: {formId}")
        };
    }

    public async Task<FormConfigurationDto> GetUserProfileFormConfigurationAsync(Guid appUserId)
    {
        await Task.CompletedTask; // Placeholder for async

        return new FormConfigurationDto
        {
            FormId = "user-profile",
            Title = "User Profile",
            Description = "Update your profile information",
            Groups = new List<string> { "Basic Information", "Contact Information", "Security" },
            Fields = new List<FormFieldDto>
            {
                // Basic Information Group
                new FormFieldDto
                {
                    Id = "username",
                    Label = "Username",
                    Type = "text",
                    Placeholder = "Enter username",
                    Required = false,
                    Order = 1,
                    Group = "Basic Information",
                    Icon = "User",
                    MaxLength = 50
                },
                new FormFieldDto
                {
                    Id = "email",
                    Label = "Email",
                    Type = "email",
                    Placeholder = "Enter email address",
                    Required = false,
                    Order = 2,
                    Group = "Basic Information",
                    Icon = "Mail",
                    ValidationPattern = @"^[^\s@]+@[^\s@]+\.[^\s@]+$",
                    ValidationMessage = "Please enter a valid email address"
                },
                new FormFieldDto
                {
                    Id = "accessPin",
                    Label = "Access PIN",
                    Type = "password",
                    Placeholder = "Enter PIN (4-10 digits)",
                    Required = false,
                    Order = 3,
                    Group = "Basic Information",
                    Icon = "Lock",
                    MinLength = 4,
                    MaxLength = 10,
                    ValidationPattern = @"^\d+$",
                    ValidationMessage = "PIN must contain only digits"
                },
                new FormFieldDto
                {
                    Id = "language",
                    Label = "Language",
                    Type = "select",
                    Required = false,
                    Order = 4,
                    Group = "Basic Information",
                    Icon = "Globe",
                    Options = new List<SelectOptionDto>
                    {
                        new SelectOptionDto { Value = "en", Label = "English" },
                        new SelectOptionDto { Value = "es", Label = "Spanish" },
                        new SelectOptionDto { Value = "fr", Label = "French" }
                    },
                    DefaultValue = "en"
                },
                new FormFieldDto
                {
                    Id = "userType",
                    Label = "User Type",
                    Type = "select",
                    Required = false,
                    Order = 5,
                    Group = "Basic Information",
                    Icon = "UserCircle",
                    Options = new List<SelectOptionDto>
                    {
                        new SelectOptionDto { Value = "AppUser", Label = "App User" },
                        new SelectOptionDto { Value = "Admin", Label = "Admin" },
                        new SelectOptionDto { Value = "Employee", Label = "Employee" }
                    },
                    DefaultValue = "AppUser"
                },
                // Contact Information Group
                new FormFieldDto
                {
                    Id = "phone",
                    Label = "Phone Number",
                    Type = "text",
                    Placeholder = "Enter phone number",
                    Required = false,
                    Order = 6,
                    Group = "Contact Information",
                    Icon = "Phone",
                    MaxLength = 20
                },
                new FormFieldDto
                {
                    Id = "mobile",
                    Label = "Mobile Number",
                    Type = "text",
                    Placeholder = "Enter mobile number",
                    Required = false,
                    Order = 7,
                    Group = "Contact Information",
                    Icon = "Phone",
                    MaxLength = 20
                },
                new FormFieldDto
                {
                    Id = "defaultStoreId",
                    Label = "Default Store",
                    Type = "select",
                    Required = false,
                    Order = 8,
                    Group = "Contact Information",
                    Icon = "Building2",
                    Options = new List<SelectOptionDto>(), // Will be populated dynamically
                    Metadata = new Dictionary<string, object> { { "loadFromApi", "/api/stores" } }
                },
                new FormFieldDto
                {
                    Id = "streetNumber",
                    Label = "Street Number",
                    Type = "text",
                    Placeholder = "Enter street number",
                    Required = false,
                    Order = 9,
                    Group = "Contact Information",
                    MaxLength = 50
                },
                new FormFieldDto
                {
                    Id = "streetName",
                    Label = "Street Name",
                    Type = "text",
                    Placeholder = "Enter street name",
                    Required = false,
                    Order = 10,
                    Group = "Contact Information",
                    MaxLength = 200
                },
                new FormFieldDto
                {
                    Id = "city",
                    Label = "City",
                    Type = "text",
                    Placeholder = "Enter city",
                    Required = false,
                    Order = 11,
                    Group = "Contact Information",
                    MaxLength = 100
                },
                new FormFieldDto
                {
                    Id = "state",
                    Label = "State",
                    Type = "text",
                    Placeholder = "Enter state",
                    Required = false,
                    Order = 12,
                    Group = "Contact Information",
                    MaxLength = 100
                },
                new FormFieldDto
                {
                    Id = "country",
                    Label = "Country",
                    Type = "text",
                    Placeholder = "Enter country",
                    Required = false,
                    Order = 13,
                    Group = "Contact Information",
                    MaxLength = 100
                },
                new FormFieldDto
                {
                    Id = "postalCode",
                    Label = "Postal Code",
                    Type = "text",
                    Placeholder = "Enter postal code",
                    Required = false,
                    Order = 14,
                    Group = "Contact Information",
                    MaxLength = 20
                },
                // Security Group
                new FormFieldDto
                {
                    Id = "enableTwoFactor",
                    Label = "Two-Factor Authentication",
                    Type = "switch",
                    Required = false,
                    Order = 15,
                    Group = "Security",
                    Icon = "Shield",
                    HelpText = "Add an extra layer of security",
                    DefaultValue = false
                }
            }
        };
    }

    public async Task<FormConfigurationDto> GetStoreSettingsFormConfigurationAsync(Guid storeId)
    {
        await Task.CompletedTask; // Placeholder for async

        return new FormConfigurationDto
        {
            FormId = "store-settings",
            Title = "Store Settings",
            Description = "Configure your store information and preferences",
            Groups = new List<string> { "Basic Information", "Contact Information", "Other Information", "Sales Tax", "Business Hours", "Company Email", "Email Notifications", "Security", "Restocking Fee", "Deposit", "Lock Screen", "API Key" },
            Fields = new List<FormFieldDto>
            {
                // Basic Information
                new FormFieldDto
                {
                    Id = "businessName",
                    Label = "Business Name",
                    Type = "text",
                    Placeholder = "Enter business name",
                    Required = false,
                    Order = 1,
                    Group = "Basic Information",
                    MaxLength = 200
                },
                new FormFieldDto
                {
                    Id = "storeEmail",
                    Label = "Store Email",
                    Type = "email",
                    Placeholder = "Enter store email",
                    Required = false,
                    Order = 2,
                    Group = "Basic Information",
                    Icon = "Mail"
                },
                new FormFieldDto
                {
                    Id = "alternateName",
                    Label = "Alternate Name",
                    Type = "text",
                    Placeholder = "Enter alternate name",
                    Required = false,
                    Order = 3,
                    Group = "Basic Information",
                    MaxLength = 200
                },
                // Contact Information
                new FormFieldDto
                {
                    Id = "phone",
                    Label = "Phone",
                    Type = "text",
                    Placeholder = "Enter phone number",
                    Required = false,
                    Order = 4,
                    Group = "Contact Information",
                    Icon = "Phone",
                    MaxLength = 20
                },
                new FormFieldDto
                {
                    Id = "mobile",
                    Label = "Mobile",
                    Type = "text",
                    Placeholder = "Enter mobile number",
                    Required = false,
                    Order = 5,
                    Group = "Contact Information",
                    Icon = "Phone",
                    MaxLength = 20
                },
                new FormFieldDto
                {
                    Id = "website",
                    Label = "Website",
                    Type = "url",
                    Placeholder = "https://example.com",
                    Required = false,
                    Order = 6,
                    Group = "Contact Information",
                    Icon = "Globe"
                },
                // Other Information
                new FormFieldDto
                {
                    Id = "timeZone",
                    Label = "Time Zone",
                    Type = "select",
                    Required = false,
                    Order = 7,
                    Group = "Other Information",
                    Icon = "Clock",
                    Options = new List<SelectOptionDto>
                    {
                        new SelectOptionDto { Value = "America/New_York", Label = "America/New_York" },
                        new SelectOptionDto { Value = "America/Chicago", Label = "America/Chicago" },
                        new SelectOptionDto { Value = "America/Denver", Label = "America/Denver" },
                        new SelectOptionDto { Value = "America/Los_Angeles", Label = "America/Los_Angeles" }
                    }
                },
                new FormFieldDto
                {
                    Id = "timeFormat",
                    Label = "Time Format",
                    Type = "select",
                    Required = false,
                    Order = 8,
                    Group = "Other Information",
                    Options = new List<SelectOptionDto>
                    {
                        new SelectOptionDto { Value = "12h", Label = "12 Hour" },
                        new SelectOptionDto { Value = "24h", Label = "24 Hour" }
                    },
                    DefaultValue = "12h"
                },
                new FormFieldDto
                {
                    Id = "language",
                    Label = "Language",
                    Type = "select",
                    Required = false,
                    Order = 9,
                    Group = "Other Information",
                    Options = new List<SelectOptionDto>
                    {
                        new SelectOptionDto { Value = "en", Label = "English" },
                        new SelectOptionDto { Value = "es", Label = "Spanish" },
                        new SelectOptionDto { Value = "fr", Label = "French" }
                    },
                    DefaultValue = "en"
                },
                new FormFieldDto
                {
                    Id = "defaultCurrency",
                    Label = "Default Currency",
                    Type = "select",
                    Required = false,
                    Order = 10,
                    Group = "Other Information",
                    Icon = "DollarSign",
                    Options = new List<SelectOptionDto>
                    {
                        new SelectOptionDto { Value = "USD", Label = "USD ($)" },
                        new SelectOptionDto { Value = "EUR", Label = "EUR (€)" },
                        new SelectOptionDto { Value = "GBP", Label = "GBP (£)" }
                    },
                    DefaultValue = "USD"
                },
                new FormFieldDto
                {
                    Id = "priceFormat",
                    Label = "Price Format",
                    Type = "text",
                    Placeholder = "$0.00",
                    Required = false,
                    Order = 11,
                    Group = "Other Information",
                    DefaultValue = "$0.00"
                },
                new FormFieldDto
                {
                    Id = "decimalFormat",
                    Label = "Decimal Format",
                    Type = "number",
                    Placeholder = "2",
                    Required = false,
                    Order = 12,
                    Group = "Other Information",
                    Min = 0,
                    Max = 10,
                    DefaultValue = 2
                },
                new FormFieldDto
                {
                    Id = "registrationNumber",
                    Label = "Registration Number",
                    Type = "text",
                    Placeholder = "Enter registration number",
                    Required = false,
                    Order = 13,
                    Group = "Other Information",
                    MaxLength = 100
                },
                new FormFieldDto
                {
                    Id = "accountingMethod",
                    Label = "Accounting Method",
                    Type = "select",
                    Required = false,
                    Order = 14,
                    Group = "Other Information",
                    Options = new List<SelectOptionDto>
                    {
                        new SelectOptionDto { Value = "Cash Basis", Label = "Cash Basis" },
                        new SelectOptionDto { Value = "Accrual Basis", Label = "Accrual Basis" }
                    },
                    DefaultValue = "Cash Basis"
                },
                // Sales Tax
                new FormFieldDto
                {
                    Id = "chargeSalesTax",
                    Label = "Charge Sales Tax",
                    Type = "switch",
                    Required = false,
                    Order = 15,
                    Group = "Sales Tax",
                    DefaultValue = false
                },
                new FormFieldDto
                {
                    Id = "defaultTaxClass",
                    Label = "Default Tax Class",
                    Type = "text",
                    Placeholder = "Enter tax class",
                    Required = false,
                    Order = 16,
                    Group = "Sales Tax",
                    Visible = false, // Shown conditionally when chargeSalesTax is true
                    Metadata = new Dictionary<string, object> { { "dependsOn", "chargeSalesTax" } }
                },
                new FormFieldDto
                {
                    Id = "taxPercentage",
                    Label = "Tax Percentage (%)",
                    Type = "number",
                    Placeholder = "0.00",
                    Required = false,
                    Order = 17,
                    Group = "Sales Tax",
                    Min = 0,
                    Max = 100,
                    Visible = false, // Shown conditionally when chargeSalesTax is true
                    Metadata = new Dictionary<string, object> { { "dependsOn", "chargeSalesTax" } }
                },
                // Business Hours
                new FormFieldDto
                {
                    Id = "startTime",
                    Label = "Start Time",
                    Type = "time",
                    Required = false,
                    Order = 18,
                    Group = "Business Hours"
                },
                new FormFieldDto
                {
                    Id = "endTime",
                    Label = "End Time",
                    Type = "time",
                    Required = false,
                    Order = 19,
                    Group = "Business Hours"
                },
                // Company Email
                new FormFieldDto
                {
                    Id = "companyEmail",
                    Label = "Company Email",
                    Type = "email",
                    Placeholder = "company@example.com",
                    Required = false,
                    Order = 20,
                    Group = "Company Email",
                    Icon = "Mail",
                    HelpText = "Recommended to use a company domain for reliable delivery"
                },
                // Email Notifications
                new FormFieldDto
                {
                    Id = "emailNotifications",
                    Label = "Receive all internal system emails",
                    Type = "switch",
                    Required = false,
                    Order = 21,
                    Group = "Email Notifications",
                    DefaultValue = true
                },
                // Security
                new FormFieldDto
                {
                    Id = "requireTwoFactorForAllUsers",
                    Label = "Require all users to set up two-factor authentication to log in",
                    Type = "switch",
                    Required = false,
                    Order = 22,
                    Group = "Security",
                    Icon = "Shield",
                    DefaultValue = false
                },
                // Restocking Fee
                new FormFieldDto
                {
                    Id = "chargeRestockingFee",
                    Label = "Charge a restocking fee for returns",
                    Type = "switch",
                    Required = false,
                    Order = 23,
                    Group = "Restocking Fee",
                    DefaultValue = false
                },
                // Deposit
                new FormFieldDto
                {
                    Id = "diagnosticBenchFee",
                    Label = "Diagnostic/Bench Fee",
                    Type = "number",
                    Placeholder = "0.00",
                    Required = false,
                    Order = 24,
                    Group = "Deposit",
                    Min = 0,
                    Step = 0.01
                },
                new FormFieldDto
                {
                    Id = "chargeDepositOnRepairs",
                    Label = "Charge a deposit on repairs",
                    Type = "switch",
                    Required = false,
                    Order = 25,
                    Group = "Deposit",
                    DefaultValue = false
                },
                // Lock Screen
                new FormFieldDto
                {
                    Id = "lockScreenTimeoutMinutes",
                    Label = "Auto-screen-off timeout (minutes)",
                    Type = "number",
                    Placeholder = "15",
                    Required = false,
                    Order = 26,
                    Group = "Lock Screen",
                    Min = 1,
                    DefaultValue = 15
                }
            }
        };
    }

    public async Task<FormConfigurationDto> GetEmployeeFormConfigurationAsync(Guid? storeId = null)
    {
        await Task.CompletedTask; // Placeholder for async

        return new FormConfigurationDto
        {
            FormId = "employee",
            Title = "Employee",
            Description = "Add or edit employee information",
            Fields = new List<FormFieldDto>
            {
                new FormFieldDto
                {
                    Id = "fullName",
                    Label = "Full Name",
                    Type = "text",
                    Placeholder = "Enter full name",
                    Required = true,
                    Order = 1,
                    MaxLength = 200
                },
                new FormFieldDto
                {
                    Id = "email",
                    Label = "Email",
                    Type = "email",
                    Placeholder = "Enter email address",
                    Required = true,
                    Order = 2,
                    Icon = "Mail"
                },
                new FormFieldDto
                {
                    Id = "phone",
                    Label = "Phone",
                    Type = "text",
                    Placeholder = "Enter phone number",
                    Required = false,
                    Order = 3,
                    Icon = "Phone",
                    MaxLength = 20
                },
                new FormFieldDto
                {
                    Id = "role",
                    Label = "Role",
                    Type = "select",
                    Required = false,
                    Order = 4,
                    Options = new List<SelectOptionDto>
                    {
                        new SelectOptionDto { Value = EmployeeRole.Owner.ToString(), Label = "Owner" },
                        new SelectOptionDto { Value = EmployeeRole.Partner.ToString(), Label = "Partner" },
                        new SelectOptionDto { Value = EmployeeRole.Admin.ToString(), Label = "Admin" },
                        new SelectOptionDto { Value = EmployeeRole.SuperAdmin.ToString(), Label = "Super Admin" },
                        new SelectOptionDto { Value = EmployeeRole.Manager.ToString(), Label = "Manager" },
                        new SelectOptionDto { Value = EmployeeRole.Technician.ToString(), Label = "Technician" },
                        new SelectOptionDto { Value = EmployeeRole.Cashier.ToString(), Label = "Cashier" }
                    }
                },
                new FormFieldDto
                {
                    Id = "employeeCode",
                    Label = "Employee Code",
                    Type = "text",
                    Placeholder = "Enter employee code",
                    Required = false,
                    Order = 5,
                    MaxLength = 50
                },
                new FormFieldDto
                {
                    Id = "hourlyRate",
                    Label = "Hourly Rate",
                    Type = "number",
                    Placeholder = "0.00",
                    Required = false,
                    Order = 6,
                    Min = 0,
                    Step = 0.01
                }
            }
        };
    }
}

