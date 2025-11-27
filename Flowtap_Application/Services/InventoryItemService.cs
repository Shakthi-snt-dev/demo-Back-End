using AutoMapper;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.BoundedContexts.Inventory.Entities;
using Flowtap_Domain.BoundedContexts.Inventory.Interfaces;
using Flowtap_Domain.BoundedContexts.Store.Interfaces;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;
using Flowtap_Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Flowtap_Application.Services;

public class InventoryItemService : IInventoryItemService
{
    private readonly IInventoryItemRepository _inventoryItemRepository;
    private readonly IProductRepository _productRepository;
    private readonly IStoreRepository _storeRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<InventoryItemService> _logger;

    public InventoryItemService(
        IInventoryItemRepository inventoryItemRepository,
        IProductRepository productRepository,
        IStoreRepository storeRepository,
        IMapper mapper,
        ILogger<InventoryItemService> logger)
    {
        _inventoryItemRepository = inventoryItemRepository;
        _productRepository = productRepository;
        _storeRepository = storeRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<InventoryItemResponseDto> CreateInventoryItemAsync(CreateInventoryItemRequestDto request)
    {
        // Validate product exists
        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product == null)
        {
            throw new EntityNotFoundException("Product", request.ProductId);
        }

        // Validate store exists
        var store = await _storeRepository.GetByIdAsync(request.StoreId);
        if (store == null)
        {
            throw new EntityNotFoundException("Store", request.StoreId);
        }

        // Check if inventory item already exists for this product and store
        var existing = await _inventoryItemRepository.GetByProductIdAndStoreIdAsync(request.ProductId, request.StoreId);
        if (existing != null)
        {
            throw new System.InvalidOperationException("Inventory item already exists for this product and store");
        }

        var inventoryItem = _mapper.Map<InventoryItem>(request);
        inventoryItem.Id = Guid.NewGuid();
        inventoryItem.UpdatedAt = DateTime.UtcNow;

        var created = await _inventoryItemRepository.CreateAsync(inventoryItem);
        _logger.LogInformation("Created inventory item {InventoryItemId} for product {ProductId} in store {StoreId}", 
            created.Id, request.ProductId, request.StoreId);

        return await MapToResponseDto(created);
    }

    public async Task<InventoryItemResponseDto> GetInventoryItemByIdAsync(Guid id)
    {
        var item = await _inventoryItemRepository.GetByIdAsync(id);
        if (item == null)
        {
            throw new EntityNotFoundException("InventoryItem", id);
        }

        return await MapToResponseDto(item);
    }

    public async Task<IEnumerable<InventoryItemResponseDto>> GetInventoryItemsByStoreIdAsync(Guid storeId)
    {
        var items = await _inventoryItemRepository.GetByStoreIdAsync(storeId);
        var result = new List<InventoryItemResponseDto>();

        foreach (var item in items)
        {
            result.Add(await MapToResponseDto(item));
        }

        return result;
    }

    public async Task<IEnumerable<InventoryItemResponseDto>> GetInventoryItemsByProductIdAsync(Guid productId)
    {
        var items = await _inventoryItemRepository.GetByProductIdAsync(productId);
        var result = new List<InventoryItemResponseDto>();

        foreach (var item in items)
        {
            result.Add(await MapToResponseDto(item));
        }

        return result;
    }

    public async Task<IEnumerable<InventoryItemResponseDto>> GetLowStockItemsAsync(Guid storeId)
    {
        var items = await _inventoryItemRepository.GetLowStockItemsAsync(storeId);
        var result = new List<InventoryItemResponseDto>();

        foreach (var item in items)
        {
            result.Add(await MapToResponseDto(item));
        }

        return result;
    }

    public async Task<InventoryItemResponseDto> UpdateInventoryItemAsync(Guid id, UpdateInventoryItemRequestDto request)
    {
        var item = await _inventoryItemRepository.GetByIdAsync(id);
        if (item == null)
        {
            throw new EntityNotFoundException("InventoryItem", id);
        }

        if (request.QuantityOnHand.HasValue)
            item.UpdateQuantity(request.QuantityOnHand.Value);

        if (request.ReorderLevel.HasValue)
            item.UpdateReorderLevel(request.ReorderLevel.Value);

        if (request.Location != null)
            item.UpdateLocation(request.Location);

        await _inventoryItemRepository.UpdateAsync(item);
        return await MapToResponseDto(item);
    }

    public async Task<InventoryItemResponseDto> AddQuantityAsync(Guid id, int quantity)
    {
        var item = await _inventoryItemRepository.GetByIdAsync(id);
        if (item == null)
        {
            throw new EntityNotFoundException("InventoryItem", id);
        }

        item.AddQuantity(quantity);
        await _inventoryItemRepository.UpdateAsync(item);
        return await MapToResponseDto(item);
    }

    public async Task<InventoryItemResponseDto> RemoveQuantityAsync(Guid id, int quantity)
    {
        var item = await _inventoryItemRepository.GetByIdAsync(id);
        if (item == null)
        {
            throw new EntityNotFoundException("InventoryItem", id);
        }

        item.RemoveQuantity(quantity);
        await _inventoryItemRepository.UpdateAsync(item);
        return await MapToResponseDto(item);
    }

    public async Task<bool> DeleteInventoryItemAsync(Guid id)
    {
        return await _inventoryItemRepository.DeleteAsync(id);
    }

    private async Task<InventoryItemResponseDto> MapToResponseDto(InventoryItem item)
    {
        var dto = _mapper.Map<InventoryItemResponseDto>(item);
        
        if (item.Product != null)
        {
            dto.Product = _mapper.Map<ProductResponseDto>(item.Product);
        }

        dto.IsInStock = item.IsInStock();
        dto.IsBelowReorderLevel = item.IsBelowReorderLevel();
        dto.SerialCount = item.Serials?.Count ?? 0;
        dto.QuantityReserved = item.QuantityReserved;
        dto.QuantityAvailable = item.GetQuantityAvailable();

        return dto;
    }

    public async Task<InventoryItemResponseDto> ReserveQuantityAsync(Guid id, int quantity)
    {
        var item = await _inventoryItemRepository.GetByIdAsync(id);
        if (item == null)
        {
            throw new EntityNotFoundException("InventoryItem", id);
        }

        item.ReserveQuantity(quantity);
        await _inventoryItemRepository.UpdateAsync(item);
        return await MapToResponseDto(item);
    }

    public async Task<InventoryItemResponseDto> ReleaseReservedQuantityAsync(Guid id, int quantity)
    {
        var item = await _inventoryItemRepository.GetByIdAsync(id);
        if (item == null)
        {
            throw new EntityNotFoundException("InventoryItem", id);
        }

        item.ReleaseReservedQuantity(quantity);
        await _inventoryItemRepository.UpdateAsync(item);
        return await MapToResponseDto(item);
    }
}

