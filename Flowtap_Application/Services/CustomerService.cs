using AutoMapper;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.BoundedContexts.Sales.Interfaces;
using Flowtap_Domain.BoundedContexts.Sales.Entities;
using Flowtap_Domain.BoundedContexts.Store.Interfaces;
using Flowtap_Domain.BoundedContexts.Identity.Interfaces;
using Flowtap_Domain.SharedKernel.Enums;
using Flowtap_Application.DtoModel;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;
using Flowtap_Domain.Exceptions;
using Flowtap_Domain.SharedKernel.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Flowtap_Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IStoreRepository _storeRepository;
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(
        ICustomerRepository customerRepository,
        IStoreRepository storeRepository,
        IUserAccountRepository userAccountRepository,
        IMapper mapper,
        ILogger<CustomerService> logger)
    {
        _customerRepository = customerRepository;
        _storeRepository = storeRepository;
        _userAccountRepository = userAccountRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CustomerResponseDto> CreateCustomerAsync(CreateCustomerRequestDto request)
    {
        // Validate store exists
        var store = await _storeRepository.GetByIdAsync(request.StoreId);
        if (store == null)
        {
            throw new EntityNotFoundException("Store", request.StoreId);
        }

        // Check if email already exists for this store (optional, but recommended for uniqueness)
        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var existingCustomer = await _customerRepository.GetByEmailAsync(request.Email);
            if (existingCustomer != null && existingCustomer.StoreId == request.StoreId)
            {
                throw new System.InvalidOperationException($"Customer with email {request.Email} already exists in this store");
            }
        }

        // Map DTO to entity
        var customer = _mapper.Map<Customer>(request);
        customer.Id = Guid.NewGuid();
        customer.Status = CustomerStatus.Active; // Default status
        customer.CreatedAt = DateTime.UtcNow;
        customer.UpdatedAt = DateTime.UtcNow;

        // Map address if provided
        if (request.Address != null &&
            !string.IsNullOrWhiteSpace(request.Address.StreetNumber) &&
            !string.IsNullOrWhiteSpace(request.Address.StreetName) &&
            !string.IsNullOrWhiteSpace(request.Address.City) &&
            !string.IsNullOrWhiteSpace(request.Address.State) &&
            !string.IsNullOrWhiteSpace(request.Address.PostalCode))
        {
            customer.Address = new Address(
                request.Address.StreetNumber,
                request.Address.StreetName,
                request.Address.City,
                request.Address.State,
                request.Address.PostalCode);
        }

        var createdCustomer = await _customerRepository.CreateAsync(customer);
        _logger.LogInformation("Created customer {CustomerId} for store {StoreId}", createdCustomer.Id, request.StoreId);

        return _mapper.Map<CustomerResponseDto>(createdCustomer);
    }

    public async Task<CustomerResponseDto> GetCustomerByIdAsync(Guid id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
        {
            throw new EntityNotFoundException("Customer", id);
        }

        return _mapper.Map<CustomerResponseDto>(customer);
    }

    public async Task<IEnumerable<CustomerResponseDto>> GetAllCustomersAsync()
    {
        var customers = await _customerRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<CustomerResponseDto>>(customers);
    }

    public async Task<IEnumerable<CustomerResponseDto>> GetCustomersByUserAccountIdAsync(Guid userAccountId)
    {
        // Get UserAccount to find AppUserId
        var userAccount = await _userAccountRepository.GetByIdAsync(userAccountId);
        if (userAccount == null)
        {
            throw new EntityNotFoundException("UserAccount", userAccountId);
        }

        if (!userAccount.AppUserId.HasValue)
        {
            // UserAccount is not associated with an AppUser, return empty list
            _logger.LogInformation("UserAccount {UserAccountId} is not associated with an AppUser, returning empty customer list", userAccountId);
            return new List<CustomerResponseDto>();
        }

        // Get all stores for this AppUser
        var stores = await _storeRepository.GetByAppUserIdAsync(userAccount.AppUserId.Value);
        var storeIds = stores.Select(s => s.Id).ToList();

        if (!storeIds.Any())
        {
            // No stores found for this AppUser, return empty list
            _logger.LogInformation("No stores found for UserAccount {UserAccountId} (AppUser {AppUserId}), returning empty customer list", 
                userAccountId, userAccount.AppUserId.Value);
            return new List<CustomerResponseDto>();
        }

        // Get all customers for this AppUser's stores in a single query
        var customers = await _customerRepository.GetByStoreIdsAsync(storeIds);
        _logger.LogInformation("Retrieved {Count} customers for UserAccount {UserAccountId} (AppUser {AppUserId}) across {StoreCount} stores", 
            customers.Count(), userAccountId, userAccount.AppUserId.Value, storeIds.Count);

        return _mapper.Map<IEnumerable<CustomerResponseDto>>(customers);
    }

    public async Task<IEnumerable<CustomerResponseDto>> GetCustomersByStoreIdAsync(Guid storeId)
    {
        // Validate store exists
        var store = await _storeRepository.GetByIdAsync(storeId);
        if (store == null)
        {
            throw new EntityNotFoundException("Store", storeId);
        }

        var customers = await _customerRepository.GetByStoreIdAsync(storeId);
        return _mapper.Map<IEnumerable<CustomerResponseDto>>(customers);
    }

    public async Task<IEnumerable<CustomerResponseDto>> SearchCustomersAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            throw new ArgumentException("Search term cannot be empty", nameof(searchTerm));
        }

        var customers = await _customerRepository.SearchAsync(searchTerm);
        return _mapper.Map<IEnumerable<CustomerResponseDto>>(customers);
    }

    public async Task<IEnumerable<CustomerResponseDto>> GetCustomersByStatusAsync(string status)
    {
        var customers = await _customerRepository.GetByStatusAsync(status);
        return _mapper.Map<IEnumerable<CustomerResponseDto>>(customers);
    }

    public async Task<CustomerResponseDto> UpdateCustomerAsync(Guid id, UpdateCustomerRequestDto request)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
        {
            throw new EntityNotFoundException("Customer", id);
        }

        // Update name if provided
        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            customer.Name = request.Name;
            customer.UpdatedAt = DateTime.UtcNow;
        }

        // Update contact info using domain method
        if (!string.IsNullOrWhiteSpace(request.Email) || !string.IsNullOrWhiteSpace(request.Phone))
        {
            customer.UpdateContactInfo(request.Email, request.Phone);
        }

        // Update address if provided
        if (request.Address != null &&
            !string.IsNullOrWhiteSpace(request.Address.StreetNumber) &&
            !string.IsNullOrWhiteSpace(request.Address.StreetName) &&
            !string.IsNullOrWhiteSpace(request.Address.City) &&
            !string.IsNullOrWhiteSpace(request.Address.State) &&
            !string.IsNullOrWhiteSpace(request.Address.PostalCode))
        {
            var address = new Address(
                request.Address.StreetNumber,
                request.Address.StreetName,
                request.Address.City,
                request.Address.State,
                request.Address.PostalCode);
            customer.UpdateAddress(address);
        }

        // Update status if provided
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            if (Enum.TryParse<CustomerStatus>(request.Status, true, out var status))
            {
                customer.UpdateStatus(status);
            }
            else
            {
                throw new System.InvalidOperationException($"Invalid customer status: {request.Status}");
            }
        }

        var updatedCustomer = await _customerRepository.UpdateAsync(customer);
        _logger.LogInformation("Updated customer {CustomerId}", id);

        return _mapper.Map<CustomerResponseDto>(updatedCustomer);
    }

    public async Task<bool> DeleteCustomerAsync(Guid id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
        {
            throw new EntityNotFoundException("Customer", id);
        }

        var result = await _customerRepository.DeleteAsync(id);
        _logger.LogInformation("Deleted customer {CustomerId}", id);

        return result;
    }

    public async Task<CustomerResponseDto> MarkCustomerAsVipAsync(Guid id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
        {
            throw new EntityNotFoundException("Customer", id);
        }

        customer.MarkAsVip();
        var updatedCustomer = await _customerRepository.UpdateAsync(customer);
        _logger.LogInformation("Marked customer {CustomerId} as VIP", id);

        return _mapper.Map<CustomerResponseDto>(updatedCustomer);
    }
}

