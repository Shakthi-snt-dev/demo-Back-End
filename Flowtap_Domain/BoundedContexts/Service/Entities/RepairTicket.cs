using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flowtap_Domain.BoundedContexts.Service.Entities;

public class RepairTicket
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(50)]
    public string TicketNumber { get; set; } = string.Empty;

    [Required]
    public Guid StoreId { get; set; }

    public Guid? CustomerId { get; set; } // Optional link to Sales.Customer

    [Required, MaxLength(200)]
    public string CustomerName { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? CustomerPhone { get; set; }

    [MaxLength(320)]
    public string? CustomerEmail { get; set; }

    [Required, MaxLength(200)]
    public string Device { get; set; } = string.Empty;

    [Required]
    public string Issue { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string Status { get; set; } = "pending"; // pending, in-progress, completed, cancelled

    [Required, MaxLength(50)]
    public string Priority { get; set; } = "medium"; // low, medium, high, urgent

    [Column(TypeName = "decimal(18,2)")]
    public decimal EstimatedCost { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal DepositPaid { get; set; } = 0;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? DueDate { get; set; }

    public Guid? AssignedToEmployeeId { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Domain methods
    public void UpdateStatus(string status)
    {
        var validStatuses = new[] { "pending", "in-progress", "completed", "cancelled" };
        if (!validStatuses.Contains(status.ToLower()))
            throw new InvalidOperationException($"Invalid status: {status}");

        Status = status.ToLower();
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePriority(string priority)
    {
        var validPriorities = new[] { "low", "medium", "high", "urgent" };
        if (!validPriorities.Contains(priority.ToLower()))
            throw new InvalidOperationException($"Invalid priority: {priority}");

        Priority = priority.ToLower();
        UpdatedAt = DateTime.UtcNow;
    }

    public void AssignToEmployee(Guid employeeId)
    {
        AssignedToEmployeeId = employeeId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UnassignEmployee()
    {
        AssignedToEmployeeId = null;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateEstimatedCost(decimal cost)
    {
        if (cost < 0)
            throw new InvalidOperationException("Estimated cost cannot be negative");

        EstimatedCost = cost;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddDeposit(decimal amount)
    {
        if (amount < 0)
            throw new InvalidOperationException("Deposit amount cannot be negative");

        DepositPaid += amount;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateDueDate(DateTime dueDate)
    {
        if (dueDate < CreatedDate)
            throw new InvalidOperationException("Due date cannot be before creation date");

        DueDate = dueDate;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddNote(string note)
    {
        if (string.IsNullOrWhiteSpace(note))
            return;

        Notes = string.IsNullOrWhiteSpace(Notes) ? note : $"{Notes}\n{note}";
        UpdatedAt = DateTime.UtcNow;
    }

    public void Complete()
    {
        Status = "completed";
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        Status = "cancelled";
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsCompleted()
    {
        return Status == "completed";
    }

    public bool IsCancelled()
    {
        return Status == "cancelled";
    }

    public bool IsOverdue()
    {
        return DueDate.HasValue && DueDate.Value < DateTime.UtcNow && !IsCompleted() && !IsCancelled();
    }

    public decimal GetBalanceDue()
    {
        return EstimatedCost - DepositPaid;
    }
}

