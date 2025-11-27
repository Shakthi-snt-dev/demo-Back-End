using System.ComponentModel.DataAnnotations;

namespace Flowtap_Domain.BoundedContexts.Inventory.Entities;

public class BarcodeTemplate
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid StoreId { get; set; }
    // Note: Store is in Store context - reference by ID only, no navigation property

    [MaxLength(200)]
    public string TemplateName { get; set; } = "Default";

    // JSON structure for barcode label layout
    public string TemplateJson { get; set; } = @"{
        ""width"": ""50mm"",
        ""height"": ""30mm"",
        ""fields"": [
            { ""type"": ""text"", ""value"": ""{SKU}"", ""x"": 5, ""y"": 5 },
            { ""type"": ""barcode"", ""value"": ""{SKU}"", ""x"": 5, ""y"": 12 }
        ]
    }";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Updates the template name
    /// </summary>
    public void UpdateTemplateName(string templateName)
    {
        if (string.IsNullOrWhiteSpace(templateName))
            throw new ArgumentException("Template name cannot be empty", nameof(templateName));
        if (templateName.Length > 200)
            throw new ArgumentException("Template name cannot exceed 200 characters", nameof(templateName));

        TemplateName = templateName;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the template JSON
    /// </summary>
    public void UpdateTemplateJson(string templateJson)
    {
        if (string.IsNullOrWhiteSpace(templateJson))
            throw new ArgumentException("Template JSON cannot be empty", nameof(templateJson));

        TemplateJson = templateJson;
        UpdatedAt = DateTime.UtcNow;
    }
}

