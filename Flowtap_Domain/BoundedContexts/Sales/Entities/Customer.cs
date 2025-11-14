using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Flowtap_Domain.SharedKernel.ValueObjects;

namespace Flowtap_Domain.BoundedContexts.Sales.Entities;

public class Customer
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid StoreId { get; set; }

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

    [Required, MaxLength(50)]
    public string Status { get; set; } = "active"; // active, vip, inactive

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

    public void MarkAsVip()
    {
        Status = "vip";
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsActive()
    {
        Status = "active";
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsInactive()
    {
        Status = "inactive";
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddOrder(decimal amount)
    {
        TotalOrders++;
        TotalSpent += amount;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsVip()
    {
        return Status == "vip";
    }

    public bool IsActive()
    {
        return Status == "active" || Status == "vip";
    }
}

