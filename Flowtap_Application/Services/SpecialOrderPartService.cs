using AutoMapper;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.BoundedContexts.Service.Entities;
using Flowtap_Domain.BoundedContexts.Service.Interfaces;
using Flowtap_Domain.BoundedContexts.Procurement.Interfaces;
using Flowtap_Domain.BoundedContexts.Store.Interfaces;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;
using Flowtap_Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Flowtap_Application.Services;

public class SpecialOrderPartService : ISpecialOrderPartService
{
    private readonly ISpecialOrderPartRepository _specialOrderPartRepository;
    private readonly IStoreRepository _storeRepository;
    private readonly ISupplierRepository _supplierRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<SpecialOrderPartService> _logger;

    public SpecialOrderPartService(
        ISpecialOrderPartRepository specialOrderPartRepository,
        IStoreRepository storeRepository,
        ISupplierRepository supplierRepository,
        IMapper mapper,
        ILogger<SpecialOrderPartService> logger)
    {
        _specialOrderPartRepository = specialOrderPartRepository;
        _storeRepository = storeRepository;
        _supplierRepository = supplierRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<SpecialOrderPartResponseDto> CreateSpecialOrderPartAsync(CreateSpecialOrderPartRequestDto request)
    {
        // Validate store exists
        var store = await _storeRepository.GetByIdAsync(request.StoreId);
        if (store == null)
        {
            throw new EntityNotFoundException("Store", request.StoreId);
        }

        // Validate supplier exists
        var supplier = await _supplierRepository.GetByIdAsync(request.SupplierId);
        if (supplier == null)
        {
            throw new EntityNotFoundException("Supplier", request.SupplierId);
        }

        var part = _mapper.Map<SpecialOrderPart>(request);
        part.Id = Guid.NewGuid();
        part.CreatedAt = DateTime.UtcNow;
        part.UpdatedAt = DateTime.UtcNow;

        var created = await _specialOrderPartRepository.CreateAsync(part);
        _logger.LogInformation("Created special order part {PartId} for store {StoreId}", created.Id, request.StoreId);

        return await MapToResponseDto(created);
    }

    public async Task<SpecialOrderPartResponseDto> GetSpecialOrderPartByIdAsync(Guid id)
    {
        var part = await _specialOrderPartRepository.GetByIdAsync(id);
        if (part == null)
        {
            throw new EntityNotFoundException("SpecialOrderPart", id);
        }

        return await MapToResponseDto(part);
    }

    public async Task<IEnumerable<SpecialOrderPartResponseDto>> GetSpecialOrderPartsByStoreIdAsync(Guid storeId)
    {
        var parts = await _specialOrderPartRepository.GetByStoreIdAsync(storeId);
        var result = new List<SpecialOrderPartResponseDto>();

        foreach (var part in parts)
        {
            result.Add(await MapToResponseDto(part));
        }

        return result;
    }

    public async Task<IEnumerable<SpecialOrderPartResponseDto>> GetPendingOrdersAsync(Guid storeId)
    {
        var parts = await _specialOrderPartRepository.GetPendingOrdersAsync(storeId);
        var result = new List<SpecialOrderPartResponseDto>();

        foreach (var part in parts)
        {
            result.Add(await MapToResponseDto(part));
        }

        return result;
    }

    public async Task<SpecialOrderPartResponseDto> UpdateSpecialOrderPartAsync(Guid id, UpdateSpecialOrderPartRequestDto request)
    {
        var part = await _specialOrderPartRepository.GetByIdAsync(id);
        if (part == null)
        {
            throw new EntityNotFoundException("SpecialOrderPart", id);
        }

        if (!string.IsNullOrWhiteSpace(request.ItemName))
            part.UpdatePartInfo(
                request.ItemName,
                request.RequiredQty ?? part.RequiredQty,
                request.UnitCost ?? part.UnitCost,
                request.RetailPrice ?? part.RetailPrice,
                request.SupplierId ?? part.SupplierId
            );

        if (request.OrderLink != null || request.TrackingId != null || request.Notes != null || request.OrderDate.HasValue || request.ReceivedDate.HasValue)
        {
            part.UpdateOrderDetails(
                request.OrderLink ?? part.OrderLink,
                request.TrackingId ?? part.TrackingId,
                request.Notes ?? part.Notes,
                request.OrderDate ?? part.OrderDate,
                request.ReceivedDate ?? part.ReceivedDate
            );
        }

        if (request.IsTaxExclusive.HasValue)
            part.IsTaxExclusive = request.IsTaxExclusive.Value;

        if (request.CreatePurchaseOrder.HasValue)
            part.CreatePurchaseOrder = request.CreatePurchaseOrder.Value;

        await _specialOrderPartRepository.UpdateAsync(part);
        return await MapToResponseDto(part);
    }

    public async Task<SpecialOrderPartResponseDto> MarkAsReceivedAsync(Guid id, DateTime? receivedDate = null)
    {
        var part = await _specialOrderPartRepository.GetByIdAsync(id);
        if (part == null)
        {
            throw new EntityNotFoundException("SpecialOrderPart", id);
        }

        part.MarkAsReceived(receivedDate);
        await _specialOrderPartRepository.UpdateAsync(part);
        return await MapToResponseDto(part);
    }

    public async Task<bool> DeleteSpecialOrderPartAsync(Guid id)
    {
        return await _specialOrderPartRepository.DeleteAsync(id);
    }

    private async Task<SpecialOrderPartResponseDto> MapToResponseDto(SpecialOrderPart part)
    {
        var dto = _mapper.Map<SpecialOrderPartResponseDto>(part);
        dto.TotalCost = part.GetTotalCost();

        // Fetch supplier name if needed
        if (part.SupplierId != Guid.Empty)
        {
            var supplier = await _supplierRepository.GetByIdAsync(part.SupplierId);
            if (supplier != null)
            {
                dto.SupplierName = supplier.Name;
            }
        }

        return dto;
    }
}

