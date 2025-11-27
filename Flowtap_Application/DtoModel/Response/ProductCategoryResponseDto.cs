namespace Flowtap_Application.DtoModel.Response;

public class ProductCategoryResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<ProductSubCategoryResponseDto> SubCategories { get; set; } = new();
}

public class ProductSubCategoryResponseDto
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

