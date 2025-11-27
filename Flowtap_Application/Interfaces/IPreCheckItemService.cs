using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;

namespace Flowtap_Application.Interfaces;

public interface IPreCheckItemService
{
    Task<PreCheckItemResponseDto> CreatePreCheckItemAsync(CreatePreCheckItemRequestDto request);
    Task<PreCheckItemResponseDto> GetPreCheckItemByIdAsync(Guid id);
    Task<IEnumerable<PreCheckItemResponseDto>> GetPreCheckItemsByStoreIdAsync(Guid storeId);
    Task<IEnumerable<PreCheckItemResponseDto>> GetActivePreCheckItemsAsync(Guid storeId);
    Task<PreCheckItemResponseDto> UpdatePreCheckItemAsync(Guid id, UpdatePreCheckItemRequestDto request);
    Task<bool> DeletePreCheckItemAsync(Guid id);
}

