namespace Flowtap_Domain.DtoModel;

/// <summary>
/// Represents a form field configuration for dynamic forms
/// </summary>
public class FormFieldDto
{
    public string Id { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string Type { get; set; } = "text"; // text, email, password, number, select, checkbox, switch, textarea, date, time
    public string? Placeholder { get; set; }
    public string? HelpText { get; set; }
    public bool Required { get; set; } = false;
    public bool Disabled { get; set; } = false;
    public bool Visible { get; set; } = true;
    public object? DefaultValue { get; set; }
    public string? ValidationPattern { get; set; }
    public string? ValidationMessage { get; set; }
    public int? MinLength { get; set; }
    public int? MaxLength { get; set; }
    public double? Min { get; set; }
    public double? Max { get; set; }
    public double? Step { get; set; } // For number inputs
    public int? Rows { get; set; } // For textarea
    public List<SelectOptionDto>? Options { get; set; } // For select, radio, checkbox groups
    public Dictionary<string, object>? Metadata { get; set; } // Additional metadata
    public int? Order { get; set; } // Display order
    public string? Group { get; set; } // Group name for grouping fields
    public string? Icon { get; set; } // Icon name for the field
}

public class SelectOptionDto
{
    public string Value { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public bool Disabled { get; set; } = false;
    public string? Icon { get; set; }
}

/// <summary>
/// Form configuration response
/// </summary>
public class FormConfigurationDto
{
    public string FormId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<FormFieldDto> Fields { get; set; } = new();
    public Dictionary<string, object>? DefaultValues { get; set; }
    public List<string>? Groups { get; set; } // Field groups for organization
}

