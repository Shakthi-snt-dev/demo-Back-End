using AutoMapper;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.BoundedContexts.Owner.Interfaces;
using Flowtap_Domain.BoundedContexts.Store.Interfaces;
using Flowtap_Domain.BoundedContexts.Identity.Interfaces;
using Flowtap_Application.DtoModel;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;
using Flowtap_Domain.Exceptions;
using Flowtap_Domain.SharedKernel.ValueObjects;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Security.Cryptography;
using System;

namespace Flowtap_Application.Services;

/// <summary>
/// Store Settings Service - Manage store settings
/// </summary>
public class StoreSettingsService : IStoreSettingsService
{
    private readonly IAppUserRepository _appUserRepository;
    private readonly IStoreRepository _storeRepository;
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly IEmailService _emailService;
    private readonly IMapper _mapper;
    private readonly ILogger<StoreSettingsService> _logger;

    public StoreSettingsService(
        IAppUserRepository appUserRepository,
        IStoreRepository storeRepository,
        IUserAccountRepository userAccountRepository,
        IEmailService emailService,
        IMapper mapper,
        ILogger<StoreSettingsService> logger)
    {
        _appUserRepository = appUserRepository;
        _storeRepository = storeRepository;
        _userAccountRepository = userAccountRepository;
        _emailService = emailService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<StoreSettingsDto> GetStoreSettingsAsync(Guid storeId)
    {
        var store = await _storeRepository.GetByIdAsync(storeId);
        if (store == null)
            throw new EntityNotFoundException("Store", storeId);

        // Ensure StoreSettings exists
        if (store.Settings == null)
        {
            store.Settings = new Flowtap_Domain.BoundedContexts.Store.Entities.StoreSettings
            {
                Id = Guid.NewGuid(),
                StoreId = store.Id,
                CreatedAt = DateTime.UtcNow
            };
            await _storeRepository.UpdateAsync(store);
        }

        // Map Store to StoreSettingsDto using AutoMapper
        var settingsDto = _mapper.Map<StoreSettingsDto>(store);
        
        // Handle special cases that need manual mapping
        settingsDto.Address = store.Address ?? CreateEmptyAddress();
        
        // Mask API Key for security (show only last 8 characters)
        if (!string.IsNullOrEmpty(store.Settings.ApiKey))
        {
            settingsDto.ApiKey = $"****{store.Settings.ApiKey.Substring(Math.Max(0, store.Settings.ApiKey.Length - 8))}";
        }
        
        // Ensure LockScreenTimeoutMinutes has a default value
        if (settingsDto.LockScreenTimeoutMinutes <= 0)
        {
            settingsDto.LockScreenTimeoutMinutes = 15;
        }

        return settingsDto;
    }

    public async Task<StoreSettingsDto> UpdateStoreSettingsAsync(Guid storeId, UpdateStoreSettingsRequestDto request)
    {
        var store = await _storeRepository.GetByIdAsync(storeId);
        if (store == null)
            throw new EntityNotFoundException("Store", storeId);

        // Ensure StoreSettings exists
        if (store.Settings == null)
        {
            store.Settings = new Flowtap_Domain.BoundedContexts.Store.Entities.StoreSettings
            {
                Id = Guid.NewGuid(),
                StoreId = store.Id,
                CreatedAt = DateTime.UtcNow
            };
        }

        var settings = store.Settings;

        // Update Basic Information
        if (!string.IsNullOrWhiteSpace(request.BusinessName))
            store.StoreName = request.BusinessName;

        if (!string.IsNullOrWhiteSpace(request.StoreEmail))
            settings.StoreEmail = request.StoreEmail;

        if (request.AlternateName != null)
            settings.AlternateName = request.AlternateName;

        if (request.StoreLogoUrl != null)
            settings.StoreLogoUrl = request.StoreLogoUrl;

        // Update Contact Information
        if (!string.IsNullOrWhiteSpace(request.Phone))
            store.Phone = request.Phone;

        if (request.Mobile != null)
            settings.Mobile = request.Mobile;

        if (request.Website != null)
            settings.Website = request.Website;

        if (request.Address != null)
            store.UpdateAddress(request.Address);

        // Update Other Information
        if (!string.IsNullOrWhiteSpace(request.TimeZone))
        {
            var enablePOS = store.Settings?.EnablePOS ?? true;
            var enableInventory = store.Settings?.EnableInventory ?? true;
            store.UpdateSettings(enablePOS, enableInventory, request.TimeZone);
        }

        if (!string.IsNullOrWhiteSpace(request.TimeFormat))
            settings.TimeFormat = request.TimeFormat;

        if (!string.IsNullOrWhiteSpace(request.Language))
            settings.Language = request.Language;

        if (!string.IsNullOrWhiteSpace(request.DefaultCurrency))
            settings.DefaultCurrency = request.DefaultCurrency;

        if (!string.IsNullOrWhiteSpace(request.PriceFormat))
            settings.PriceFormat = request.PriceFormat;

        if (!string.IsNullOrWhiteSpace(request.DecimalFormat))
            settings.DecimalFormat = request.DecimalFormat;

        // Update Sales Tax
        if (request.ChargeSalesTax.HasValue)
            settings.ChargeSalesTax = request.ChargeSalesTax.Value;

        if (request.DefaultTaxClass != null)
            settings.DefaultTaxClass = request.DefaultTaxClass;

        if (request.TaxPercentage.HasValue)
            settings.TaxPercentage = request.TaxPercentage.Value;

        // Update Business Information
        if (request.RegistrationNumber != null)
            settings.RegistrationNumber = request.RegistrationNumber;

        if (request.StartTime != null)
            settings.StartTime = request.StartTime;

        if (request.EndTime != null)
            settings.EndTime = request.EndTime;

        if (request.DefaultAddress != null)
            settings.DefaultAddress = request.DefaultAddress;

        // Update Accounting Method
        if (!string.IsNullOrWhiteSpace(request.AccountingMethod))
        {
            if (request.AccountingMethod != "Cash Basis" && request.AccountingMethod != "Accrual Basis")
                throw new ArgumentException("Accounting method must be either 'Cash Basis' or 'Accrual Basis'");
            settings.AccountingMethod = request.AccountingMethod;
        }

        // Update Company Email
        if (!string.IsNullOrWhiteSpace(request.CompanyEmail))
        {
            settings.CompanyEmail = request.CompanyEmail;
            // Reset verification status when email is changed
            settings.CompanyEmailVerified = false;
        }

        // Update Email Notifications
        if (request.EmailNotifications.HasValue)
            settings.EmailNotifications = request.EmailNotifications.Value;

        // Update Security (2FA)
        if (request.RequireTwoFactorForAllUsers.HasValue)
            settings.RequireTwoFactorForAllUsers = request.RequireTwoFactorForAllUsers.Value;

        // Update Restocking Fee
        if (request.ChargeRestockingFee.HasValue)
            settings.ChargeRestockingFee = request.ChargeRestockingFee.Value;

        // Update Deposit
        if (request.DiagnosticBenchFee.HasValue)
            settings.DiagnosticBenchFee = request.DiagnosticBenchFee.Value;

        if (request.ChargeDepositOnRepairs.HasValue)
            settings.ChargeDepositOnRepairs = request.ChargeDepositOnRepairs.Value;

        // Update Lock Screen
        if (request.LockScreenTimeoutMinutes.HasValue)
        {
            if (request.LockScreenTimeoutMinutes.Value < 1 || request.LockScreenTimeoutMinutes.Value > 1440)
                throw new ArgumentException("Lock screen timeout must be between 1 and 1440 minutes");
            settings.LockScreenTimeoutMinutes = request.LockScreenTimeoutMinutes.Value;
        }

        settings.UpdatedAt = DateTime.UtcNow;
        store.UpdatedAt = DateTime.UtcNow;

        await _storeRepository.UpdateAsync(store);

        return await GetStoreSettingsAsync(storeId);
    }

    public async Task<string> ResetApiKeyAsync(Guid storeId)
    {
        var store = await _storeRepository.GetByIdAsync(storeId);
        if (store == null)
            throw new EntityNotFoundException("Store", storeId);

        // Ensure StoreSettings exists
        if (store.Settings == null)
        {
            store.Settings = new Flowtap_Domain.BoundedContexts.Store.Entities.StoreSettings
            {
                Id = Guid.NewGuid(),
                StoreId = store.Id,
                CreatedAt = DateTime.UtcNow
            };
        }

        // Generate new API key (using secure random bytes)
        var apiKeyBytes = new byte[32];
        System.Security.Cryptography.RandomNumberGenerator.Fill(apiKeyBytes);
        var newApiKey = Convert.ToBase64String(apiKeyBytes)
            .Replace("+", "-")
            .Replace("/", "_")
            .Replace("=", "");

        store.Settings.ApiKey = newApiKey;
        store.Settings.ApiKeyCreatedAt = DateTime.UtcNow;
        store.Settings.UpdatedAt = DateTime.UtcNow;
        store.UpdatedAt = DateTime.UtcNow;

        await _storeRepository.UpdateAsync(store);

        _logger.LogInformation("API key reset for store {StoreId}", storeId);

        // Return the full key (only on reset, not on GET)
        return newApiKey;
    }

    public async Task<StoreSettingsDto> GetStoreSettingsByUserAccountIdAsync(Guid userAccountId, Guid storeId)
    {
        // Get UserAccount to find AppUserId
        var userAccount = await _userAccountRepository.GetByIdAsync(userAccountId);
        if (userAccount == null)
            throw new EntityNotFoundException("UserAccount", userAccountId);

        if (!userAccount.AppUserId.HasValue)
            throw new UnauthorizedException("User is not associated with an AppUser account");

        // Validate store belongs to AppUser
        var store = await _storeRepository.GetByIdAsync(storeId);
        if (store == null)
            throw new EntityNotFoundException("Store", storeId);

        if (store.AppUserId != userAccount.AppUserId.Value)
            throw new UnauthorizedException("Store does not belong to the authenticated user");

        return await GetStoreSettingsAsync(storeId);
    }

    public async Task<StoreSettingsDto> UpdateStoreSettingsByUserAccountIdAsync(Guid userAccountId, Guid storeId, UpdateStoreSettingsRequestDto request)
    {
        // Get UserAccount to find AppUserId
        var userAccount = await _userAccountRepository.GetByIdAsync(userAccountId);
        if (userAccount == null)
            throw new EntityNotFoundException("UserAccount", userAccountId);

        if (!userAccount.AppUserId.HasValue)
            throw new UnauthorizedException("User is not associated with an AppUser account");

        // Validate store belongs to AppUser
        var store = await _storeRepository.GetByIdAsync(storeId);
        if (store == null)
            throw new EntityNotFoundException("Store", storeId);

        if (store.AppUserId != userAccount.AppUserId.Value)
            throw new UnauthorizedException("Store does not belong to the authenticated user");

        return await UpdateStoreSettingsAsync(storeId, request);
    }

    public async Task<string> ResetApiKeyByUserAccountIdAsync(Guid userAccountId, Guid storeId)
    {
        // Get UserAccount to find AppUserId
        var userAccount = await _userAccountRepository.GetByIdAsync(userAccountId);
        if (userAccount == null)
            throw new EntityNotFoundException("UserAccount", userAccountId);

        if (!userAccount.AppUserId.HasValue)
            throw new UnauthorizedException("User is not associated with an AppUser account");

        // Validate store belongs to AppUser
        var store = await _storeRepository.GetByIdAsync(storeId);
        if (store == null)
            throw new EntityNotFoundException("Store", storeId);

        if (store.AppUserId != userAccount.AppUserId.Value)
            throw new UnauthorizedException("Store does not belong to the authenticated user");

        return await ResetApiKeyAsync(storeId);
    }



    public async Task<bool> VerifyCompanyEmailAsync(Guid userAccountId, Guid storeId, string verificationToken)
    {
        // Get UserAccount to find AppUserId
        var userAccount = await _userAccountRepository.GetByIdAsync(userAccountId);
        if (userAccount == null)
            throw new EntityNotFoundException("UserAccount", userAccountId);

        if (!userAccount.AppUserId.HasValue)
            throw new UnauthorizedException("User is not associated with an AppUser account");

        // Validate store belongs to AppUser
        var store = await _storeRepository.GetByIdAsync(storeId);
        if (store == null)
            throw new EntityNotFoundException("Store", storeId);

        if (store.AppUserId != userAccount.AppUserId.Value)
            throw new UnauthorizedException("Store does not belong to the authenticated user");

        // Ensure StoreSettings exists
        if (store.Settings == null)
            throw new EntityNotFoundException("StoreSettings", storeId);

        // For now, we'll just verify if token matches and mark as verified
        // In a real implementation, you'd store and validate the token properly
        if (string.IsNullOrWhiteSpace(verificationToken))
            return false;

        // Mark company email as verified
        store.Settings.CompanyEmailVerified = true;
        store.Settings.UpdatedAt = DateTime.UtcNow;
        store.UpdatedAt = DateTime.UtcNow;

        await _storeRepository.UpdateAsync(store);

        return true;
    }

    public async Task<bool> SendCompanyEmailVerificationAsync(Guid userAccountId, Guid storeId)
    {
        // Get UserAccount to find AppUserId
        var userAccount = await _userAccountRepository.GetByIdAsync(userAccountId);
        if (userAccount == null)
            throw new EntityNotFoundException("UserAccount", userAccountId);

        if (!userAccount.AppUserId.HasValue)
            throw new UnauthorizedException("User is not associated with an AppUser account");

        // Validate store belongs to AppUser
        var store = await _storeRepository.GetByIdAsync(storeId);
        if (store == null)
            throw new EntityNotFoundException("Store", storeId);

        if (store.AppUserId != userAccount.AppUserId.Value)
            throw new UnauthorizedException("Store does not belong to the authenticated user");

        // Ensure StoreSettings exists
        if (store.Settings == null)
        {
            store.Settings = new Flowtap_Domain.BoundedContexts.Store.Entities.StoreSettings
            {
                Id = Guid.NewGuid(),
                StoreId = store.Id,
                CreatedAt = DateTime.UtcNow
            };
            await _storeRepository.UpdateAsync(store);
        }

        if (string.IsNullOrWhiteSpace(store.Settings.CompanyEmail))
            throw new ArgumentException("Company email is not set");

        // Generate verification token
        var verificationToken = Guid.NewGuid().ToString("N");
        // In a real implementation, you'd store this token with expiry
        
        // Get frontend URL from configuration
        var frontendUrl = "http://localhost:5173"; // Default, should come from config
        var verificationLink = $"{frontendUrl}/settings/stores/{storeId}/verify-email?token={verificationToken}";

        // Send verification email
        var emailSent = await _emailService.SendVerificationEmailAsync(
            store.Settings.CompanyEmail,
            verificationLink,
            store.StoreName);

        if (emailSent)
        {
            // Reset verification status when new email is sent
            store.Settings.CompanyEmailVerified = false;
            store.Settings.UpdatedAt = DateTime.UtcNow;
            store.UpdatedAt = DateTime.UtcNow;
            await _storeRepository.UpdateAsync(store);
        }

        return emailSent;
    }

    /// <summary>
    /// Creates an empty Address instance using reflection to access the private parameterless constructor
    /// </summary>
    private Address CreateEmptyAddress()
    {
        var constructor = typeof(Address).GetConstructor(
            BindingFlags.Instance | BindingFlags.NonPublic,
            null,
            Type.EmptyTypes,
            null);

        if (constructor == null)
            throw new System.InvalidOperationException("Unable to create empty Address instance");

        return (Address)constructor.Invoke(null);
    }
}

