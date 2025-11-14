using Flowtap_Application.Interfaces;
using Flowtap_Domain.BoundedContexts.Sales.Entities;
using Flowtap_Domain.BoundedContexts.Sales.Interfaces;
using Flowtap_Domain.DtoModel;
using Flowtap_Domain.Exceptions;
using Flowtap_Domain.SharedKernel.ValueObjects;

namespace Flowtap_Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(
        ICustomerRepository customerRepository,
        ILogger<CustomerService> logger)
    {
        _customerRepository = customerRepository;
        _logger = logger;
    }

    public async Task<CustomerResponseDto> CreateCustomerAsync(CreateCustomerRequestDto request)
    {
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            StoreId = request.StoreId,
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            Address = request.Address != null
                ? new Address(
                    request.Address.StreetNumber,
                    request.Address.StreetName,
                    request.Address.City,
                    request.Address.State,
                    request.Address.PostalCode)
                : null,
            Status = "active",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _customerRepository.CreateAsync(customer);
        return MapToDto(created);
    }

    public async Task<CustomerResponseDto> GetCustomerByIdAsync(Guid id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
            throw new EntityNotFoundException("Customer", id);

        return MapToDto(customer);
    }

    public async Task<IEnumerable<CustomerResponseDto>> GetAllCustomersAsync()
    {
        var customers = await _customerRepository.GetAllAsync();
        return customers.Select(MapToDto);
    }

    public async Task<IEnumerable<CustomerResponseDto>> SearchCustomersAsync(string searchTerm)
    {
        var customers = await _customerRepository.SearchAsync(searchTerm);
        return customers.Select(MapToDto);
    }

    public async Task<IEnumerable<CustomerResponseDto>> GetCustomersByStatusAsync(string status)
    {
        var customers = await _customerRepository.GetByStatusAsync(status);
        return customers.Select(MapToDto);
    }

    public async Task<CustomerResponseDto> UpdateCustomerAsync(Guid id, UpdateCustomerRequestDto request)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
            throw new EntityNotFoundException("Customer", id);

        if (!string.IsNullOrWhiteSpace(request.Name))
            customer.Name = request.Name;

        if (request.Email != null)
            customer.UpdateContactInfo(request.Email, customer.Phone);

        if (request.Phone != null)
            customer.UpdateContactInfo(customer.Email, request.Phone);

        if (request.Address != null)
        {
            var address = new Address(
                request.Address.StreetNumber,
                request.Address.StreetName,
                request.Address.City,
                request.Address.State,
                request.Address.PostalCode);
            customer.UpdateAddress(address);
        }

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            if (request.Status == "vip")
                customer.MarkAsVip();
            else if (request.Status == "active")
                customer.MarkAsActive();
            else if (request.Status == "inactive")
                customer.MarkAsInactive();
        }

        var updated = await _customerRepository.UpdateAsync(customer);
        return MapToDto(updated);
    }

    public async Task<bool> DeleteCustomerAsync(Guid id)
    {
        return await _customerRepository.DeleteAsync(id);
    }

    public async Task<CustomerResponseDto> MarkCustomerAsVipAsync(Guid id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
            throw new EntityNotFoundException("Customer", id);

        customer.MarkAsVip();
        var updated = await _customerRepository.UpdateAsync(customer);
        return MapToDto(updated);
    }

    private static CustomerResponseDto MapToDto(Customer customer)
    {
        return new CustomerResponseDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone,
            Address = customer.Address != null
                ? new AddressDto
                {
                    StreetNumber = customer.Address.StreetNumber,
                    StreetName = customer.Address.StreetName,
                    City = customer.Address.City,
                    State = customer.Address.State,
                    PostalCode = customer.Address.PostalCode
                }
                : null,
            TotalOrders = customer.TotalOrders,
            TotalSpent = customer.TotalSpent,
            Status = customer.Status,
            CreatedAt = customer.CreatedAt,
            UpdatedAt = customer.UpdatedAt
        };
    }
}

