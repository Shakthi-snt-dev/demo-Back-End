using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Flowtap_Domain.SharedKernel.ValueObjects;
using Flowtap_Domain.SharedKernel.Enums;

namespace Flowtap_Domain.BoundedContexts.Sales.Entities;

public class Customer
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid StoreId { get; set; } // Reference to Store context (ID only, no navigation)

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(320)]
    public string? Email { get; set; }

    [MaxLength(20)]
    public string? Phone { get; set; }

    public Address? Address { get; set; }

    public int TotalOrders { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalSpent { get; set; } = 0;

    [Required]
    public CustomerStatus Status { get; set; } = CustomerStatus.Active;

    public bool IsSyncedWithExternal { get; set; } = false;

    [MaxLength(100)]
    public string? ExternalId { get; set; }

    // Navigation properties within Sales bounded context only
    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    // Note: RepairTickets are in Service context - reference by CustomerId only, no navigation property

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Domain methods
    public void UpdateContactInfo(string? email, string? phone)
    {
        if (!string.IsNullOrWhiteSpace(email))
            Email = email;
        if (!string.IsNullOrWhiteSpace(phone))
            Phone = phone;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateAddress(Address address)
    {
        Address = address;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Marks the customer as VIP
    /// </summary>
    public void MarkAsVip()
    {
        Status = CustomerStatus.VIP;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Marks the customer as active
    /// </summary>
    public void MarkAsActive()
    {
        Status = CustomerStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Marks the customer as inactive
    /// </summary>
    public void MarkAsInactive()
    {
        Status = CustomerStatus.Inactive;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the customer status
    /// </summary>
    public void UpdateStatus(CustomerStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddOrder(decimal amount)
    {
        TotalOrders++;
        TotalSpent += amount;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Checks if the customer is VIP
    /// </summary>
    public bool IsVip()
    {
        return Status == CustomerStatus.VIP;
    }

    /// <summary>
    /// Checks if the customer is active
    /// </summary>
    public bool IsActive()
    {
        return Status == CustomerStatus.Active || Status == CustomerStatus.VIP;
    }

    /// <summary>
    /// Updates external sync information
    /// </summary>
    public void UpdateExternalSync(bool isSynced, string? externalId = null)
    {
        IsSyncedWithExternal = isSynced;
        ExternalId = externalId;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the number of invoices
    /// </summary>
    public int GetInvoiceCount()
    {
        return Invoices.Count;
    }

    // Note: To get repair ticket count, query Service context using CustomerId
}

