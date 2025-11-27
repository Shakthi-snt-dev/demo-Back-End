using AutoMapper;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.BoundedContexts.Service.Entities;
using Flowtap_Domain.BoundedContexts.Service.Interfaces;
using Flowtap_Domain.BoundedContexts.Store.Interfaces;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;
using Flowtap_Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Flowtap_Application.Services;

public class PreCheckItemService : IPreCheckItemService
{
    private readonly IPreCheckItemRepository _preCheckItemRepository;
    private readonly IStoreRepository _storeRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<PreCheckItemService> _logger;

    public PreCheckItemService(
        IPreCheckItemRepository preCheckItemRepository,
        IStoreRepository storeRepository,
        IMapper mapper,
        ILogger<PreCheckItemService> logger)
    {
        _preCheckItemRepository = preCheckItemRepository;
        _storeRepository = storeRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PreCheckItemResponseDto> CreatePreCheckItemAsync(CreatePreCheckItemRequestDto request)
    {
        var store = await _storeRepository.GetByIdAsync(request.StoreId);
        if (store == null)
        {
            throw new EntityNotFoundException("Store", request.StoreId);
        }

        var item = _mapper.Map<PreCheckItem>(request);
        item.Id = Guid.NewGuid();
        item.CreatedAt = DateTime.UtcNow;
        item.UpdatedAt = DateTime.UtcNow;

        var created = await _preCheckItemRepository.CreateAsync(item);
        _logger.LogInformation("Created pre-check item {ItemId} for store {StoreId}", created.Id, request.StoreId);

        return _mapper.Map<PreCheckItemResponseDto>(created);
    }

    public async Task<PreCheckItemResponseDto> GetPreCheckItemByIdAsync(Guid id)
    {
        var item = await _preCheckItemRepository.GetByIdAsync(id);
        if (item == null)
        {
            throw new EntityNotFoundException("PreCheckItem", id);
        }

        return _mapper.Map<PreCheckItemResponseDto>(item);
    }

    public async Task<IEnumerable<PreCheckItemResponseDto>> GetPreCheckItemsByStoreIdAsync(Guid storeId)
    {
        var items = await _preCheckItemRepository.GetByStoreIdAsync(storeId);
        return items.Select(i => _mapper.Map<PreCheckItemResponseDto>(i));
    }

    public async Task<IEnumerable<PreCheckItemResponseDto>> GetActivePreCheckItemsAsync(Guid storeId)
    {
        var items = await _preCheckItemRepository.GetActiveItemsAsync(storeId);
        return items.Select(i => _mapper.Map<PreCheckItemResponseDto>(i));
    }

    public async Task<PreCheckItemResponseDto> UpdatePreCheckItemAsync(Guid id, UpdatePreCheckItemRequestDto request)
    {
        var item = await _preCheckItemRepository.GetByIdAsync(id);
        if (item == null)
        {
            throw new EntityNotFoundException("PreCheckItem", id);
        }

        if (!string.IsNullOrWhiteSpace(request.Description))
            item.UpdateDescription(request.Description);

        if (request.IsActive.HasValue)
        {
            if (request.IsActive.Value)
                item.Activate();
            else
                item.Deactivate();
        }

        if (request.DisplayOrder.HasValue)
        {
            // Note: DisplayOrder update method not in entity, but we can set it directly
            item.DisplayOrder = request.DisplayOrder.Value;
            item.UpdatedAt = DateTime.UtcNow;
        }

        await _preCheckItemRepository.UpdateAsync(item);
        return _mapper.Map<PreCheckItemResponseDto>(item);
    }

    public async Task<bool> DeletePreCheckItemAsync(Guid id)
    {
        return await _preCheckItemRepository.DeleteAsync(id);
    }
}

