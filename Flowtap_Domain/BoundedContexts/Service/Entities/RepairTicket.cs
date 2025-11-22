using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Flowtap_Domain.SharedKernel.Enums;

namespace Flowtap_Domain.BoundedContexts.Service.Entities;

public class RepairTicket
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid StoreId { get; set; } // Reference to Store context (ID only, no navigation)

    [Required]
    public Guid CustomerId { get; set; } // Reference to Sales.Customer (ID only, no navigation)

    [MaxLength(100)]
    public string TicketNumber { get; set; } = string.Empty; // e.g. RD-2025-0001

    public TicketStatus Status { get; set; } = TicketStatus.Open;

    public Guid? DeviceId { get; set; }

    public Device? Device { get; set; }

    [MaxLength(500)]
    public string? ProblemDescription { get; set; }

    [MaxLength(500)]
    public string? ResolutionNotes { get; set; }

    public Guid? TechnicianId { get; set; } // Maps to Employee.Id in HR context

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? EstimatedCompletionAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "decimal(18,2)")]
    public decimal? EstimatedCost { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? ActualCost { get; set; }

    // Legacy fields for backward compatibility
    [MaxLength(200)]
    public string? CustomerName { get; set; }

    [MaxLength(20)]
    public string? CustomerPhone { get; set; }

    [MaxLength(320)]
    public string? CustomerEmail { get; set; }

    [MaxLength(200)]
    public string? DeviceDescription { get; set; } // Legacy field, use Device entity instead

    [MaxLength(50)]
    public string? Priority { get; set; } = "medium"; // low, medium, high, urgent

    [Column(TypeName = "decimal(18,2)")]
    public decimal DepositPaid { get; set; } = 0;

    public DateTime? DueDate { get; set; }

    public string? Notes { get; set; }

    public ICollection<PartUsed> PartsUsed { get; set; } = new List<PartUsed>();

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Updates the ticket status
    /// </summary>
    public void UpdateStatus(TicketStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;

        if (status == TicketStatus.Completed && !CompletedAt.HasValue)
        {
            CompletedAt = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Updates the ticket status (legacy string method for backward compatibility)
    /// </summary>
    public void UpdateStatus(string status)
    {
        var statusMap = new Dictionary<string, TicketStatus>(StringComparer.OrdinalIgnoreCase)
        {
            { "pending", TicketStatus.Open },
            { "open", TicketStatus.Open },
            { "in-progress", TicketStatus.InProgress },
            { "inprogress", TicketStatus.InProgress },
            { "completed", TicketStatus.Completed },
            { "cancelled", TicketStatus.Cancelled }
        };

        if (!statusMap.TryGetValue(status, out var ticketStatus))
            throw new InvalidOperationException($"Invalid status: {status}");

        UpdateStatus(ticketStatus);
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
        return Status == TicketStatus.Completed;
    }

    /// <summary>
    /// Checks if the ticket is cancelled
    /// </summary>
    public bool IsCancelled()
    {
        return Status == TicketStatus.Cancelled;
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
}

