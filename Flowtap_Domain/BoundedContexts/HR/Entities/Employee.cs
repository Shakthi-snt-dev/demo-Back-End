using System.ComponentModel.DataAnnotations;
using Flowtap_Domain.SharedKernel.ValueObjects;

namespace Flowtap_Domain.BoundedContexts.HR.Entities;

public class Employee
{
    [Key]
    public Guid Id { get; set; }

    public Guid StoreId { get; set; }

    public string? FullName { get; set; }

    [Required, MaxLength(320)]
    public string Email { get; set; } = string.Empty;

    public string? Role { get; set; }

    public string? AccessPinHash { get; set; }

    /// <summary>
    /// Links to either AppUser.Id (subscription owner) or AppUserAdmin.Id (business owner)
    /// - If LinkedAppUserId exists in AppUser table → subscription owner
    /// - If LinkedAppUserId exists in AppUserAdmin table → business owner
    /// Both have Owner role but LinkedAppUserId distinguishes the type
    /// </summary>
    public Guid? LinkedAppUserId { get; set; }

    public bool IsActive { get; set; } = true;

    public bool CanSwitchRole { get; set; }

    public string? AvatarUrl { get; set; }

    public Address? Address { get; set; }

    // Optional HR fields
    public string? EmployeeCode { get; set; }

    public string? EmergencyContactName { get; set; }

    public string? EmergencyContactPhone { get; set; }

    public decimal? HourlyRate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Activates the employee
    /// </summary>
    public void Activate()
    {
        if (IsActive)
            throw new InvalidOperationException("Employee is already active");

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Deactivates the employee
    /// </summary>
    public void Deactivate()
    {
        if (!IsActive)
            throw new InvalidOperationException("Employee is already inactive");

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the employee's role
    /// </summary>
    /// <param name="newRole">The new role</param>
    public void UpdateRole(string newRole)
    {
        if (string.IsNullOrWhiteSpace(newRole))
            throw new ArgumentException("Role cannot be empty", nameof(newRole));

        if (!CanSwitchRole && Role != null && Role != newRole)
            throw new InvalidOperationException("Employee does not have permission to switch roles");

        Role = newRole;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the employee's address
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
    /// Links the employee to an AppUser account
    /// </summary>
    /// <param name="appUserId">The AppUser ID to link</param>
    public void LinkToAppUser(Guid appUserId)
    {
        if (LinkedAppUserId.HasValue && LinkedAppUserId.Value != appUserId)
            throw new InvalidOperationException($"Employee is already linked to AppUser {LinkedAppUserId.Value}");

        LinkedAppUserId = appUserId;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Unlinks the employee from an AppUser account
    /// </summary>
    public void UnlinkFromAppUser()
    {
        if (!LinkedAppUserId.HasValue)
            throw new InvalidOperationException("Employee is not linked to any AppUser account");

        LinkedAppUserId = null;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the employee's hourly rate
    /// </summary>
    /// <param name="hourlyRate">The new hourly rate</param>
    public void UpdateHourlyRate(decimal hourlyRate)
    {
        if (hourlyRate < 0)
            throw new ArgumentException("Hourly rate cannot be negative", nameof(hourlyRate));

        HourlyRate = hourlyRate;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates emergency contact information
    /// </summary>
    /// <param name="contactName">Emergency contact name</param>
    /// <param name="contactPhone">Emergency contact phone</param>
    public void UpdateEmergencyContact(string contactName, string contactPhone)
    {
        if (string.IsNullOrWhiteSpace(contactName))
            throw new ArgumentException("Contact name cannot be empty", nameof(contactName));

        if (string.IsNullOrWhiteSpace(contactPhone))
            throw new ArgumentException("Contact phone cannot be empty", nameof(contactPhone));

        EmergencyContactName = contactName;
        EmergencyContactPhone = contactPhone;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Checks if the employee is linked to an AppUser account
    /// </summary>
    public bool IsLinkedToAppUser()
    {
        return LinkedAppUserId.HasValue;
    }
}

