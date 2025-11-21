using AutoMapper;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.BoundedContexts.HR.Interfaces;
using Flowtap_Domain.BoundedContexts.HR.Entities;
using Flowtap_Domain.BoundedContexts.Store.Interfaces;
using Flowtap_Application.DtoModel;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;
using Flowtap_Domain.Exceptions;
using Flowtap_Domain.SharedKernel.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Flowtap_Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IStoreRepository _storeRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<EmployeeService> _logger;

    public EmployeeService(
        IEmployeeRepository employeeRepository,
        IStoreRepository storeRepository,
        IMapper mapper,
        ILogger<EmployeeService> logger)
    {
        _employeeRepository = employeeRepository;
        _storeRepository = storeRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<EmployeeResponseDto> CreateEmployeeAsync(CreateEmployeeRequestDto request)
    {
        // Validate store exists
        var store = await _storeRepository.GetByIdAsync(request.StoreId);
        if (store == null)
        {
            throw new EntityNotFoundException("Store", request.StoreId);
        }

        // Check if email already exists for this store
        var existingEmployee = await _employeeRepository.GetByEmailAsync(request.Email);
        if (existingEmployee != null && existingEmployee.StoreId == request.StoreId)
        {
            throw new System.InvalidOperationException($"Employee with email {request.Email} already exists in this store");
        }

        // Check if employee code already exists (if provided)
        if (!string.IsNullOrWhiteSpace(request.EmployeeCode))
        {
            var existingCode = await _employeeRepository.GetByEmployeeCodeAsync(request.EmployeeCode);
            if (existingCode != null)
            {
                throw new System.InvalidOperationException($"Employee with code {request.EmployeeCode} already exists");
            }
        }

        // Map DTO to entity
        var employee = _mapper.Map<Employee>(request);
        employee.Id = Guid.NewGuid();
        employee.IsActive = true;
        employee.CreatedAt = DateTime.UtcNow;
        employee.UpdatedAt = DateTime.UtcNow;

        // Map address if provided
        if (request.Address != null && 
            !string.IsNullOrWhiteSpace(request.Address.StreetNumber) &&
            !string.IsNullOrWhiteSpace(request.Address.StreetName) &&
            !string.IsNullOrWhiteSpace(request.Address.City) &&
            !string.IsNullOrWhiteSpace(request.Address.State) &&
            !string.IsNullOrWhiteSpace(request.Address.PostalCode))
        {
            employee.Address = new Address(
                request.Address.StreetNumber,
                request.Address.StreetName,
                request.Address.City,
                request.Address.State,
                request.Address.PostalCode);
        }

        var createdEmployee = await _employeeRepository.AddAsync(employee);
        _logger.LogInformation("Created employee {EmployeeId} for store {StoreId}", createdEmployee.Id, request.StoreId);

        return _mapper.Map<EmployeeResponseDto>(createdEmployee);
    }

    public async Task<EmployeeResponseDto> GetEmployeeByIdAsync(Guid id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
        {
            throw new EntityNotFoundException("Employee", id);
        }

        return _mapper.Map<EmployeeResponseDto>(employee);
    }

    public async Task<IEnumerable<EmployeeResponseDto>> GetAllEmployeesAsync()
    {
        var employees = await _employeeRepository.GetByStoreIdAsync(Guid.Empty); // This needs to be fixed - should get all
        // For now, we'll need to add a method to get all employees, but for this use case, we focus on store-based
        return _mapper.Map<IEnumerable<EmployeeResponseDto>>(employees);
    }

    public async Task<IEnumerable<EmployeeResponseDto>> GetEmployeesByStoreIdAsync(Guid storeId, string? role = null)
    {
        // Validate store exists
        var store = await _storeRepository.GetByIdAsync(storeId);
        if (store == null)
        {
            throw new EntityNotFoundException("Store", storeId);
        }

        IEnumerable<Employee> employees;

        if (string.IsNullOrWhiteSpace(role) || role.ToLower() == "all")
        {
            // Get all employees for the store
            employees = await _employeeRepository.GetByStoreIdAsync(storeId);
        }
        else
        {
            // Get employees by store and filter by role
            var allEmployees = await _employeeRepository.GetByStoreIdAsync(storeId);
            employees = allEmployees.Where(e => e.Role == role);
        }

        return _mapper.Map<IEnumerable<EmployeeResponseDto>>(employees);
    }

    public async Task<IEnumerable<EmployeeResponseDto>> GetEmployeesByRoleAsync(string role)
    {
        var employees = await _employeeRepository.GetByRoleAsync(role);
        return _mapper.Map<IEnumerable<EmployeeResponseDto>>(employees);
    }

    public async Task<IEnumerable<EmployeeResponseDto>> GetActiveEmployeesAsync()
    {
        // This needs a repository method to get all active employees
        // For now, we'll get all and filter
        var allEmployees = await _employeeRepository.GetByStoreIdAsync(Guid.Empty); // This needs to be fixed
        var activeEmployees = allEmployees.Where(e => e.IsActive);
        return _mapper.Map<IEnumerable<EmployeeResponseDto>>(activeEmployees);
    }

    public async Task<EmployeeResponseDto> UpdateEmployeeAsync(Guid id, UpdateEmployeeRequestDto request)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
        {
            throw new EntityNotFoundException("Employee", id);
        }

        // Update properties
        if (!string.IsNullOrWhiteSpace(request.FullName))
            employee.FullName = request.FullName;

        if (!string.IsNullOrWhiteSpace(request.Role))
            employee.UpdateRole(request.Role);

        if (request.HourlyRate.HasValue)
            employee.UpdateHourlyRate(request.HourlyRate.Value);

        if (request.Address != null && 
            !string.IsNullOrWhiteSpace(request.Address.StreetNumber) &&
            !string.IsNullOrWhiteSpace(request.Address.StreetName) &&
            !string.IsNullOrWhiteSpace(request.Address.City) &&
            !string.IsNullOrWhiteSpace(request.Address.State) &&
            !string.IsNullOrWhiteSpace(request.Address.PostalCode))
        {
            employee.UpdateAddress(new Address(
                request.Address.StreetNumber,
                request.Address.StreetName,
                request.Address.City,
                request.Address.State,
                request.Address.PostalCode));
        }

        if (!string.IsNullOrWhiteSpace(request.EmergencyContactName) && !string.IsNullOrWhiteSpace(request.EmergencyContactPhone))
        {
            employee.UpdateEmergencyContact(request.EmergencyContactName, request.EmergencyContactPhone);
        }

        if (request.IsActive.HasValue)
        {
            if (request.IsActive.Value)
                employee.Activate();
            else
                employee.Deactivate();
        }

        await _employeeRepository.UpdateAsync(employee);
        _logger.LogInformation("Updated employee {EmployeeId}", id);

        return _mapper.Map<EmployeeResponseDto>(employee);
    }

    public async Task<EmployeeResponseDto> ActivateEmployeeAsync(Guid id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
        {
            throw new EntityNotFoundException("Employee", id);
        }

        employee.Activate();
        await _employeeRepository.UpdateAsync(employee);
        _logger.LogInformation("Activated employee {EmployeeId}", id);

        return _mapper.Map<EmployeeResponseDto>(employee);
    }

    public async Task<EmployeeResponseDto> DeactivateEmployeeAsync(Guid id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
        {
            throw new EntityNotFoundException("Employee", id);
        }

        employee.Deactivate();
        await _employeeRepository.UpdateAsync(employee);
        _logger.LogInformation("Deactivated employee {EmployeeId}", id);

        return _mapper.Map<EmployeeResponseDto>(employee);
    }

    public async Task<bool> DeleteEmployeeAsync(Guid id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
        {
            throw new EntityNotFoundException("Employee", id);
        }

        await _employeeRepository.DeleteAsync(id);
        _logger.LogInformation("Deleted employee {EmployeeId}", id);

        return true;
    }

    public async Task<EmployeeResponseDto> AddPartnerAsync(Guid ownerAppUserId, AddPartnerRequestDto request)
    {
        // Validate store exists
        var store = await _storeRepository.GetByIdAsync(request.StoreId);
        if (store == null)
        {
            throw new EntityNotFoundException("Store", request.StoreId);
        }

        // Check if email already exists
        var existingEmployee = await _employeeRepository.GetByEmailAsync(request.Email);
        if (existingEmployee != null && existingEmployee.StoreId == request.StoreId)
        {
            throw new System.InvalidOperationException($"Employee with email {request.Email} already exists in this store");
        }

        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            StoreId = request.StoreId,
            FullName = request.FullName,
            Email = request.Email,
            Role = request.Role,
            LinkedAppUserId = request.LinkedAppUserId ?? ownerAppUserId,
            IsActive = true,
            CanSwitchRole = true, // Partners can switch roles
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        if (request.Address != null && 
            !string.IsNullOrWhiteSpace(request.Address.StreetNumber) &&
            !string.IsNullOrWhiteSpace(request.Address.StreetName) &&
            !string.IsNullOrWhiteSpace(request.Address.City) &&
            !string.IsNullOrWhiteSpace(request.Address.State) &&
            !string.IsNullOrWhiteSpace(request.Address.PostalCode))
        {
            employee.Address = new Address(
                request.Address.StreetNumber,
                request.Address.StreetName,
                request.Address.City,
                request.Address.State,
                request.Address.PostalCode);
        }

        var createdEmployee = await _employeeRepository.AddAsync(employee);
        _logger.LogInformation("Added partner {EmployeeId} to store {StoreId}", createdEmployee.Id, request.StoreId);

        return _mapper.Map<EmployeeResponseDto>(createdEmployee);
    }
}

