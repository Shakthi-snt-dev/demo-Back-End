using Flowtap_Application.Interfaces;
using Flowtap_Domain.BoundedContexts.Owner.Entities;
using Flowtap_Domain.BoundedContexts.Owner.Interfaces;
using Flowtap_Domain.BoundedContexts.HR.Entities;
using Flowtap_Domain.BoundedContexts.HR.Interfaces;
using Flowtap_Domain.BoundedContexts.Store.Interfaces;
using Flowtap_Domain.DtoModel;
using Flowtap_Domain.Exceptions;
using Flowtap_Domain.SharedKernel.ValueObjects;
using Flowtap_Domain.SharedKernel.Enums;
using Microsoft.Extensions.Logging;

namespace Flowtap_Application.Services;

public class AppUserAdminService : IAppUserAdminService
{
    private readonly IAppUserAdminRepository _appUserAdminRepository;
    private readonly IAppUserRepository _appUserRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IStoreRepository _storeRepository;
    private readonly ILogger<AppUserAdminService> _logger;

    public AppUserAdminService(
        IAppUserAdminRepository appUserAdminRepository,
        IAppUserRepository appUserRepository,
        IEmployeeRepository employeeRepository,
        IStoreRepository storeRepository,
        ILogger<AppUserAdminService> logger)
    {
        _appUserAdminRepository = appUserAdminRepository;
        _appUserRepository = appUserRepository;
        _employeeRepository = employeeRepository;
        _storeRepository = storeRepository;
        _logger = logger;
    }

