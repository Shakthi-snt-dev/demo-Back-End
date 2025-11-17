namespace Flowtap_Domain.SharedKernel.Enums;

/// <summary>
/// Employee roles in the system
/// </summary>
public enum EmployeeRole
{
    /// <summary>
    /// Owner - The primary account holder who owns the subscription
    /// </summary>
    Owner = 1,
    
    /// <summary>
    /// Partner - Secondary owner/admin with full access
    /// </summary>
    Partner = 2,
    
    /// <summary>
    /// Admin - Administrator with management permissions
    /// </summary>
    Admin = 3,
    
    /// <summary>
    /// SuperAdmin - Super administrator with all permissions
    /// </summary>
    SuperAdmin = 4,
    
    /// <summary>
    /// Manager - Store manager with operational permissions
    /// </summary>
    Manager = 5,
    
    /// <summary>
    /// Technician - Technical staff member
    /// </summary>
    Technician = 6,
    
    /// <summary>
    /// Cashier - Point of sale operator
    /// </summary>
    Cashier = 7
}

