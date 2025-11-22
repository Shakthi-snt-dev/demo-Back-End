using System.ComponentModel.DataAnnotations;
using Flowtap_Domain.SharedKernel.ValueObjects;

namespace Flowtap_Domain.BoundedContexts.Store.Entities;

public class Store
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid AppUserId { get; set; }

    [Required, MaxLength(200)]
    public string StoreName { get; set; } = string.Empty;

    public string? StoreType { get; set; }

    public string? StoreCategory { get; set; }

    public Address? Address { get; set; }

    public string? Phone { get; set; }

  
    public StoreSettings Settings { get; set; } = new StoreSettings();

    // Employee IDs only to avoid cross-context coupling
    public ICollection<Guid> EmployeeIds { get; set; } = new List<Guid>();

    // Note: Navigation properties to other bounded contexts removed for DDD compliance
    // Reference other contexts by ID only:
    // - Customers: Query Sales context using StoreId
    // - RepairTickets: Query Service context using StoreId
    // - InventoryItems: Query Inventory context using StoreId

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Adds an employee to the store
    /// </summary>
    /// <param name="employeeId">The employee ID to add</param>
    public void AddEmployee(Guid employeeId)
    {
        if (EmployeeIds.Contains(employeeId))
            throw new InvalidOperationException($"Employee {employeeId} is already associated with this store");

        EmployeeIds.Add(employeeId);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Removes an employee from the store
    /// </summary>
    /// <param name="employeeId">The employee ID to remove</param>
    public void RemoveEmployee(Guid employeeId)
    {
        if (!EmployeeIds.Contains(employeeId))
            throw new InvalidOperationException($"Employee {employeeId} is not associated with this store");

        EmployeeIds.Remove(employeeId);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the store settings
    /// </summary>
    /// <param name="enablePOS">Enable POS system</param>
    /// <param name="enableInventory">Enable inventory management</param>
    /// <param name="timeZone">Store timezone</param>
    public void UpdateSettings(bool enablePOS, bool enableInventory, string timeZone)
    {
        if (string.IsNullOrWhiteSpace(timeZone))
            throw new ArgumentException("TimeZone cannot be empty", nameof(timeZone));

        Settings.EnablePOS = enablePOS;
        Settings.EnableInventory = enableInventory;
        Settings.TimeZone = timeZone;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the store address
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
    /// Updates business hours (stored as JSON)
    /// </summary>
    /// <param name="businessHoursJson">Business hours in JSON format</param>
    public void UpdateBusinessHours(string businessHoursJson)
    {
        if (string.IsNullOrWhiteSpace(businessHoursJson))
            throw new ArgumentException("Business hours JSON cannot be empty", nameof(businessHoursJson));

        Settings.BusinessHoursJson = businessHoursJson;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Checks if POS is enabled
    /// </summary>
    public bool IsPOSEnabled()
    {
        return Settings.EnablePOS;
    }

    /// <summary>
    /// Checks if inventory management is enabled
    /// </summary>
    public bool IsInventoryEnabled()
    {
        return Settings.EnableInventory;
    }

    /// <summary>
    /// Gets the number of employees in the store
    /// </summary>
    public int GetEmployeeCount()
    {
        return EmployeeIds.Count;
    }
}

