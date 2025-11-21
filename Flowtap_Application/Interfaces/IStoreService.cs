using Flowtap_Application.DtoModel;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;

namespace Flowtap_Application.Interfaces;

public interface IStoreService
{
    Task<StoreResponseDto> CreateStoreAsync(CreateStoreRequestDto request);
    Task<StoreResponseDto> GetStoreByIdAsync(Guid id);
    Task<IEnumerable<StoreResponseDto>> GetAllStoresAsync();
    Task<IEnumerable<StoreResponseDto>> GetStoresByAppUserIdAsync(Guid appUserId);
    Task<IEnumerable<StoreResponseDto>> GetStoresByTypeAsync(string storeType);
    Task<StoreResponseDto> UpdateStoreAsync(Guid id, UpdateStoreRequestDto request);
    Task<bool> DeleteStoreAsync(Guid id);
    Task<IEnumerable<StoreTypeResponseDto>> GetStoreTypesAsync();
    Task<StoreTypeResponseDto> CreateStoreTypeAsync(CreateStoreTypeRequestDto request);
    Task<bool> DeleteStoreTypeAsync(string name);
    
    /// <summary>
    /// Gets stores for the current user (UserAccount) - returns only ID and Name for dropdowns
    /// </summary>
    Task<IEnumerable<StoreListItemDto>> GetStoresForCurrentUserAsync(Guid userAccountId);
}

