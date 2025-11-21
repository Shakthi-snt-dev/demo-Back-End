using AutoMapper;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.BoundedContexts.Store.Interfaces;
using Flowtap_Domain.BoundedContexts.Owner.Interfaces;
using Flowtap_Domain.BoundedContexts.Identity.Interfaces;
using Flowtap_Application.DtoModel;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;
using Flowtap_Domain.Exceptions;
using Flowtap_Domain.SharedKernel.Enums;
using Microsoft.Extensions.Logging;

namespace Flowtap_Application.Services;

public class StoreService : IStoreService
{
    private readonly IStoreRepository _storeRepository;
    private readonly IAppUserRepository _appUserRepository;
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<StoreService> _logger;

    public StoreService(
        IStoreRepository storeRepository,
        IAppUserRepository appUserRepository,
        IUserAccountRepository userAccountRepository,
        IMapper mapper,
        ILogger<StoreService> logger)
    {
        _storeRepository = storeRepository;
        _appUserRepository = appUserRepository;
        _userAccountRepository = userAccountRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<StoreListItemDto>> GetStoresForCurrentUserAsync(Guid userAccountId)
    {
        // Get UserAccount by ID
        var userAccount = await _userAccountRepository.GetByIdAsync(userAccountId);
        if (userAccount == null)
        {
            throw new EntityNotFoundException("UserAccount", userAccountId);
        }

        // Check if user has AppUserId
        if (!userAccount.AppUserId.HasValue)
        {
            throw new System.InvalidOperationException($"AppUserId not found for UserAccount {userAccountId}");
        }

        // Validate AppUser exists
        var appUser = await _appUserRepository.GetByIdAsync(userAccount.AppUserId.Value);
        if (appUser == null)
        {
            throw new EntityNotFoundException("AppUser", userAccount.AppUserId.Value);
        }

        // Get stores for this AppUser
        var stores = await _storeRepository.GetByAppUserIdAsync(userAccount.AppUserId.Value);

        // Map to simple DTO with only ID and Name
        var storeList = stores.Select(s => new StoreListItemDto
        {
            Id = s.Id,
            StoreName = s.StoreName
        }).ToList();

        _logger.LogInformation("Retrieved {Count} stores for UserAccount {UserAccountId} (AppUser {AppUserId})", 
            storeList.Count, userAccountId, userAccount.AppUserId.Value);
        return storeList;
    }

    public async Task<StoreResponseDto> CreateStoreAsync(CreateStoreRequestDto request)
    {
        // Implementation for creating store
        throw new NotImplementedException();
    }

    public async Task<StoreResponseDto> GetStoreByIdAsync(Guid id)
    {
        var store = await _storeRepository.GetByIdAsync(id);
        if (store == null)
        {
            throw new EntityNotFoundException("Store", id);
        }

        return _mapper.Map<StoreResponseDto>(store);
    }

    public async Task<IEnumerable<StoreResponseDto>> GetAllStoresAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<StoreResponseDto>> GetStoresByAppUserIdAsync(Guid appUserId)
    {
        var stores = await _storeRepository.GetByAppUserIdAsync(appUserId);
        return _mapper.Map<IEnumerable<StoreResponseDto>>(stores);
    }

    public async Task<IEnumerable<StoreResponseDto>> GetStoresByTypeAsync(string storeType)
    {
        var stores = await _storeRepository.GetByStoreTypeAsync(storeType);
        return _mapper.Map<IEnumerable<StoreResponseDto>>(stores);
    }

    public async Task<StoreResponseDto> UpdateStoreAsync(Guid id, UpdateStoreRequestDto request)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteStoreAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<StoreTypeResponseDto>> GetStoreTypesAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<StoreTypeResponseDto> CreateStoreTypeAsync(CreateStoreTypeRequestDto request)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteStoreTypeAsync(string name)
    {
        throw new NotImplementedException();
    }
}

