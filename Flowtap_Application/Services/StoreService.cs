using Flowtap_Application.Interfaces;
using Flowtap_Domain.BoundedContexts.Store.Entities;
using Flowtap_Domain.BoundedContexts.Store.Interfaces;
using Flowtap_Domain.BoundedContexts.Owner.Interfaces;
using Flowtap_Domain.DtoModel;
using Flowtap_Domain.Exceptions;
using Flowtap_Domain.SharedKernel.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Flowtap_Application.Services;

public class StoreService : IStoreService
{
    private readonly IStoreRepository _storeRepository;
    private readonly IAppUserRepository _appUserRepository;
    private readonly ILogger<StoreService> _logger;

    public StoreService(
        IStoreRepository storeRepository,
        IAppUserRepository appUserRepository,
        ILogger<StoreService> logger)
    {
        _storeRepository = storeRepository;
        _appUserRepository = appUserRepository;
        _logger = logger;
    }

    public async Task<StoreResponseDto> CreateStoreAsync(CreateStoreRequestDto request)
    {
        // Verify AppUser exists
        var appUser = await _appUserRepository.GetByIdAsync(request.AppUserId);
        if (appUser == null)
            throw new EntityNotFoundException("AppUser", request.AppUserId);

        var store = new Store
        {
            Id = Guid.NewGuid(),
            AppUserId = request.AppUserId,
            StoreName = request.StoreName,
            StoreType = request.StoreType,
            StoreCategory = request.StoreCategory,
            Phone = request.Phone,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        if (request.Address != null)
        {
            var address = new Address(
                request.Address.StreetNumber,
                request.Address.StreetName,
                request.Address.City,
                request.Address.State,
                request.Address.PostalCode);
            store.UpdateAddress(address);
        }

        store.UpdateSettings(
            request.EnablePOS ?? true,
            request.EnableInventory ?? true,
            request.TimeZone ?? "UTC");

        store.Settings.Id = Guid.NewGuid();
        store.Settings.StoreId = store.Id;
        store.Settings.CreatedAt = DateTime.UtcNow;

        await _storeRepository.AddAsync(store);

        // Link store to app user
        appUser.AddStore(store.Id);
        await _appUserRepository.UpdateAsync(appUser);

        return MapToDto(store);
    }

    public async Task<StoreResponseDto> GetStoreByIdAsync(Guid id)
    {
        var store = await _storeRepository.GetByIdAsync(id);
        if (store == null)
            throw new EntityNotFoundException("Store", id);

        return MapToDto(store);
    }

    public async Task<IEnumerable<StoreResponseDto>> GetAllStoresAsync()
    {
        // Note: This method requires appUserId to be passed
        // For now, return empty list - use GetStoresByAppUserIdAsync instead
        await Task.CompletedTask;
        return new List<StoreResponseDto>();
    }

    public async Task<IEnumerable<StoreResponseDto>> GetStoresByAppUserIdAsync(Guid appUserId)
    {
        var stores = await _storeRepository.GetByAppUserIdAsync(appUserId);
        return stores.Select(MapToDto);
    }

    public async Task<IEnumerable<StoreResponseDto>> GetStoresByTypeAsync(string storeType)
    {
        var stores = await _storeRepository.GetByStoreTypeAsync(storeType);
        return stores.Select(MapToDto);
    }

    public async Task<StoreResponseDto> UpdateStoreAsync(Guid id, UpdateStoreRequestDto request)
    {
        var store = await _storeRepository.GetByIdAsync(id);
        if (store == null)
            throw new EntityNotFoundException("Store", id);

        if (!string.IsNullOrWhiteSpace(request.StoreName))
            store.StoreName = request.StoreName;

        if (request.StoreType != null)
            store.StoreType = request.StoreType;

        if (request.StoreCategory != null)
            store.StoreCategory = request.StoreCategory;

        if (request.Phone != null)
            store.Phone = request.Phone;

        if (request.Address != null)
        {
            var address = new Address(
                request.Address.StreetNumber,
                request.Address.StreetName,
                request.Address.City,
                request.Address.State,
                request.Address.PostalCode);
            store.UpdateAddress(address);
        }

        if (request.EnablePOS.HasValue || request.EnableInventory.HasValue || !string.IsNullOrWhiteSpace(request.TimeZone))
        {
            store.UpdateSettings(
                request.EnablePOS ?? store.Settings.EnablePOS,
                request.EnableInventory ?? store.Settings.EnableInventory,
                request.TimeZone ?? store.Settings.TimeZone);
        }

        store.UpdatedAt = DateTime.UtcNow;
        await _storeRepository.UpdateAsync(store);

        return MapToDto(store);
    }

    public async Task<bool> DeleteStoreAsync(Guid id)
    {
        var store = await _storeRepository.GetByIdAsync(id);
        if (store == null)
            throw new EntityNotFoundException("Store", id);

        await _storeRepository.DeleteAsync(id);
        return true;
    }

    public async Task<IEnumerable<StoreTypeResponseDto>> GetStoreTypesAsync()
    {
        // Get all stores to extract unique store types
        var allStores = await _storeRepository.GetByAppUserIdAsync(Guid.Empty);
        // This is a workaround - ideally we'd have a StoreType entity
        var storeTypes = new HashSet<string>();
        var stores = await _storeRepository.GetByAppUserIdAsync(Guid.Empty);
        
        // For now, return common store types
        return new List<StoreTypeResponseDto>
        {
            new StoreTypeResponseDto { Name = "Company Owned", Description = "Company owned store", StoreCount = 0 },
            new StoreTypeResponseDto { Name = "Franchise", Description = "Franchise store", StoreCount = 0 },
            new StoreTypeResponseDto { Name = "Partner", Description = "Partner store", StoreCount = 0 }
        };
    }

    public async Task<StoreTypeResponseDto> CreateStoreTypeAsync(CreateStoreTypeRequestDto request)
    {
        // Store types are currently just strings in the Store entity
        // In a full implementation, you'd have a StoreType entity
        await Task.CompletedTask;
        return new StoreTypeResponseDto
        {
            Name = request.Name,
            Description = request.Description,
            StoreCount = 0
        };
    }

    public async Task<bool> DeleteStoreTypeAsync(string name)
    {
        // Check if any stores use this type
        var stores = await _storeRepository.GetByStoreTypeAsync(name);
        if (stores.Any())
            throw new System.InvalidOperationException($"Cannot delete store type '{name}' because it is in use by {stores.Count()} store(s)");

        await Task.CompletedTask;
        return true;
    }

    private StoreResponseDto MapToDto(Store store)
    {
        return new StoreResponseDto
        {
            Id = store.Id,
            AppUserId = store.AppUserId,
            StoreName = store.StoreName,
            StoreType = store.StoreType,
            StoreCategory = store.StoreCategory,
            Phone = store.Phone,
            Address = store.Address != null ? new AddressDto
            {
                StreetNumber = store.Address.StreetNumber,
                StreetName = store.Address.StreetName,
                City = store.Address.City,
                State = store.Address.State,
                PostalCode = store.Address.PostalCode
            } : null,
            EnablePOS = store.Settings.EnablePOS,
            EnableInventory = store.Settings.EnableInventory,
            TimeZone = store.Settings.TimeZone,
            EmployeeCount = store.EmployeeIds.Count,
            CreatedAt = store.CreatedAt,
            UpdatedAt = store.UpdatedAt
        };
    }
}

