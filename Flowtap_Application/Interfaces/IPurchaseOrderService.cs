using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;

namespace Flowtap_Application.Interfaces;

public interface IPurchaseOrderService
{
    Task<PurchaseOrderResponseDto> CreatePurchaseOrderAsync(CreatePurchaseOrderRequestDto request);
    Task<PurchaseOrderResponseDto> GetPurchaseOrderByIdAsync(Guid id);
    Task<IEnumerable<PurchaseOrderResponseDto>> GetPurchaseOrdersByStoreIdAsync(Guid storeId);
    Task<IEnumerable<PurchaseOrderResponseDto>> GetPurchaseOrdersBySupplierIdAsync(Guid supplierId);
    Task<PurchaseOrderResponseDto> SubmitPurchaseOrderAsync(Guid id);
    Task<PurchaseOrderResponseDto> MarkAsReceivedAsync(Guid id);
    Task<PurchaseOrderResponseDto> CancelPurchaseOrderAsync(Guid id);
    Task<bool> DeletePurchaseOrderAsync(Guid id);
}

