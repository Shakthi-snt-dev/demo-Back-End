using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flowtap_Domain.BoundedContexts.Service.Entities;

public class ServiceLabor
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid ServiceId { get; set; }

    public Service? Service { get; set; }

    [MaxLength(200)]
    public string Label { get; set; } = "Labor";

    [Column(TypeName = "decimal(18,2)")]
    public decimal Cost { get; set; }

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Updates the labor information
    /// </summary>
    public void UpdateLaborInfo(string label, decimal cost)
    {
        if (string.IsNullOrWhiteSpace(label))
            throw new ArgumentException("Label cannot be empty", nameof(label));
        if (cost < 0)
            throw new ArgumentException("Cost cannot be negative", nameof(cost));

        Label = label;
        Cost = cost;
    }
}

