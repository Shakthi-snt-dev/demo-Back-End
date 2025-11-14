using Flowtap_Domain.DtoModel;

namespace Flowtap_Application.Interfaces;

public interface IProductService
{
    Task<ProductResponseDto> CreateProductAsync(CreateProductRequestDto request);
    Task<ProductResponseDto> GetProductByIdAsync(Guid id);
    Task<ProductResponseDto?> GetProductBySkuAsync(string sku);
    Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync();
    Task<IEnumerable<ProductResponseDto>> GetProductsByCategoryAsync(string category);
    Task<IEnumerable<ProductResponseDto>> GetLowStockProductsAsync();
    Task<IEnumerable<ProductResponseDto>> SearchProductsAsync(string searchTerm);
    Task<ProductResponseDto> UpdateProductAsync(Guid id, UpdateProductRequestDto request);
    Task<ProductResponseDto> UpdateStockAsync(Guid id, int quantity);
    Task<ProductResponseDto> AddStockAsync(Guid id, int quantity);
    Task<bool> DeleteProductAsync(Guid id);
}

