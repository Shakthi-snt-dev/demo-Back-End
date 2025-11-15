using Flowtap_Application.Interfaces;
using Flowtap_Domain.BoundedContexts.HR.Entities;
using Flowtap_Domain.BoundedContexts.HR.Interfaces;
using Flowtap_Domain.DtoModel;
using Flowtap_Domain.Exceptions;
using Flowtap_Domain.SharedKernel.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Flowtap_Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ILogger<EmployeeService> _logger;

    public EmployeeService(
        IEmployeeRepository employeeRepository,
        ILogger<EmployeeService> logger)
    {
        _employeeRepository = employeeRepository;
        _logger = logger;
    }

    public async Task<EmployeeResponseDto> CreateEmployeeAsync(CreateEmployeeRequestDto request)
    {
        // Check if email already exists
        var existingEmployee = await _employeeRepository.GetByEmailAsync(request.Email);
        if (existingEmployee != null)
            throw new Flowtap_Domain.Exceptions.InvalidOperationException(
                $"Employee with email {request.Email} already exists",
                "Employee",
                new Dictionary<string, string> { { "Email", request.Email } });

        // Check if employee code already exists
        if (!string.IsNullOrWhiteSpace(request.EmployeeCode))
        {
            var existingCode = await _employeeRepository.GetByEmployeeCodeAsync(request.EmployeeCode);
            if (existingCode != null)
                throw new Flowtap_Domain.Exceptions.InvalidOperationException(
                    $"Employee code {request.EmployeeCode} already exists",
                    "Employee",
                    new Dictionary<string, string> { { "EmployeeCode", request.EmployeeCode } });
        }

        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            StoreId = request.StoreId,
            FullName = request.FullName,
            Email = request.Email,
            Role = request.Role,
            EmployeeCode = request.EmployeeCode,
            HourlyRate = request.HourlyRate,
            EmergencyContactName = request.EmergencyContactName,
            EmergencyContactPhone = request.EmergencyContactPhone,
            LinkedAppUserId = request.LinkedAppUserId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        if (request.Address != null)
        {
            employee.Address = new Address(
                request.Address.StreetNumber,
                request.Address.StreetName,
                request.Address.City,
                request.Address.State,
                request.Address.PostalCode);
        }

        var created = await _employeeRepository.AddAsync(employee);
        return MapToDto(created);
    }

    public async Task<EmployeeResponseDto> GetEmployeeByIdAsync(Guid id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
            throw new EntityNotFoundException("Employee", id);

        return MapToDto(employee);
    }

    public async Task<IEnumerable<EmployeeResponseDto>> GetAllEmployeesAsync()
    {
        var employees = await _employeeRepository.GetByStoreIdAsync(Guid.Empty); // This needs to be fixed
        // For now, we'll need to get all employees differently
        // This is a limitation - we need a GetAll method in repository
        return new List<EmployeeResponseDto>();
    }

    public async Task<IEnumerable<EmployeeResponseDto>> GetEmployeesByStoreIdAsync(Guid storeId)
    {
        var employees = await _employeeRepository.GetByStoreIdAsync(storeId);
        return employees.Select(MapToDto);
    }

    public async Task<IEnumerable<EmployeeResponseDto>> GetEmployeesByRoleAsync(string role)
    {
        var employees = await _employeeRepository.GetByRoleAsync(role);
        return employees.Select(MapToDto);
    }

    public async Task<IEnumerable<EmployeeResponseDto>> GetActiveEmployeesAsync()
    {
        // This needs a repository method - for now return empty
        return new List<EmployeeResponseDto>();
    }

    public async Task<EmployeeResponseDto> UpdateEmployeeAsync(Guid id, UpdateEmployeeRequestDto request)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
            throw new EntityNotFoundException("Employee", id);

        if (!string.IsNullOrWhiteSpace(request.FullName))
            employee.FullName = request.FullName;

        if (!string.IsNullOrWhiteSpace(request.Role))
            employee.UpdateRole(request.Role);

        if (request.HourlyRate.HasValue)
            employee.UpdateHourlyRate(request.HourlyRate.Value);

        if (request.Address != null)
        {
            var address = new Address(
                request.Address.StreetNumber,
                request.Address.StreetName,
                request.Address.City,
                request.Address.State,
                request.Address.PostalCode);
            employee.UpdateAddress(address);
        }

        if (!string.IsNullOrWhiteSpace(request.EmergencyContactName))
            employee.UpdateEmergencyContact(request.EmergencyContactName, request.EmergencyContactPhone);

        if (request.IsActive.HasValue)
        {
            if (request.IsActive.Value)
                employee.Activate();
            else
                employee.Deactivate();
        }

        await _employeeRepository.UpdateAsync(employee);
        return MapToDto(employee);
    }

    public async Task<EmployeeResponseDto> ActivateEmployeeAsync(Guid id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
            throw new EntityNotFoundException("Employee", id);

        employee.Activate();
        await _employeeRepository.UpdateAsync(employee);
        return MapToDto(employee);
    }

    public async Task<EmployeeResponseDto> DeactivateEmployeeAsync(Guid id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
            throw new EntityNotFoundException("Employee", id);

        employee.Deactivate();
        await _employeeRepository.UpdateAsync(employee);
        return MapToDto(employee);
    }

    public async Task<bool> DeleteEmployeeAsync(Guid id)
    {
        await _employeeRepository.DeleteAsync(id);
        return true;
    }

    private static EmployeeResponseDto MapToDto(Employee employee)
    {
        return new EmployeeResponseDto
        {
            Id = employee.Id,
            StoreId = employee.StoreId,
            FullName = employee.FullName,
            Email = employee.Email,
            Role = employee.Role,
            EmployeeCode = employee.EmployeeCode,
            HourlyRate = employee.HourlyRate,
            Address = employee.Address != null
                ? new AddressDto
                {
                    StreetNumber = employee.Address.StreetNumber,
                    StreetName = employee.Address.StreetName,
                    City = employee.Address.City,
                    State = employee.Address.State,
                    PostalCode = employee.Address.PostalCode
                }
                : null,
            EmergencyContactName = employee.EmergencyContactName,
            EmergencyContactPhone = employee.EmergencyContactPhone,
            LinkedAppUserId = employee.LinkedAppUserId,
            IsActive = employee.IsActive,
            CreatedAt = employee.CreatedAt,
            UpdatedAt = employee.UpdatedAt
        };
    }
}

