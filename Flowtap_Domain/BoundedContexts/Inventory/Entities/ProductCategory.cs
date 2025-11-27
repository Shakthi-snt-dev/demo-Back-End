using System.ComponentModel.DataAnnotations;

namespace Flowtap_Domain.BoundedContexts.Inventory.Entities;

public class ProductCategory
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    public ICollection<ProductSubCategory> SubCategories { get; set; } = new List<ProductSubCategory>();

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Updates the category name
    /// </summary>
    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
        if (name.Length > 200)
            throw new ArgumentException("Name cannot exceed 200 characters", nameof(name));

        Name = name;
    }
}