    public async Task<AppUserAdminResponseDto> CreateAppUserAdminAsync(Guid appUserId, CreateAppUserAdminRequestDto request)
    {
        // Verify AppUser exists and request.AppUserId matches
        var appUser = await _appUserRepository.GetByIdAsync(appUserId);
        if (appUser == null)
            throw new EntityNotFoundException("AppUser", appUserId);

        if (request.AppUserId != appUserId)
            throw new UnauthorizedException("AppUserId mismatch");

        // Check if email already exists
        var existingAdmin = await _appUserAdminRepository.GetByEmailAsync(request.Email);
        if (existingAdmin != null)
            throw new Flowtap_Domain.Exceptions.InvalidOperationException(
                $"Admin with email {request.Email} already exists",
                "AppUserAdmin",
                new Dictionary<string, string> { { "Email", request.Email } });

        // Create AppUserAdmin
        var admin = new AppUserAdmin
        {
            Id = Guid.NewGuid(),
            AppUserId = appUserId,
            FullName = request.FullName,
            Email = request.Email,
            IsPrimaryOwner = request.IsPrimaryOwner,
            CreatedAt = DateTime.UtcNow
        };

        if (request.Address != null)
        {
            admin.Address = new Address(
                request.Address.StreetNumber,
                request.Address.StreetName,
                request.Address.City,
                request.Address.State,
                request.Address.PostalCode);
        }

        if (request.Permissions != null)
        {
            admin.Permissions = request.Permissions;
        }

        // Add admin to AppUser
        appUser.AddAdmin(admin);
        await _appUserRepository.UpdateAsync(appUser);
        await _appUserAdminRepository.AddAsync(admin);

        Guid? employeeId = null;

        // Create Employee record if requested
        if (request.CreateAsEmployee && request.StoreId.HasValue)
        {
            var store = await _storeRepository.GetByIdAsync(request.StoreId.Value);
            if (store == null)
                throw new EntityNotFoundException("Store", request.StoreId.Value);

            // Verify store belongs to this AppUser
            if (store.AppUserId != appUserId)
                throw new UnauthorizedException("Store does not belong to this AppUser");

            // Check if employee already exists for this AppUserAdmin in this store
            var existingEmployee = await _employeeRepository.GetByLinkedAppUserIdAsync(admin.Id);
            var employeeInStore = existingEmployee.FirstOrDefault(e => e.StoreId == request.StoreId.Value);
            
            if (employeeInStore == null)
            {
                var employee = new Employee
                {
                    Id = Guid.NewGuid(),
                    StoreId = request.StoreId.Value,
                    FullName = admin.FullName,
                    Email = admin.Email,
                    Role = EmployeeRole.Owner.ToString(), // Business owner also has Owner role
                    LinkedAppUserId = admin.Id, // Link to AppUserAdmin.Id to distinguish from AppUser
                    IsActive = true,
                    CanSwitchRole = true, // Business owners can switch roles
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                if (admin.Address != null)
                {
                    employee.Address = admin.Address;
                }

                await _employeeRepository.AddAsync(employee);
                employeeId = employee.Id;
                _logger.LogInformation("Created Employee record for AppUserAdmin {AdminId} as Owner in Store {StoreId}", admin.Id, request.StoreId.Value);
            }
            else
            {
                employeeId = employeeInStore.Id;
            }
        }

        return MapToDto(admin, employeeId);
    }

    public async Task<AppUserAdminResponseDto> GetAppUserAdminByIdAsync(Guid id)
    {
        var admin = await _appUserAdminRepository.GetByIdAsync(id);
        if (admin == null)
            throw new EntityNotFoundException("AppUserAdmin", id);

        // Find associated employee if exists
        var employees = await _employeeRepository.GetByLinkedAppUserIdAsync(id);
        var employeeId = employees.FirstOrDefault()?.Id;

        return MapToDto(admin, employeeId);
    }

    public async Task<IEnumerable<AppUserAdminResponseDto>> GetAppUserAdminsByAppUserIdAsync(Guid appUserId)
    {
        var admins = await _appUserAdminRepository.GetByAppUserIdAsync(appUserId);
        var result = new List<AppUserAdminResponseDto>();

        foreach (var admin in admins)
        {
            var employees = await _employeeRepository.GetByLinkedAppUserIdAsync(admin.Id);
            var employeeId = employees.FirstOrDefault()?.Id;
            result.Add(MapToDto(admin, employeeId));
        }

        return result;
    }

    public async Task<AppUserAdminResponseDto> UpdateAppUserAdminAsync(Guid id, CreateAppUserAdminRequestDto request)
    {
        var admin = await _appUserAdminRepository.GetByIdAsync(id);
        if (admin == null)
            throw new EntityNotFoundException("AppUserAdmin", id);

        if (!string.IsNullOrWhiteSpace(request.FullName))
            admin.FullName = request.FullName;

        if (request.Address != null)
        {
            admin.Address = new Address(
                request.Address.StreetNumber,
                request.Address.StreetName,
                request.Address.City,
                request.Address.State,
                request.Address.PostalCode);
        }

        if (request.Permissions != null)
        {
            admin.Permissions = request.Permissions;
        }

        await _appUserAdminRepository.UpdateAsync(admin);

        // Update employee if exists
        var employees = await _employeeRepository.GetByLinkedAppUserIdAsync(id);
        foreach (var employee in employees)
        {
            if (!string.IsNullOrWhiteSpace(request.FullName))
                employee.FullName = request.FullName;

            if (request.Address != null)
            {
                employee.Address = new Address(
                    request.Address.StreetNumber,
                    request.Address.StreetName,
                    request.Address.City,
                    request.Address.State,
                    request.Address.PostalCode);
            }

            await _employeeRepository.UpdateAsync(employee);
        }

        var employeeId = employees.FirstOrDefault()?.Id;
        return MapToDto(admin, employeeId);
    }

    public async Task<bool> DeleteAppUserAdminAsync(Guid id)
    {
        var admin = await _appUserAdminRepository.GetByIdAsync(id);
        if (admin == null)
            throw new EntityNotFoundException("AppUserAdmin", id);

        if (admin.IsPrimaryOwner)
            throw new System.InvalidOperationException("Cannot delete primary owner");

        // Delete associated employees
        var employees = await _employeeRepository.GetByLinkedAppUserIdAsync(id);
        foreach (var employee in employees)
        {
            await _employeeRepository.DeleteAsync(employee.Id);
        }

        await _appUserAdminRepository.DeleteAsync(id);
        return true;
    }

    private static AppUserAdminResponseDto MapToDto(AppUserAdmin admin, Guid? employeeId = null)
    {
        return new AppUserAdminResponseDto
        {
            Id = admin.Id,
            AppUserId = admin.AppUserId,
            FullName = admin.FullName,
            Email = admin.Email,
            IsPrimaryOwner = admin.IsPrimaryOwner,
            Address = admin.Address != null
                ? new AddressDto
                {
                    StreetNumber = admin.Address.StreetNumber,
                    StreetName = admin.Address.StreetName,
                    City = admin.Address.City,
                    State = admin.Address.State,
                    PostalCode = admin.Address.PostalCode
                }
                : null,
            Permissions = admin.Permissions?.ToList() ?? new List<string>(),
            CreatedAt = admin.CreatedAt,
            EmployeeId = employeeId
        };
    }
}

