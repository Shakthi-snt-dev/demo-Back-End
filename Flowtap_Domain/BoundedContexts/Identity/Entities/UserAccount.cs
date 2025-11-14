using System.ComponentModel.DataAnnotations;
using Flowtap_Domain.SharedKernel.Enums;

namespace Flowtap_Domain.BoundedContexts.Identity.Entities;

public class UserAccount
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(320)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Username { get; set; }

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    public string? PasswordSalt { get; set; }

    [Required]
    public UserType UserType { get; set; } = UserType.AppUser;

    public Guid? AppUserId { get; set; }

    public Guid? EmployeeId { get; set; }

    public bool IsActive { get; set; } = true;

    public bool IsEmailVerified { get; set; } = false;

    public string? EmailVerificationToken { get; set; }

    public DateTime? EmailVerificationTokenExpiry { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public DateTime? LastLoginAt { get; set; }

    public string? ExternalProvider { get; set; }

    public string? ExternalProviderId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Activates the user account
    /// </summary>
    public void Activate()
    {
        if (IsActive)
            throw new InvalidOperationException("Account is already active");

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Deactivates the user account
    /// </summary>
    public void Deactivate()
    {
        if (!IsActive)
            throw new InvalidOperationException("Account is already inactive");

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the last login timestamp
    /// </summary>
    public void UpdateLastLogin()
    {
        LastLoginAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Enables two-factor authentication
    /// </summary>
    public void EnableTwoFactor()
    {
        if (TwoFactorEnabled)
            throw new InvalidOperationException("Two-factor authentication is already enabled");

        TwoFactorEnabled = true;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Disables two-factor authentication
    /// </summary>
    public void DisableTwoFactor()
    {
        if (!TwoFactorEnabled)
            throw new InvalidOperationException("Two-factor authentication is already disabled");

        TwoFactorEnabled = false;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the password hash
    /// </summary>
    /// <param name="passwordHash">The new password hash</param>
    /// <param name="passwordSalt">The new password salt</param>
    public void UpdatePassword(string passwordHash, string? passwordSalt = null)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash cannot be empty", nameof(passwordHash));

        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Links the account to an external provider
    /// </summary>
    /// <param name="provider">The provider name</param>
    /// <param name="providerId">The provider user ID</param>
    public void LinkExternalProvider(string provider, string providerId)
    {
        if (string.IsNullOrWhiteSpace(provider))
            throw new ArgumentException("Provider cannot be empty", nameof(provider));

        if (string.IsNullOrWhiteSpace(providerId))
            throw new ArgumentException("Provider ID cannot be empty", nameof(providerId));

        ExternalProvider = provider;
        ExternalProviderId = providerId;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Unlinks the account from external provider
    /// </summary>
    public void UnlinkExternalProvider()
    {
        if (string.IsNullOrWhiteSpace(ExternalProvider))
            throw new InvalidOperationException("Account is not linked to any external provider");

        ExternalProvider = null;
        ExternalProviderId = null;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Checks if the account is linked to an external provider
    /// </summary>
    public bool IsLinkedToExternalProvider()
    {
        return !string.IsNullOrWhiteSpace(ExternalProvider) && !string.IsNullOrWhiteSpace(ExternalProviderId);
    }

    /// <summary>
    /// Checks if the account can login (is active)
    /// </summary>
    public bool CanLogin()
    {
        return IsActive;
    }

    /// <summary>
    /// Verifies the email address
    /// </summary>
    public void VerifyEmail()
    {
        if (IsEmailVerified)
            throw new InvalidOperationException("Email is already verified");

        IsEmailVerified = true;
        EmailVerificationToken = null;
        EmailVerificationTokenExpiry = null;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Generates a new email verification token
    /// </summary>
    /// <param name="token">The verification token</param>
    /// <param name="expiryHours">Hours until token expires (default 24)</param>
    public void SetVerificationToken(string token, int expiryHours = 24)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("Token cannot be empty", nameof(token));

        EmailVerificationToken = token;
        EmailVerificationTokenExpiry = DateTime.UtcNow.AddHours(expiryHours);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Checks if the verification token is valid
    /// </summary>
    /// <param name="token">The token to validate</param>
    public bool IsVerificationTokenValid(string token)
    {
        if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(EmailVerificationToken))
            return false;

        if (EmailVerificationToken != token)
            return false;

        if (EmailVerificationTokenExpiry.HasValue && EmailVerificationTokenExpiry.Value < DateTime.UtcNow)
            return false;

        return true;
    }
}

