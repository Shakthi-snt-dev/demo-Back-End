using AutoMapper;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.BoundedContexts.Procurement.Entities;
using Flowtap_Domain.BoundedContexts.Procurement.Interfaces;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;
using Flowtap_Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Flowtap_Application.Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _supplierRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<SupplierService> _logger;

    public SupplierService(
        ISupplierRepository supplierRepository,
        IMapper mapper,
        ILogger<SupplierService> logger)
    {
        _supplierRepository = supplierRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<SupplierResponseDto> CreateSupplierAsync(CreateSupplierRequestDto request)
    {
        // Check if supplier with same name exists
        var existing = await _supplierRepository.GetByNameAsync(request.Name);
        if (existing != null)
        {
            throw new System.InvalidOperationException($"Supplier with name '{request.Name}' already exists");
        }

        var supplier = _mapper.Map<Supplier>(request);
        supplier.Id = Guid.NewGuid();
        supplier.CreatedAt = DateTime.UtcNow;
        supplier.UpdatedAt = DateTime.UtcNow;

        var created = await _supplierRepository.CreateAsync(supplier);
        _logger.LogInformation("Created supplier {SupplierId}", created.Id);

        return _mapper.Map<SupplierResponseDto>(created);
    }

    public async Task<SupplierResponseDto> GetSupplierByIdAsync(Guid id)
    {
        var supplier = await _supplierRepository.GetByIdAsync(id);
        if (supplier == null)
        {
            throw new EntityNotFoundException("Supplier", id);
        }

        return _mapper.Map<SupplierResponseDto>(supplier);
    }

    public async Task<IEnumerable<SupplierResponseDto>> GetAllSuppliersAsync()
    {
        var suppliers = await _supplierRepository.GetAllAsync();
        return suppliers.Select(s => _mapper.Map<SupplierResponseDto>(s));
    }

    public async Task<IEnumerable<SupplierResponseDto>> GetActiveSuppliersAsync()
    {
        var suppliers = await _supplierRepository.GetActiveSuppliersAsync();
        return suppliers.Select(s => _mapper.Map<SupplierResponseDto>(s));
    }

    public async Task<SupplierResponseDto> UpdateSupplierAsync(Guid id, UpdateSupplierRequestDto request)
    {
        var supplier = await _supplierRepository.GetByIdAsync(id);
        if (supplier == null)
        {
            throw new EntityNotFoundException("Supplier", id);
        }

        supplier.UpdateSupplierInfo(
            request.Name ?? supplier.Name,
            request.Email,
            request.Phone,
            request.Address,
            request.ContactPerson);

        if (request.IsActive.HasValue)
        {
            if (request.IsActive.Value)
                supplier.Activate();
            else
                supplier.Deactivate();
        }

        await _supplierRepository.UpdateAsync(supplier);
        return _mapper.Map<SupplierResponseDto>(supplier);
    }

    public async Task<bool> DeleteSupplierAsync(Guid id)
    {
        return await _supplierRepository.DeleteAsync(id);
    }
}

