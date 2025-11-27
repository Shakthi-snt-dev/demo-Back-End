using System.ComponentModel.DataAnnotations;

namespace Flowtap_Application.DtoModel.Request;

public class CreateProductCategoryRequestDto
{
    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;
}

public class UpdateProductCategoryRequestDto
{
    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;
}

