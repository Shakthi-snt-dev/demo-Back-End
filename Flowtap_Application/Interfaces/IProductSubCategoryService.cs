using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;

namespace Flowtap_Application.Interfaces;

public interface IProductSubCategoryService
{
    Task<ProductSubCategoryResponseDto> GetProductSubCategoryByIdAsync(Guid id);
    Task<IEnumerable<ProductSubCategoryResponseDto>> GetAllProductSubCategoriesAsync();
    Task<IEnumerable<ProductSubCategoryResponseDto>> GetProductSubCategoriesByCategoryIdAsync(Guid categoryId);
    Task<ProductSubCategoryResponseDto> CreateProductSubCategoryAsync(CreateProductSubCategoryRequestDto request);
    Task<ProductSubCategoryResponseDto> UpdateProductSubCategoryAsync(Guid id, UpdateProductSubCategoryRequestDto request);
    Task<bool> DeleteProductSubCategoryAsync(Guid id);
}

