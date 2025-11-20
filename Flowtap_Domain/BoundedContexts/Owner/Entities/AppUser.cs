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

    [MaxLength(50)]
    public string? Language { get; set; }

    // Subscription
    public Guid? SubscriptionId { get; set; }

    public DateTime? TrialStartDate { get; set; }

    public DateTime? TrialEndDate { get; set; }

    public TrialStatus TrialStatus { get; set; } = TrialStatus.NotStarted;

    [NotMapped]
    public bool IsTrialActive => TrialEndDate.HasValue && DateTime.UtcNow <= TrialEndDate.Value;

    // Co-owners
    // public ICollection<AppUserAdmin> Admins { get; set; } = new List<AppUserAdmin>();

    // Stores (IDs only to avoid cross-context coupling)
    public ICollection<Guid> StoreIds { get; set; } = new List<Guid>();

    // Default Store
    public Guid? DefaultStoreId { get; set; }

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

    // Note: Admin management methods removed since AppUserAdmin is no longer part of the DbContext
    // If admin functionality is needed in the future, it should be implemented through a different mechanism

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

    /// <summary>
    /// Sets the default store for this user
    /// </summary>
    /// <param name="storeId">The store ID to set as default</param>
    public void SetDefaultStore(Guid storeId)
    {
        if (!StoreIds.Contains(storeId))
            throw new InvalidOperationException($"Store {storeId} is not linked to this user");

        DefaultStoreId = storeId;
        UpdatedAt = DateTime.UtcNow;
    }
}

