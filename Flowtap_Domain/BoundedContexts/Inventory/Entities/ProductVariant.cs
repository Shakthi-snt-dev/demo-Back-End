using System.ComponentModel.DataAnnotations;

namespace Flowtap_Domain.BoundedContexts.Inventory.Entities;

public class ProductVariant
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid ProductId { get; set; }

    public Product? Product { get; set; }

    [MaxLength(100)]
    public string? AttributeName { get; set; }  // Color, Size, etc.

    [MaxLength(100)]
    public string? AttributeValue { get; set; } // Black, 128GB, etc.

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Updates the variant attributes
    /// </summary>
    public void UpdateAttributes(string? attributeName, string? attributeValue)
    {
        if (attributeName != null && attributeName.Length > 100)
            throw new ArgumentException("Attribute name cannot exceed 100 characters", nameof(attributeName));
        if (attributeValue != null && attributeValue.Length > 100)
            throw new ArgumentException("Attribute value cannot exceed 100 characters", nameof(attributeValue));

        AttributeName = attributeName;
        AttributeValue = attributeValue;
    }
}

