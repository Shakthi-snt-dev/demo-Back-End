using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;

namespace Flowtap_Application.Interfaces;

public interface ISupplierService
{
    Task<SupplierResponseDto> CreateSupplierAsync(CreateSupplierRequestDto request);
    Task<SupplierResponseDto> GetSupplierByIdAsync(Guid id);
    Task<IEnumerable<SupplierResponseDto>> GetAllSuppliersAsync();
    Task<IEnumerable<SupplierResponseDto>> GetActiveSuppliersAsync();
    Task<SupplierResponseDto> UpdateSupplierAsync(Guid id, UpdateSupplierRequestDto request);
    Task<bool> DeleteSupplierAsync(Guid id);
}

