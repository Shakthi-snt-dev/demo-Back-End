using Flowtap_Application.Interfaces;
using Flowtap_Domain.BoundedContexts.Identity.Entities;
using Flowtap_Domain.BoundedContexts.Identity.Interfaces;
using Flowtap_Domain.BoundedContexts.Owner.Entities;
using Flowtap_Domain.BoundedContexts.Owner.Interfaces;
using Flowtap_Domain.BoundedContexts.Store.Entities;
using Flowtap_Domain.BoundedContexts.Store.Interfaces;
using Flowtap_Domain.BoundedContexts.Billing.Entities;
using Flowtap_Domain.BoundedContexts.Billing.Interfaces;
using Flowtap_Domain.BoundedContexts.HR.Entities;
using Flowtap_Domain.BoundedContexts.HR.Interfaces;
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
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<RegistrationService> _logger;

    public RegistrationService(
        IUserAccountRepository userAccountRepository,
        IAppUserRepository appUserRepository,
        IStoreRepository storeRepository,
        ISubscriptionRepository subscriptionRepository,
        IEmployeeRepository employeeRepository,
        IEmailService emailService,
        IConfiguration configuration,
        ILogger<RegistrationService> logger)
    {
        _userAccountRepository = userAccountRepository;
        _appUserRepository = appUserRepository;
        _storeRepository = storeRepository;
        _subscriptionRepository = subscriptionRepository;
        _employeeRepository = employeeRepository;
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
        Console.WriteLine($"[REGISTRATION] Creating UserAccount: {userAccount.Email}, ID: {userAccount.Id}");
        var saveResult = await _userAccountRepository.AddAsync(userAccount);
        Console.WriteLine($"[REGISTRATION] UserAccount saved successfully. Result ID: {saveResult.Id}");

        // Create AppUser (inactive until email verified)
        var appUser = new AppUser
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            TrialStatus = TrialStatus.NotStarted,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        Console.WriteLine($"[REGISTRATION] Creating AppUser: {appUser.Email}, ID: {appUser.Id}");
        var appUserResult = await _appUserRepository.AddAsync(appUser);
        Console.WriteLine($"[REGISTRATION] AppUser saved successfully. Result ID: {appUserResult.Id}");

        // Link UserAccount to AppUser
        userAccount.AppUserId = appUser.Id;
        await _userAccountRepository.UpdateAsync(userAccount);

        // Create a default store during registration
        var defaultStoreName = !string.IsNullOrWhiteSpace(request.Username) 
            ? $"{request.Username}'s Store" 
            : $"{request.Email.Split('@')[0]}'s Store";
        
        var defaultStore = new Store
        {
            Id = Guid.NewGuid(),
            AppUserId = appUser.Id,
            StoreName = defaultStoreName,
            StoreType = null,
            StoreCategory = null,
            Phone = null,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Set default store settings
        defaultStore.UpdateSettings(
            enablePOS: true,
            enableInventory: true,
            timeZone: "UTC");

        defaultStore.Settings.Id = Guid.NewGuid();
        defaultStore.Settings.StoreId = defaultStore.Id;
        defaultStore.Settings.CreatedAt = DateTime.UtcNow;

        await _storeRepository.AddAsync(defaultStore);
        Console.WriteLine($"[REGISTRATION] Default store created: {defaultStore.StoreName}, ID: {defaultStore.Id}");

        // Link store to app user and set as default
        appUser.AddStore(defaultStore.Id);
        appUser.SetDefaultStore(defaultStore.Id);
        await _appUserRepository.UpdateAsync(appUser);
        Console.WriteLine($"[REGISTRATION] Store linked to AppUser and set as default store");

        // Create Employee record for the AppUser as Owner (similar to onboarding step 2)
        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            StoreId = defaultStore.Id,
            FullName = string.Empty, // Will be updated during onboarding step 1
            Email = appUser.Email,
            Role = EmployeeRole.Owner.ToString(),
            LinkedAppUserId = appUser.Id,
            IsActive = true,
            CanSwitchRole = true, // Owner can switch roles
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var createdEmployee = await _employeeRepository.AddAsync(employee);
        _logger.LogInformation("Created Employee record for AppUser {AppUserId} as Owner in Store {StoreId} during registration", appUser.Id, defaultStore.Id);

        // Link Employee to UserAccount
        userAccount.EmployeeId = createdEmployee.Id;
        await _userAccountRepository.UpdateAsync(userAccount);
        _logger.LogInformation("Linked Employee {EmployeeId} to UserAccount {UserAccountId} during registration", createdEmployee.Id, userAccount.Id);

        // Send verification email
        var frontendUrl = _configuration["AppSettings:FrontendUrl"] ?? "http://localhost:5173";
        var verificationLink = $"{frontendUrl}/verification-successful?token={verificationToken}";

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
        if (string.IsNullOrWhiteSpace(token))
        {
            _logger.LogWarning("VerifyEmailAsync called with null or empty token");
            return new VerifyEmailResponseDto
            {
                IsVerified = false,
                Message = "Verification token is required."
            };
        }

        // Clean the token (remove whitespace)
        var cleanToken = token.Trim();

        _logger.LogInformation("Attempting to verify email with token (length: {Length}, first 10: {Preview})", 
            cleanToken.Length, cleanToken.Length > 10 ? cleanToken.Substring(0, 10) : cleanToken);

        var userAccount = await _userAccountRepository.GetByVerificationTokenAsync(cleanToken);
        
        if (userAccount == null)
        {
            _logger.LogWarning("No user account found with verification token (length: {Length}, first 10: {Preview}). " +
                "Token may have already been used. Checking for recently verified accounts.", 
                cleanToken.Length, cleanToken.Length > 10 ? cleanToken.Substring(0, 10) : cleanToken);
            
        
            
            _logger.LogWarning("Invalid verification token. Token may have been used, expired, or is invalid.");
            
            return new VerifyEmailResponseDto
            {
                IsVerified = false,
                Message = "Invalid or expired verification token. This link may have already been used or has expired. If you've already verified your email, please proceed to login."
            };
        }

        _logger.LogInformation("User account found: {Email}, IsEmailVerified: {IsVerified}, TokenExpiry: {Expiry}", 
            userAccount.Email, userAccount.IsEmailVerified, userAccount.EmailVerificationTokenExpiry);

        // Check if email is already verified - but only if token matches
        // This means the user clicked the link again after verification
        if (userAccount.IsEmailVerified)
        {
            _logger.LogInformation("Email {Email} is already verified. Token matches, so this is a re-verification attempt.", 
                userAccount.Email);
            
            return new VerifyEmailResponseDto
            {
                IsVerified = true,
                Message = "Verification success! Your email is already verified. Please go to login to access your account.",
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

        // Ensure AppUserId is set - if not, try to get it from the linked AppUser
        Guid? appUserId = userAccount.AppUserId;
        if (!appUserId.HasValue)
        {
            // Try to find AppUser by email as fallback
            var appUser = await _appUserRepository.GetByEmailAsync(userAccount.Email);
            if (appUser != null)
            {
                appUserId = appUser.Id;
                // Update UserAccount with AppUserId if it was missing
                userAccount.AppUserId = appUser.Id;
                await _userAccountRepository.UpdateAsync(userAccount);
                _logger.LogInformation("Linked AppUserId {AppUserId} to UserAccount {UserAccountId} during verification", appUser.Id, userAccount.Id);
            }
            else
            {
                _logger.LogWarning("AppUserId is null for UserAccount {UserAccountId} and no AppUser found with email {Email}", userAccount.Id, userAccount.Email);
            }
        }

        // Send welcome email
        await _emailService.SendWelcomeEmailAsync(userAccount.Email, userAccount.Username ?? userAccount.Email);

        _logger.LogInformation("Email {Email} verified successfully for the first time.", userAccount.Email);

        return new VerifyEmailResponseDto
        {
            IsVerified = true,
            Message = "Verification success! Your email has been verified and your account is now active. Please go to login to access your account.",
            AppUserId = appUserId,
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

        // Create Employee record for the AppUser as Owner
        var userAccount = await _userAccountRepository.GetByEmailAsync(appUser.Email);
        if (userAccount != null)
        {
            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                StoreId = store.Id,
                FullName = $"{appUser.FirstName} {appUser.LastName}".Trim(),
                Email = appUser.Email,
                Role = EmployeeRole.Owner.ToString(), // Set as Owner role
                LinkedAppUserId = appUser.Id,
                IsActive = true,
                CanSwitchRole = true, // Owner can switch roles
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Set employee address if available
            if (appUser.Address != null)
            {
                employee.Address = appUser.Address;
            }

            var createdEmployee = await _employeeRepository.AddAsync(employee);
            
            // Link Employee to UserAccount
            userAccount.EmployeeId = createdEmployee.Id;
            await _userAccountRepository.UpdateAsync(userAccount);
            
            _logger.LogInformation("Created Employee record for AppUser {AppUserId} as Owner in Store {StoreId} and linked to UserAccount {UserAccountId}", appUser.Id, store.Id, userAccount.Id);
        }

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

