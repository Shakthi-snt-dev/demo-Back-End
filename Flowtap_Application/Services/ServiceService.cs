using AutoMapper;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.BoundedContexts.Service.Entities;
using Flowtap_Domain.BoundedContexts.Service.Interfaces;
using Flowtap_Domain.BoundedContexts.Store.Interfaces;
using Flowtap_Domain.BoundedContexts.Inventory.Interfaces;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;
using Flowtap_Domain.Exceptions;
using Microsoft.Extensions.Logging;
using ServiceEntity = Flowtap_Domain.BoundedContexts.Service.Entities.Service;

namespace Flowtap_Application.Services;

public class ServiceService : IServiceService
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IStoreRepository _storeRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ServiceService> _logger;

    public ServiceService(
        IServiceRepository serviceRepository,
        IStoreRepository storeRepository,
        IProductRepository productRepository,
        IMapper mapper,
        ILogger<ServiceService> logger)
    {
        _serviceRepository = serviceRepository;
        _storeRepository = storeRepository;
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ServiceResponseDto> CreateServiceAsync(CreateServiceRequestDto request)
    {
        // Validate store exists
        var store = await _storeRepository.GetByIdAsync(request.StoreId);
        if (store == null)
        {
            throw new EntityNotFoundException("Store", request.StoreId);
        }

        // Validate parts (products) exist
        if (request.Parts != null && request.Parts.Any())
        {
            foreach (var part in request.Parts)
            {
                var product = await _productRepository.GetByIdAsync(part.ProductId);
                if (product == null)
                {
                    throw new EntityNotFoundException("Product", part.ProductId);
                }
            }
        }

        var service = new ServiceEntity
        {
            Id = Guid.NewGuid(),
            StoreId = request.StoreId,
            Name = request.Name,
            Description = request.Description,
            BasePrice = request.BasePrice,
            IsDeviceSpecific = request.IsDeviceSpecific,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Add parts
        if (request.Parts != null)
        {
            foreach (var partDto in request.Parts)
            {
                service.Parts.Add(new ServicePart
                {
                    Id = Guid.NewGuid(),
                    ServiceId = service.Id,
                    ProductId = partDto.ProductId,
                    Quantity = partDto.Quantity
                });
            }
        }

        // Add labor
        if (request.Labor != null)
        {
            foreach (var laborDto in request.Labor)
            {
                service.Labor.Add(new ServiceLabor
                {
                    Id = Guid.NewGuid(),
                    ServiceId = service.Id,
                    Label = laborDto.Label,
                    Cost = laborDto.Cost
                });
            }
        }

        // Add warranties
        if (request.Warranties != null)
        {
            foreach (var warrantyDto in request.Warranties)
            {
                service.Warranties.Add(new ServiceWarranty
                {
                    Id = Guid.NewGuid(),
                    ServiceId = service.Id,
                    WarrantyDays = warrantyDto.WarrantyDays
                });
            }
        }

        var created = await _serviceRepository.CreateAsync(service);
        _logger.LogInformation("Created service {ServiceId} for store {StoreId}", created.Id, request.StoreId);

        return await MapToResponseDto(created);
    }

    public async Task<ServiceResponseDto> GetServiceByIdAsync(Guid id)
    {
        var service = await _serviceRepository.GetByIdAsync(id);
        if (service == null)
        {
            throw new EntityNotFoundException("Service", id);
        }

        return await MapToResponseDto(service);
    }

    public async Task<IEnumerable<ServiceResponseDto>> GetServicesByStoreIdAsync(Guid storeId)
    {
        var services = await _serviceRepository.GetByStoreIdAsync(storeId);
        var result = new List<ServiceResponseDto>();

        foreach (var service in services)
        {
            result.Add(await MapToResponseDto(service));
        }

        return result;
    }

    public async Task<IEnumerable<ServiceResponseDto>> GetActiveServicesAsync(Guid storeId)
    {
        var services = await _serviceRepository.GetActiveServicesAsync(storeId);
        var result = new List<ServiceResponseDto>();

        foreach (var service in services)
        {
            result.Add(await MapToResponseDto(service));
        }

        return result;
    }

    public async Task<ServiceResponseDto> UpdateServiceAsync(Guid id, UpdateServiceRequestDto request)
    {
        var service = await _serviceRepository.GetByIdAsync(id);
        if (service == null)
        {
            throw new EntityNotFoundException("Service", id);
        }

        if (!string.IsNullOrWhiteSpace(request.Name))
            service.UpdateServiceInfo(
                request.Name, 
                request.Description, 
                request.BasePrice ?? service.BasePrice, 
                request.IsDeviceSpecific ?? service.IsDeviceSpecific,
                request.TaxClass,
                request.ShowOnPOS,
                request.ShowOnWidget
            );

        if (request.IsActive.HasValue)
        {
            if (request.IsActive.Value)
                service.Activate();
            else
                service.Deactivate();
        }

        await _serviceRepository.UpdateAsync(service);
        return await MapToResponseDto(service);
    }

    public async Task<bool> DeleteServiceAsync(Guid id)
    {
        return await _serviceRepository.DeleteAsync(id);
    }

    private async Task<ServiceResponseDto> MapToResponseDto(Service service)
    {
        var dto = _mapper.Map<ServiceResponseDto>(service);
        dto.TotalCost = service.GetTotalCost();
        dto.TaxClass = service.TaxClass;
        dto.ShowOnPOS = service.ShowOnPOS;
        dto.ShowOnWidget = service.ShowOnWidget;

        // Map parts with product names
        foreach (var part in service.Parts)
        {
            var partDto = _mapper.Map<ServicePartResponseDto>(part);
            if (part.ProductId != Guid.Empty)
            {
                var product = await _productRepository.GetByIdAsync(part.ProductId);
                if (product != null)
                {
                    partDto.ProductName = product.Name;
                }
            }
            dto.Parts.Add(partDto);
        }

        return dto;
    }
}

