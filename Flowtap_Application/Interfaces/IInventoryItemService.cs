using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;

namespace Flowtap_Application.Interfaces;

public interface IInventoryItemService
{
    Task<InventoryItemResponseDto> CreateInventoryItemAsync(CreateInventoryItemRequestDto request);
    Task<InventoryItemResponseDto> GetInventoryItemByIdAsync(Guid id);
    Task<IEnumerable<InventoryItemResponseDto>> GetInventoryItemsByStoreIdAsync(Guid storeId);
    Task<IEnumerable<InventoryItemResponseDto>> GetInventoryItemsByProductIdAsync(Guid productId);
    Task<IEnumerable<InventoryItemResponseDto>> GetLowStockItemsAsync(Guid storeId);
    Task<InventoryItemResponseDto> UpdateInventoryItemAsync(Guid id, UpdateInventoryItemRequestDto request);
    Task<InventoryItemResponseDto> AddQuantityAsync(Guid id, int quantity);
    Task<InventoryItemResponseDto> RemoveQuantityAsync(Guid id, int quantity);
    Task<InventoryItemResponseDto> ReserveQuantityAsync(Guid id, int quantity);
    Task<InventoryItemResponseDto> ReleaseReservedQuantityAsync(Guid id, int quantity);
    Task<bool> DeleteInventoryItemAsync(Guid id);
}

