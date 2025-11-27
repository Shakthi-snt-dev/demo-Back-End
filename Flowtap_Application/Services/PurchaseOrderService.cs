using AutoMapper;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.BoundedContexts.Procurement.Entities;
using Flowtap_Domain.BoundedContexts.Procurement.Interfaces;
using Flowtap_Domain.BoundedContexts.Store.Interfaces;
using Flowtap_Domain.SharedKernel.Enums;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;
using Flowtap_Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Flowtap_Application.Services;

public class PurchaseOrderService : IPurchaseOrderService
{
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;
    private readonly ISupplierRepository _supplierRepository;
    private readonly IStoreRepository _storeRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<PurchaseOrderService> _logger;

    public PurchaseOrderService(
        IPurchaseOrderRepository purchaseOrderRepository,
        ISupplierRepository supplierRepository,
        IStoreRepository storeRepository,
        IMapper mapper,
        ILogger<PurchaseOrderService> logger)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
        _supplierRepository = supplierRepository;
        _storeRepository = storeRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PurchaseOrderResponseDto> CreatePurchaseOrderAsync(CreatePurchaseOrderRequestDto request)
    {
        // Validate supplier exists
        var supplier = await _supplierRepository.GetByIdAsync(request.SupplierId);
        if (supplier == null)
        {
            throw new EntityNotFoundException("Supplier", request.SupplierId);
        }

        // Validate store exists
        var store = await _storeRepository.GetByIdAsync(request.StoreId);
        if (store == null)
        {
            throw new EntityNotFoundException("Store", request.StoreId);
        }

        if (request.Lines == null || !request.Lines.Any())
        {
            throw new System.ArgumentException("Purchase order must have at least one line item");
        }

        var purchaseOrder = new PurchaseOrder
        {
            Id = Guid.NewGuid(),
            SupplierId = request.SupplierId,
            StoreId = request.StoreId,
            Status = PurchaseOrderStatus.Open,
            CreatedAt = DateTime.UtcNow
        };

        if (!string.IsNullOrWhiteSpace(request.PONumber))
        {
            purchaseOrder.PONumber = request.PONumber;
        }
        else
        {
            purchaseOrder.GeneratePONumber();
        }

        // Add line items
        foreach (var lineDto in request.Lines)
        {
            purchaseOrder.AddLine(lineDto.ProductId, lineDto.Quantity, lineDto.UnitCost);
        }

        var created = await _purchaseOrderRepository.CreateAsync(purchaseOrder);
        _logger.LogInformation("Created purchase order {PurchaseOrderId} with PO number {PONumber}", 
            created.Id, created.PONumber);

        return await MapToResponseDto(created);
    }

    public async Task<PurchaseOrderResponseDto> GetPurchaseOrderByIdAsync(Guid id)
    {
        var purchaseOrder = await _purchaseOrderRepository.GetByIdAsync(id);
        if (purchaseOrder == null)
        {
            throw new EntityNotFoundException("PurchaseOrder", id);
        }

        return await MapToResponseDto(purchaseOrder);
    }

    public async Task<IEnumerable<PurchaseOrderResponseDto>> GetPurchaseOrdersByStoreIdAsync(Guid storeId)
    {
        var purchaseOrders = await _purchaseOrderRepository.GetByStoreIdAsync(storeId);
        var result = new List<PurchaseOrderResponseDto>();

        foreach (var po in purchaseOrders)
        {
            result.Add(await MapToResponseDto(po));
        }

        return result;
    }

    public async Task<IEnumerable<PurchaseOrderResponseDto>> GetPurchaseOrdersBySupplierIdAsync(Guid supplierId)
    {
        var purchaseOrders = await _purchaseOrderRepository.GetBySupplierIdAsync(supplierId);
        var result = new List<PurchaseOrderResponseDto>();

        foreach (var po in purchaseOrders)
        {
            result.Add(await MapToResponseDto(po));
        }

        return result;
    }

    public async Task<PurchaseOrderResponseDto> SubmitPurchaseOrderAsync(Guid id)
    {
        var purchaseOrder = await _purchaseOrderRepository.GetByIdAsync(id);
        if (purchaseOrder == null)
        {
            throw new EntityNotFoundException("PurchaseOrder", id);
        }

        purchaseOrder.Submit();
        await _purchaseOrderRepository.UpdateAsync(purchaseOrder);
        return await MapToResponseDto(purchaseOrder);
    }

    public async Task<PurchaseOrderResponseDto> MarkAsReceivedAsync(Guid id)
    {
        var purchaseOrder = await _purchaseOrderRepository.GetByIdAsync(id);
        if (purchaseOrder == null)
        {
            throw new EntityNotFoundException("PurchaseOrder", id);
        }

        purchaseOrder.MarkAsReceived();
        await _purchaseOrderRepository.UpdateAsync(purchaseOrder);
        return await MapToResponseDto(purchaseOrder);
    }

    public async Task<PurchaseOrderResponseDto> CancelPurchaseOrderAsync(Guid id)
    {
        var purchaseOrder = await _purchaseOrderRepository.GetByIdAsync(id);
        if (purchaseOrder == null)
        {
            throw new EntityNotFoundException("PurchaseOrder", id);
        }

        purchaseOrder.Cancel();
        await _purchaseOrderRepository.UpdateAsync(purchaseOrder);
        return await MapToResponseDto(purchaseOrder);
    }

    public async Task<bool> DeletePurchaseOrderAsync(Guid id)
    {
        return await _purchaseOrderRepository.DeleteAsync(id);
    }

    private async Task<PurchaseOrderResponseDto> MapToResponseDto(PurchaseOrder purchaseOrder)
    {
        var dto = _mapper.Map<PurchaseOrderResponseDto>(purchaseOrder);
        
        if (purchaseOrder.Supplier != null)
        {
            dto.Supplier = _mapper.Map<SupplierResponseDto>(purchaseOrder.Supplier);
        }

        dto.Status = purchaseOrder.Status.ToString();
        dto.TotalAmount = purchaseOrder.GetTotalAmount();

        foreach (var line in purchaseOrder.Lines)
        {
            var lineDto = _mapper.Map<PurchaseOrderLineResponseDto>(line);
            lineDto.TotalCost = line.GetTotalCost();
            dto.Lines.Add(lineDto);
        }

        return dto;
    }
}

