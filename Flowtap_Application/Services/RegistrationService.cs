using Flowtap_Application.Interfaces;
using Flowtap_Domain.BoundedContexts.Identity.Entities;
using Flowtap_Domain.BoundedContexts.Identity.Interfaces;
using Flowtap_Domain.BoundedContexts.Owner.Entities;
using Flowtap_Domain.BoundedContexts.Owner.Interfaces;
using Flowtap_Domain.BoundedContexts.Store.Entities;
using Flowtap_Domain.BoundedContexts.Store.Interfaces;
using Flowtap_Domain.BoundedContexts.Billing.Entities;
using Flowtap_Domain.BoundedContexts.Billing.Interfaces;
using Flowtap_Domain.DtoModel;
using Flowtap_Domain.Exceptions;
using Flowtap_Domain.SharedKernel.Enums;
using Flowtap_Domain.SharedKernel.ValueObjects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Flowtap_Application.Services;

public class RegistrationService : IRegistrationService
{
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly IAppUserRepository _appUserRepository;
    private readonly IStoreRepository _storeRepository;
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<RegistrationService> _logger;

    public RegistrationService(
        IUserAccountRepository userAccountRepository,
        IAppUserRepository appUserRepository,
        IStoreRepository storeRepository,
        ISubscriptionRepository subscriptionRepository,
        IEmailService emailService,
        IConfiguration configuration,
        ILogger<RegistrationService> logger)
    {
        _userAccountRepository = userAccountRepository;
        _appUserRepository = appUserRepository;
        _storeRepository = storeRepository;
        _subscriptionRepository = subscriptionRepository;
        _emailService = emailService;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        // Check if email already exists
        var existingUser = await _userAccountRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new UserAlreadyExistException(request.Email, request.Username ?? string.Empty);
        }

        // Check if username is provided and unique
        if (!string.IsNullOrWhiteSpace(request.Username))
        {
            var usernameExists = await _userAccountRepository.UsernameExistsAsync(request.Username);
            if (usernameExists)
            {
                throw new UserAlreadyExistException(request.Email, request.Username);
            }
        }

        // Hash password
        var (passwordHash, passwordSalt) = HashPassword(request.Password);

        // Generate verification token
        var verificationToken = GenerateVerificationToken();
        var tokenExpiry = DateTime.UtcNow.AddHours(24);

        // Create UserAccount
        var userAccount = new UserAccount
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            Username = request.Username,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            UserType = UserType.AppUser,
            IsActive = true,
            IsEmailVerified = false,
            EmailVerificationToken = verificationToken,
            EmailVerificationTokenExpiry = tokenExpiry,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _userAccountRepository.AddAsync(userAccount);

        // Create AppUser (inactive until email verified)
        var appUser = new AppUser
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            TrialStatus = TrialStatus.NotStarted,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _appUserRepository.AddAsync(appUser);

        // Link UserAccount to AppUser
        userAccount.AppUserId = appUser.Id;
        await _userAccountRepository.UpdateAsync(userAccount);

        // Send verification email
        var baseUrl = _configuration["AppSettings:BaseUrl"] ?? "http://localhost:5000";
        var verificationLink = $"{baseUrl}/api/auth/verify-email?token={verificationToken}";

        var emailSent = await _emailService.SendVerificationEmailAsync(
            request.Email,
            verificationLink,
            request.Username ?? request.Email);

