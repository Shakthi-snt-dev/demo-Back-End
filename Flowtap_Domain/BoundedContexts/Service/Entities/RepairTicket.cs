using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Flowtap_Domain.SharedKernel.Enums;

namespace Flowtap_Domain.BoundedContexts.Service.Entities;

public class RepairTicket
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    // Customer + Store
    [Required]
    public Guid StoreId { get; set; }
    // Note: Store is in Store context - reference by ID only, no navigation property

    [Required]
    public Guid CustomerId { get; set; }
    // Note: Customer is in Sales context - reference by ID only, no navigation property

    // Device Category, Brand, Model, Variant
    public Guid? DeviceCategoryId { get; set; }
    public DeviceCategory? DeviceCategory { get; set; }

    public Guid? DeviceBrandId { get; set; }
    public DeviceBrand? DeviceBrand { get; set; }

    public Guid? DeviceModelId { get; set; }
    public DeviceModel? DeviceModel { get; set; }

    public Guid? DeviceVariantId { get; set; }
    public DeviceVariant? DeviceVariant { get; set; }

    // Problem selected
    public Guid? DeviceProblemId { get; set; }
    public DeviceProblem? DeviceProblem { get; set; }

    // Device Identification
    [MaxLength(200)]
    public string? IMEI { get; set; }

    [MaxLength(200)]
    public string? SerialNumber { get; set; }

    // Security
    [MaxLength(100)]
    public string? Passcode { get; set; }

    [MaxLength(100)]
    public string? PatternLock { get; set; }

    // Warranty
    public bool IsWarrantyApplicable { get; set; }

    public int WarrantyDays { get; set; } = 0;

    // Technician assignment
    public Guid? TechnicianId { get; set; }
    // Note: Employee is in HR context - reference by ID only, no navigation property

    public DateTime? TaskDueDate { get; set; }

    // Status
    [MaxLength(100)]
    public string Status { get; set; } = "Waiting for inspection"; // RepairDesk default

    public bool IsRushJob { get; set; } = false;

    // Pricing
    [Column(TypeName = "decimal(18,2)")]
    public decimal RepairCharges { get; set; } = 0;

    // Physical location in store
    [MaxLength(200)]
    public string? PhysicalLocation { get; set; } // Drawer 1, Shelf A2, Locker 3

    // Job Type
    [MaxLength(50)]
    public string TaskType { get; set; } = "In-Store"; // Mail-in, Pickup, Delivery

    // Device network status
    [MaxLength(200)]
    public string? NetworkStatus { get; set; } // Locked, Unlocked, SIM issues

    // Notes
    public string? DiagnosticNote { get; set; }

    public string? PrivateComment { get; set; }

    public string? AdditionalNote { get; set; }

    // Legacy fields for backward compatibility
    [MaxLength(100)]
    public string? TicketNumber { get; set; } // e.g. RD-2025-0001

    public Guid? DeviceId { get; set; }
    public Device? Device { get; set; }

    [MaxLength(500)]
    public string? ProblemDescription { get; set; }

    [MaxLength(500)]
    public string? ResolutionNotes { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? EstimatedCost { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? ActualCost { get; set; }

    [MaxLength(200)]
    public string? CustomerName { get; set; }

    [MaxLength(20)]
    public string? CustomerPhone { get; set; }

    [MaxLength(320)]
    public string? CustomerEmail { get; set; }

    [MaxLength(200)]
    public string? DeviceDescription { get; set; }

    [MaxLength(50)]
    public string? Priority { get; set; } = "medium";

    [Column(TypeName = "decimal(18,2)")]
    public decimal DepositPaid { get; set; } = 0;

    public DateTime? DueDate { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? EstimatedCompletionAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Pre-Repair Checklist
    public ICollection<TicketPreCheck> PreChecks { get; set; } = new List<TicketPreCheck>();

    // Pre-Repair Images
    public ICollection<TicketConditionImage> ConditionImages { get; set; } = new List<TicketConditionImage>();

    // Parts used
    public ICollection<PartUsed> PartsUsed { get; set; } = new List<PartUsed>();

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Updates the ticket status
    /// </summary>
    public void UpdateStatus(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
            throw new ArgumentException("Status cannot be empty", nameof(status));

        var validStatuses = new[] { "New", "Waiting for inspection", "InProgress", "Waiting for parts", 
            "Ready for pickup", "Completed", "Returned", "Cancelled" };

        if (!validStatuses.Contains(status, StringComparer.OrdinalIgnoreCase))
            throw new InvalidOperationException($"Invalid status: {status}");

        Status = status;
        UpdatedAt = DateTime.UtcNow;

        if (status.Equals("Completed", StringComparison.OrdinalIgnoreCase) && !CompletedAt.HasValue)
        {
            CompletedAt = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Updates the ticket status using enum (for backward compatibility)
    /// </summary>
    public void UpdateStatus(TicketStatus status)
    {
        Status = status.ToString();
        UpdatedAt = DateTime.UtcNow;

        if (status == TicketStatus.Completed && !CompletedAt.HasValue)
        {
            CompletedAt = DateTime.UtcNow;
        }
    }

    public void UpdatePriority(string priority)
    {
        var validPriorities = new[] { "low", "medium", "high", "urgent" };
        if (!validPriorities.Contains(priority.ToLower()))
            throw new InvalidOperationException($"Invalid priority: {priority}");

        Priority = priority.ToLower();
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Assigns the ticket to a technician (employee)
    /// </summary>
    public void AssignToTechnician(Guid technicianId)
    {
        TechnicianId = technicianId;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Unassigns the technician from the ticket
    /// </summary>
    public void UnassignTechnician()
    {
        TechnicianId = null;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Assigns the ticket to an employee (legacy method for backward compatibility)
    /// </summary>
    public void AssignToEmployee(Guid employeeId)
    {
        AssignToTechnician(employeeId);
    }

    /// <summary>
    /// Unassigns the employee from the ticket (legacy method for backward compatibility)
    /// </summary>
    public void UnassignEmployee()
    {
        UnassignTechnician();
    }

    /// <summary>
    /// Updates the estimated cost
    /// </summary>
    public void UpdateEstimatedCost(decimal? cost)
    {
        if (cost.HasValue && cost.Value < 0)
            throw new InvalidOperationException("Estimated cost cannot be negative");

        EstimatedCost = cost;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the actual cost
    /// </summary>
    public void UpdateActualCost(decimal? cost)
    {
        if (cost.HasValue && cost.Value < 0)
            throw new InvalidOperationException("Actual cost cannot be negative");

        ActualCost = cost;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddDeposit(decimal amount)
    {
        if (amount < 0)
            throw new InvalidOperationException("Deposit amount cannot be negative");

        DepositPaid += amount;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Sets the estimated completion date
    /// </summary>
    public void SetEstimatedCompletionAt(DateTime? estimatedCompletionAt)
    {
        if (estimatedCompletionAt.HasValue && estimatedCompletionAt.Value < CreatedAt)
            throw new InvalidOperationException("Estimated completion date cannot be before creation date");

        EstimatedCompletionAt = estimatedCompletionAt;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the due date (legacy method for backward compatibility)
    /// </summary>
    public void UpdateDueDate(DateTime? dueDate)
    {
        SetEstimatedCompletionAt(dueDate);
        DueDate = dueDate; // Keep for backward compatibility
    }

    /// <summary>
    /// Updates the problem description
    /// </summary>
    public void UpdateProblemDescription(string? problemDescription)
    {
        ProblemDescription = problemDescription;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the resolution notes
    /// </summary>
    public void UpdateResolutionNotes(string? resolutionNotes)
    {
        ResolutionNotes = resolutionNotes;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Links the ticket to a device
    /// </summary>
    public void LinkDevice(Guid deviceId)
    {
        DeviceId = deviceId;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Unlinks the device from the ticket
    /// </summary>
    public void UnlinkDevice()
    {
        DeviceId = null;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Adds a note (legacy method for backward compatibility)
    /// </summary>
    public void AddNote(string note)
    {
        if (string.IsNullOrWhiteSpace(note))
            return;

        Notes = string.IsNullOrWhiteSpace(Notes) ? note : $"{Notes}\n{note}";
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Marks the ticket as completed
    /// </summary>
    public void Complete()
    {
        UpdateStatus(TicketStatus.Completed);
    }

    /// <summary>
    /// Cancels the ticket
    /// </summary>
    public void Cancel()
    {
        UpdateStatus(TicketStatus.Cancelled);
    }

    /// <summary>
    /// Checks if the ticket is completed
    /// </summary>
    public bool IsCompleted()
    {
        return Status.Equals("Completed", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Checks if the ticket is cancelled
    /// </summary>
    public bool IsCancelled()
    {
        return Status.Equals("Cancelled", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Checks if the ticket is overdue
    /// </summary>
    public bool IsOverdue()
    {
        var dueDate = EstimatedCompletionAt ?? DueDate;
        return dueDate.HasValue && 
               dueDate.Value < DateTime.UtcNow && 
               !IsCompleted() && 
               !IsCancelled();
    }

    /// <summary>
    /// Gets the balance due
    /// </summary>
    public decimal GetBalanceDue()
    {
        var cost = ActualCost ?? EstimatedCost ?? 0m;
        return cost - DepositPaid;
    }

    /// <summary>
    /// Gets the number of parts used
    /// </summary>
    public int GetPartsUsedCount()
    {
        return PartsUsed.Count;
    }

    /// <summary>
    /// Sets device hierarchy (Category, Brand, Model, Variant)
    /// </summary>
    public void SetDeviceHierarchy(Guid? categoryId, Guid? brandId, Guid? modelId, Guid? variantId)
    {
        DeviceCategoryId = categoryId;
        DeviceBrandId = brandId;
        DeviceModelId = modelId;
        DeviceVariantId = variantId;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Sets the device problem
    /// </summary>
    public void SetDeviceProblem(Guid? problemId)
    {
        DeviceProblemId = problemId;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates device identification
    /// </summary>
    public void UpdateDeviceIdentification(string? imei, string? serialNumber)
    {
        IMEI = imei;
        SerialNumber = serialNumber;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates security information
    /// </summary>
    public void UpdateSecurityInfo(string? passcode, string? patternLock)
    {
        Passcode = passcode;
        PatternLock = patternLock;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Sets warranty information
    /// </summary>
    public void SetWarrantyInfo(bool isWarrantyApplicable, int warrantyDays)
    {
        IsWarrantyApplicable = isWarrantyApplicable;
        WarrantyDays = warrantyDays;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates repair charges
    /// </summary>
    public void UpdateRepairCharges(decimal repairCharges)
    {
        if (repairCharges < 0)
            throw new ArgumentException("Repair charges cannot be negative", nameof(repairCharges));

        RepairCharges = repairCharges;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Marks as rush job
    /// </summary>
    public void MarkAsRushJob(bool isRush)
    {
        IsRushJob = isRush;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates physical location
    /// </summary>
    public void UpdatePhysicalLocation(string? location)
    {
        if (location != null && location.Length > 200)
            throw new ArgumentException("Physical location cannot exceed 200 characters", nameof(location));

        PhysicalLocation = location;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates task type
    /// </summary>
    public void UpdateTaskType(string taskType)
    {
        if (string.IsNullOrWhiteSpace(taskType))
            throw new ArgumentException("Task type cannot be empty", nameof(taskType));
        if (taskType.Length > 50)
            throw new ArgumentException("Task type cannot exceed 50 characters", nameof(taskType));

        TaskType = taskType;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates network status
    /// </summary>
    public void UpdateNetworkStatus(string? networkStatus)
    {
        if (networkStatus != null && networkStatus.Length > 200)
            throw new ArgumentException("Network status cannot exceed 200 characters", nameof(networkStatus));

        NetworkStatus = networkStatus;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates diagnostic note
    /// </summary>
    public void UpdateDiagnosticNote(string? note)
    {
        DiagnosticNote = note;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates private comment
    /// </summary>
    public void UpdatePrivateComment(string? comment)
    {
        PrivateComment = comment;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates additional note
    /// </summary>
    public void UpdateAdditionalNote(string? note)
    {
        AdditionalNote = note;
        UpdatedAt = DateTime.UtcNow;
    }
}

