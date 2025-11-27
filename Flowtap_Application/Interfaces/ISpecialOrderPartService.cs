using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;

namespace Flowtap_Application.Interfaces;

public interface ISpecialOrderPartService
{
    Task<SpecialOrderPartResponseDto> CreateSpecialOrderPartAsync(CreateSpecialOrderPartRequestDto request);
    Task<SpecialOrderPartResponseDto> GetSpecialOrderPartByIdAsync(Guid id);
    Task<IEnumerable<SpecialOrderPartResponseDto>> GetSpecialOrderPartsByStoreIdAsync(Guid storeId);
    Task<IEnumerable<SpecialOrderPartResponseDto>> GetPendingOrdersAsync(Guid storeId);
    Task<SpecialOrderPartResponseDto> UpdateSpecialOrderPartAsync(Guid id, UpdateSpecialOrderPartRequestDto request);
    Task<SpecialOrderPartResponseDto> MarkAsReceivedAsync(Guid id, DateTime? receivedDate = null);
    Task<bool> DeleteSpecialOrderPartAsync(Guid id);
}