        return new RegisterResponseDto
        {
            UserId = userAccount.Id,
            Email = userAccount.Email,
            Username = userAccount.Username,
            EmailSent = emailSent,
            Message = emailSent
                ? "Registration successful. Please check your email to verify your account."
                : "Registration successful, but email could not be sent. Please contact support."
        };
    }

    public async Task<VerifyEmailResponseDto> VerifyEmailAsync(string token)
    {
        var userAccount = await _userAccountRepository.GetByVerificationTokenAsync(token);
        
        if (userAccount == null)
        {
            return new VerifyEmailResponseDto
            {
                IsVerified = false,
                Message = "Invalid verification token."
            };
        }

        if (userAccount.IsEmailVerified)
        {
            return new VerifyEmailResponseDto
            {
                IsVerified = true,
                Message = "Email is already verified.",
                AppUserId = userAccount.AppUserId,
                OnboardingStep = 1
            };
        }

        if (userAccount.EmailVerificationTokenExpiry.HasValue && 
            userAccount.EmailVerificationTokenExpiry.Value < DateTime.UtcNow)
        {
            return new VerifyEmailResponseDto
            {
                IsVerified = false,
                Message = "Verification token has expired. Please request a new one."
            };
        }

        // Verify email using domain method
        userAccount.VerifyEmail();
        await _userAccountRepository.UpdateAsync(userAccount);

        // Send welcome email
        await _emailService.SendWelcomeEmailAsync(userAccount.Email, userAccount.Username ?? userAccount.Email);

        return new VerifyEmailResponseDto
        {
            IsVerified = true,
            Message = "Email verified successfully. You can now complete your profile setup.",
            AppUserId = userAccount.AppUserId,
            OnboardingStep = 1
        };
    }

    public async Task<OnboardingResponseDto> CompleteOnboardingStep1Async(Guid appUserId, OnboardingStep1RequestDto request)
    {
        var appUser = await _appUserRepository.GetByIdAsync(appUserId);
        if (appUser == null)
        {
            throw new EntityNotFoundException("AppUser", appUserId);
        }

        // Update profile information
        appUser.FirstName = request.FirstName;
        appUser.LastName = request.LastName;
        appUser.PhoneNumber = request.PhoneNumber;
        appUser.BusinessName = request.BusinessName;
        appUser.Country = request.Country;
        appUser.Currency = request.Currency;
        appUser.TimeZone = request.TimeZone;

        // Update address
        var address = new Address(
            request.StreetNumber,
            request.StreetName,
            request.City,
            request.State,
            request.PostalCode);
        appUser.UpdateAddress(address);

        await _appUserRepository.UpdateAsync(appUser);

        return new OnboardingResponseDto
        {
            Success = true,
            Message = "Profile settings saved successfully.",
            CurrentStep = 2
        };
    }

    public async Task<OnboardingResponseDto> CompleteOnboardingStep2Async(Guid appUserId, OnboardingStep2RequestDto request)
    {
        var appUser = await _appUserRepository.GetByIdAsync(appUserId);
        if (appUser == null)
        {
            throw new EntityNotFoundException("AppUser", appUserId);
        }

        // Create store
        var store = new Store
        {
            Id = Guid.NewGuid(),
            AppUserId = appUserId,
            StoreName = request.StoreName,
            StoreType = request.StoreType,
            StoreCategory = request.StoreCategory,
            Phone = request.Phone,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Set store address
        var address = new Address(
            request.StreetNumber,
            request.StreetName,
            request.City,
            request.State,
            request.PostalCode);
        store.UpdateAddress(address);

        // Set store settings
        store.UpdateSettings(
            request.EnablePOS,
            request.EnableInventory,
            request.TimeZone);

        store.Settings.Id = Guid.NewGuid();
        store.Settings.StoreId = store.Id;
        store.Settings.CreatedAt = DateTime.UtcNow;

        await _storeRepository.AddAsync(store);

        // Link store to app user
        appUser.AddStore(store.Id);
        await _appUserRepository.UpdateAsync(appUser);

        return new OnboardingResponseDto
        {
            Success = true,
            Message = "Store settings saved successfully.",
            CurrentStep = 3,
            StoreId = store.Id
        };
    }

    public async Task<OnboardingResponseDto> CompleteOnboardingStep3Async(Guid appUserId, OnboardingStep3RequestDto request)
    {
        var appUser = await _appUserRepository.GetByIdAsync(appUserId);
        if (appUser == null)
        {
            throw new EntityNotFoundException("AppUser", appUserId);
        }

        // Start 30-day trial
        appUser.StartTrial(30);

        await _appUserRepository.UpdateAsync(appUser);

        var message = request.Action.ToLower() == "dive_in"
            ? "Welcome! Your 30-day trial has started. Dive in and explore!"
            : "Welcome! Your 30-day trial has started. Check out our demo to get started!";

        return new OnboardingResponseDto
        {
            Success = true,
            Message = message,
            CurrentStep = 0, // Onboarding complete
            TrialStarted = true,
            TrialEndDate = appUser.TrialEndDate
        };
    }

    private (string hash, string salt) HashPassword(string password)
    {
        using var rng = RandomNumberGenerator.Create();
        var saltBytes = new byte[16];
        rng.GetBytes(saltBytes);
        var salt = Convert.ToBase64String(saltBytes);

        using var sha256 = SHA256.Create();
        var passwordBytes = Encoding.UTF8.GetBytes(password + salt);
        var hashBytes = sha256.ComputeHash(passwordBytes);
        var hash = Convert.ToBase64String(hashBytes);

        return (hash, salt);
    }

    private string GenerateVerificationToken()
    {
        using var rng = RandomNumberGenerator.Create();
        var tokenBytes = new byte[32];
        rng.GetBytes(tokenBytes);
        return Convert.ToBase64String(tokenBytes)
            .Replace("+", "-")
            .Replace("/", "_")
            .Replace("=", "");
    }

    public async Task<UpgradeToSubscriptionResponseDto> UpgradeToSubscriptionAsync(
        Guid appUserId,
        UpgradeToSubscriptionRequestDto request)
    {
        var appUser = await _appUserRepository.GetByIdAsync(appUserId);
        if (appUser == null)
        {
            throw new EntityNotFoundException("AppUser", appUserId);
        }

        // Check if user already has an active subscription
        if (appUser.SubscriptionId.HasValue)
        {
            var existingSubscription = await _subscriptionRepository.GetByIdAsync(appUser.SubscriptionId.Value);
            if (existingSubscription != null && existingSubscription.IsActive())
            {
                throw new Flowtap_Domain.Exceptions.InvalidOperationException(
                    "User already has an active subscription",
                    "Subscription",
                    new Dictionary<string, string> { { "AppUserId", appUserId.ToString() } });
            }
        }

        // Parse billing interval
        var billingInterval = request.BillingInterval.ToLower() == "yearly"
            ? BillingInterval.Yearly
            : BillingInterval.Monthly;

        // Calculate end date based on billing interval
        var startDate = DateTime.UtcNow;
        var endDate = billingInterval == BillingInterval.Yearly
            ? startDate.AddYears(1)
            : startDate.AddMonths(1);

        // Create subscription
        var subscription = new Subscription
        {
            Id = Guid.NewGuid(),
            AppUserId = appUserId,
            PlanName = request.PlanName,
            MaxStores = request.MaxStores,
            PricePerMonth = request.PricePerMonth,
            StartDate = startDate,
            EndDate = endDate,
            Status = SubscriptionStatus.Active,
            BillingInterval = billingInterval,
            PaymentProvider = request.PaymentProvider,
            ExternalSubscriptionId = request.ExternalSubscriptionId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _subscriptionRepository.AddAsync(subscription);

        // Convert trial to paid subscription
        appUser.ConvertTrialToPaid(subscription.Id);
        await _appUserRepository.UpdateAsync(appUser);

        return new UpgradeToSubscriptionResponseDto
        {
            Success = true,
            Message = "Successfully upgraded to subscription plan.",
            SubscriptionId = subscription.Id,
            StartDate = subscription.StartDate,
            EndDate = subscription.EndDate,
            PlanName = subscription.PlanName
        };
    }
}

