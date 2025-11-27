using System.ComponentModel.DataAnnotations;

namespace Flowtap_Domain.BoundedContexts.Inventory.Entities;

public class InventoryAdjustment
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid InventoryItemId { get; set; }

    public InventoryItem? InventoryItem { get; set; }

    public int OldQuantity { get; set; }

    public int NewQuantity { get; set; }

    [MaxLength(400)]
    public string Reason { get; set; } = string.Empty;

    public Guid? EmployeeId { get; set; }
    // Note: Employee is in HR context - reference by ID only, no navigation property

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Creates a new inventory adjustment
    /// </summary>
    public static InventoryAdjustment Create(
        Guid inventoryItemId,
        int oldQuantity,
        int newQuantity,
        string reason,
        Guid? employeeId = null)
    {
        if (oldQuantity < 0)
            throw new ArgumentException("Old quantity cannot be negative", nameof(oldQuantity));
        if (newQuantity < 0)
            throw new ArgumentException("New quantity cannot be negative", nameof(newQuantity));
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Reason cannot be empty", nameof(reason));
        if (reason.Length > 400)
            throw new ArgumentException("Reason cannot exceed 400 characters", nameof(reason));

        return new InventoryAdjustment
        {
            Id = Guid.NewGuid(),
            InventoryItemId = inventoryItemId,
            OldQuantity = oldQuantity,
            NewQuantity = newQuantity,
            Reason = reason,
            EmployeeId = employeeId,
            CreatedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Gets the quantity difference
    /// </summary>
    public int GetQuantityDifference()
    {
        return NewQuantity - OldQuantity;
    }

    /// <summary>
    /// Checks if the adjustment is an increase
    /// </summary>
    public bool IsIncrease()
    {
        return NewQuantity > OldQuantity;
    }

    /// <summary>
    /// Checks if the adjustment is a decrease
    /// </summary>
    public bool IsDecrease()
    {
        return NewQuantity < OldQuantity;
    }
}

