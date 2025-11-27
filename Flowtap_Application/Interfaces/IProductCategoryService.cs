using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;

namespace Flowtap_Application.Interfaces;

public interface IProductCategoryService
{
    Task<ProductCategoryResponseDto> GetProductCategoryByIdAsync(Guid id);
    Task<IEnumerable<ProductCategoryResponseDto>> GetAllProductCategoriesAsync();
    Task<ProductCategoryResponseDto> CreateProductCategoryAsync(CreateProductCategoryRequestDto request);
    Task<ProductCategoryResponseDto> UpdateProductCategoryAsync(Guid id, UpdateProductCategoryRequestDto request);
    Task<bool> DeleteProductCategoryAsync(Guid id);
}

