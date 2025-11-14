using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Flowtap_Domain.SharedKernel.Enums;
using Flowtap_Domain.SharedKernel.ValueObjects;

namespace Flowtap_Domain.BoundedContexts.Owner.Entities;

public class AppUser
{
    [Key]
    public Guid Id { get; set; }

    // Personal
    [Required, MaxLength(320)]
    public string Email { get; set; } = string.Empty;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? PhoneNumber { get; set; }

    // Address
    public Address? Address { get; set; }

    // Business
    public string? BusinessName { get; set; }

    public string? Country { get; set; }

    public string Currency { get; set; } = "USD";

    public string TimeZone { get; set; } = "UTC";

    // Subscription
    public Guid? SubscriptionId { get; set; }

    public DateTime? TrialStartDate { get; set; }

    public DateTime? TrialEndDate { get; set; }

    public TrialStatus TrialStatus { get; set; } = TrialStatus.NotStarted;

    [NotMapped]
    public bool IsTrialActive => TrialEndDate.HasValue && DateTime.UtcNow <= TrialEndDate.Value;

    // Co-owners
    public ICollection<AppUserAdmin> Admins { get; set; } = new List<AppUserAdmin>();

    // Stores (IDs only to avoid cross-context coupling)
    public ICollection<Guid> StoreIds { get; set; } = new List<Guid>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Starts a trial period for the user
    /// </summary>
    /// <param name="trialDurationDays">Duration of trial in days</param>
    /// <exception cref="InvalidOperationException">Thrown when trial cannot be started</exception>
    public void StartTrial(int trialDurationDays = 14)
    {
        if (TrialStatus != TrialStatus.NotStarted)
            throw new InvalidOperationException($"Cannot start trial. Current status: {TrialStatus}");

        if (trialDurationDays <= 0)
            throw new ArgumentException("Trial duration must be greater than 0", nameof(trialDurationDays));

        TrialStatus = TrialStatus.Active;
        TrialStartDate = DateTime.UtcNow;
        TrialEndDate = DateTime.UtcNow.AddDays(trialDurationDays);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Ends the trial period
    /// </summary>
    public void EndTrial()
    {
        if (TrialStatus != TrialStatus.Active)
            throw new InvalidOperationException($"Cannot end trial. Current status: {TrialStatus}");

        TrialStatus = TrialStatus.Ended;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Converts trial to paid subscription
    /// </summary>
    /// <param name="subscriptionId">The subscription ID to link</param>
    public void ConvertTrialToPaid(Guid subscriptionId)
    {
        if (TrialStatus != TrialStatus.Active && TrialStatus != TrialStatus.Ended)
            throw new InvalidOperationException($"Cannot convert trial. Current status: {TrialStatus}");

        TrialStatus = TrialStatus.ConvertedToPaid;
        SubscriptionId = subscriptionId;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Adds a co-owner/admin to the account
    /// </summary>
    /// <param name="admin">The admin to add</param>
    public void AddAdmin(AppUserAdmin admin)
    {
        if (admin == null)
            throw new ArgumentNullException(nameof(admin));

        if (Admins.Any(a => a.Email == admin.Email))
            throw new InvalidOperationException($"Admin with email {admin.Email} already exists");

        admin.AppUserId = Id;
        Admins.Add(admin);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Removes a co-owner/admin from the account
    /// </summary>
    /// <param name="adminId">The admin ID to remove</param>
    public void RemoveAdmin(Guid adminId)
    {
        var admin = Admins.FirstOrDefault(a => a.Id == adminId);
        if (admin == null)
            throw new InvalidOperationException($"Admin with ID {adminId} not found");

        if (admin.IsPrimaryOwner)
            throw new InvalidOperationException("Cannot remove primary owner");

        Admins.Remove(admin);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Links a store to this user
    /// </summary>
    /// <param name="storeId">The store ID to link</param>
    public void AddStore(Guid storeId)
    {
        if (StoreIds.Contains(storeId))
            throw new InvalidOperationException($"Store {storeId} is already linked to this user");

        StoreIds.Add(storeId);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Unlinks a store from this user
    /// </summary>
    /// <param name="storeId">The store ID to unlink</param>
    public void RemoveStore(Guid storeId)
    {
        if (!StoreIds.Contains(storeId))
            throw new InvalidOperationException($"Store {storeId} is not linked to this user");

        StoreIds.Remove(storeId);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the user's address
    /// </summary>
    /// <param name="address">The new address</param>
    public void UpdateAddress(Address address)
    {
        if (address == null)
            throw new ArgumentNullException(nameof(address));

        Address = address;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Checks if the user can create more stores based on subscription limits
    /// </summary>
    /// <param name="maxStores">Maximum stores allowed by subscription</param>
    /// <returns>True if user can create more stores</returns>
    public bool CanCreateStore(int maxStores)
    {
        return StoreIds.Count < maxStores;
    }
}

