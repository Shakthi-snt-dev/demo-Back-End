using System.ComponentModel.DataAnnotations;
using Flowtap_Domain.SharedKernel.Enums;

namespace Flowtap_Domain.BoundedContexts.Billing.Entities;

public class Subscription
{
    [Key]
    public Guid Id { get; set; }

    public Guid AppUserId { get; set; }

    public string PlanName { get; set; } = string.Empty;

    public int MaxStores { get; set; }

    public decimal PricePerMonth { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Active;

    public BillingInterval BillingInterval { get; set; } = BillingInterval.Monthly;

    public string? PaymentProvider { get; set; }

    public string? ExternalSubscriptionId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Checks if the subscription is currently active
    /// </summary>
    public bool IsActive()
    {
        return Status == SubscriptionStatus.Active &&
               DateTime.UtcNow >= StartDate &&
               DateTime.UtcNow <= EndDate;
    }

    /// <summary>
    /// Checks if the subscription has expired
    /// </summary>
    public bool IsExpired()
    {
        return DateTime.UtcNow > EndDate || Status == SubscriptionStatus.Expired;
    }

    /// <summary>
    /// Checks if the subscription is cancelled
    /// </summary>
    public bool IsCancelled()
    {
        return Status == SubscriptionStatus.Cancelled;
    }

    /// <summary>
    /// Cancels the subscription
    /// </summary>
    /// <param name="effectiveDate">When the cancellation takes effect (defaults to now)</param>
    public void Cancel(DateTime? effectiveDate = null)
    {
        if (Status == SubscriptionStatus.Cancelled)
            throw new InvalidOperationException("Subscription is already cancelled");

        if (Status == SubscriptionStatus.Expired)
            throw new InvalidOperationException("Cannot cancel an expired subscription");

        Status = SubscriptionStatus.Cancelled;
        if (effectiveDate.HasValue && effectiveDate.Value < EndDate)
        {
            EndDate = effectiveDate.Value;
        }
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Pauses the subscription
    /// </summary>
    public void Pause()
    {
        if (Status != SubscriptionStatus.Active)
            throw new InvalidOperationException($"Cannot pause subscription. Current status: {Status}");

        Status = SubscriptionStatus.Paused;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Resumes a paused subscription
    /// </summary>
    public void Resume()
    {
        if (Status != SubscriptionStatus.Paused)
            throw new InvalidOperationException($"Cannot resume subscription. Current status: {Status}");

        Status = SubscriptionStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Renews the subscription for another billing period
    /// </summary>
    public void Renew()
    {
        if (Status != SubscriptionStatus.Active)
            throw new InvalidOperationException($"Cannot renew subscription. Current status: {Status}");

        var monthsToAdd = BillingInterval == BillingInterval.Yearly ? 12 : 1;
        StartDate = EndDate;
        EndDate = EndDate.AddMonths(monthsToAdd);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the subscription plan
    /// </summary>
    /// <param name="planName">New plan name</param>
    /// <param name="maxStores">New maximum stores</param>
    /// <param name="pricePerMonth">New price per month</param>
    public void UpdatePlan(string planName, int maxStores, decimal pricePerMonth)
    {
        if (string.IsNullOrWhiteSpace(planName))
            throw new ArgumentException("Plan name cannot be empty", nameof(planName));

        if (maxStores <= 0)
            throw new ArgumentException("Max stores must be greater than 0", nameof(maxStores));

        if (pricePerMonth < 0)
            throw new ArgumentException("Price cannot be negative", nameof(pricePerMonth));

        PlanName = planName;
        MaxStores = maxStores;
        PricePerMonth = pricePerMonth;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the number of days until expiration
    /// </summary>
    public int DaysUntilExpiration()
    {
        if (IsExpired())
            return 0;

        return (EndDate - DateTime.UtcNow).Days;
    }
}

