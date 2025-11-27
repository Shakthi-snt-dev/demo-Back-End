using AutoMapper;
using Flowtap_Application.DtoModel;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;
using Flowtap_Domain.BoundedContexts.Identity.Entities;
using Flowtap_Domain.BoundedContexts.Owner.Entities;
using Flowtap_Domain.BoundedContexts.Store.Entities;
using Flowtap_Domain.BoundedContexts.Sales.Entities;
using Flowtap_Domain.BoundedContexts.Sales.ValueObjects;
using Flowtap_Domain.BoundedContexts.Inventory.Entities;
using Flowtap_Domain.BoundedContexts.Procurement.Entities;
using Flowtap_Domain.BoundedContexts.HR.Entities;
using Flowtap_Domain.BoundedContexts.Service.Entities;
using Flowtap_Domain.SharedKernel.ValueObjects;

namespace Flowtap_Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // UserAccount to UserInfoDto
        CreateMap<UserAccount, UserInfoDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.UserType.ToString()));

        // AppUser to AppUserProfileResponseDto (with custom mapping for address)
        CreateMap<AppUser, AppUserProfileResponseDto>()
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.StreetNumber, opt => opt.MapFrom(src => src.Address != null ? src.Address.StreetNumber : null))
            .ForMember(dest => dest.StreetName, opt => opt.MapFrom(src => src.Address != null ? src.Address.StreetName : null))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address != null ? src.Address.City : null))
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Address != null ? src.Address.State : null))
            .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.Address != null ? src.Address.PostalCode : null))
            .ForMember(dest => dest.DefaultStore, opt => opt.Ignore()) // Will be set manually
            .ForMember(dest => dest.EnableTwoFactor, opt => opt.Ignore()) // Will be set manually
            .ForMember(dest => dest.Username, opt => opt.Ignore()) // Will be set manually from UserAccount
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

        // Store to StoreResponseDto
        CreateMap<Store, StoreResponseDto>()
            .ForMember(dest => dest.StoreName, opt => opt.MapFrom(src => src.StoreName))
            .ForMember(dest => dest.StoreType, opt => opt.MapFrom(src => src.StoreType))
            .ForMember(dest => dest.StoreCategory, opt => opt.MapFrom(src => src.StoreCategory))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
            .ForMember(dest => dest.EnablePOS, opt => opt.MapFrom(src => src.Settings.EnablePOS))
            .ForMember(dest => dest.EnableInventory, opt => opt.MapFrom(src => src.Settings.EnableInventory))
            .ForMember(dest => dest.TimeZone, opt => opt.MapFrom(src => src.Settings.TimeZone))
            .ForMember(dest => dest.EmployeeCount, opt => opt.MapFrom(src => src.EmployeeIds.Count))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address != null ? new AddressDto
            {
                StreetNumber = src.Address.StreetNumber,
                StreetName = src.Address.StreetName,
                City = src.Address.City,
                State = src.Address.State,
                PostalCode = src.Address.PostalCode
            } : null));

        // Store to DefaultStoreDto
        CreateMap<Store, DefaultStoreDto>()
            .ForMember(dest => dest.StoreName, opt => opt.MapFrom(src => src.StoreName));

        // Address ValueObject to AddressDto
        CreateMap<Address, AddressDto>();

        // Product mappings
        CreateMap<Product, ProductResponseDto>()
            .ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.Variants))
            .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductType.ToString()));
        
        CreateMap<ProductVariant, ProductVariantResponseDto>();
        
        CreateMap<CreateProductRequestDto, Product>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Variants, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

        // InventoryItem mappings
        CreateMap<InventoryItem, InventoryItemResponseDto>()
            .ForMember(dest => dest.Product, opt => opt.Ignore()) // Will be set manually
            .ForMember(dest => dest.IsInStock, opt => opt.Ignore())
            .ForMember(dest => dest.IsBelowReorderLevel, opt => opt.Ignore())
            .ForMember(dest => dest.SerialCount, opt => opt.Ignore())
            .ForMember(dest => dest.QuantityReserved, opt => opt.MapFrom(src => src.QuantityReserved))
            .ForMember(dest => dest.QuantityAvailable, opt => opt.MapFrom(src => src.GetQuantityAvailable()));
        
        CreateMap<CreateInventoryItemRequestDto, InventoryItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Product, opt => opt.Ignore())
            .ForMember(dest => dest.Serials, opt => opt.Ignore())
            .ForMember(dest => dest.Transactions, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        // SerialNumber mappings
        CreateMap<SerialNumber, SerialNumberResponseDto>()
            .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable()));

        // InventoryTransaction mappings
        CreateMap<InventoryTransaction, InventoryTransactionResponseDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
            .ForMember(dest => dest.TotalCost, opt => opt.MapFrom(src => src.GetTotalCost()))
            .ForMember(dest => dest.ReferenceNumber, opt => opt.MapFrom(src => src.ReferenceNumber))
            .ForMember(dest => dest.ReferenceType, opt => opt.MapFrom(src => src.ReferenceType));

        // StockTransfer mappings
        CreateMap<StockTransfer, StockTransferResponseDto>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
            .ForMember(dest => dest.TotalQuantity, opt => opt.MapFrom(src => src.GetTotalQuantity()));
        
        CreateMap<StockTransferItem, StockTransferItemResponseDto>()
            .ForMember(dest => dest.InventoryItem, opt => opt.Ignore());

        // Supplier mappings
        CreateMap<Supplier, SupplierResponseDto>();
        
        CreateMap<CreateSupplierRequestDto, Supplier>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore());

        // PurchaseOrder mappings
        CreateMap<PurchaseOrder, PurchaseOrderResponseDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Lines, opt => opt.MapFrom(src => src.Lines))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.GetTotalAmount()))
            .ForMember(dest => dest.Supplier, opt => opt.Ignore()); // Will be set manually
        
        CreateMap<PurchaseOrderLine, PurchaseOrderLineResponseDto>()
            .ForMember(dest => dest.TotalCost, opt => opt.MapFrom(src => src.GetTotalCost()))
            .ForMember(dest => dest.ProductName, opt => opt.Ignore());

        // SupplierReturn mappings
        CreateMap<SupplierReturn, SupplierReturnResponseDto>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
            .ForMember(dest => dest.TotalQuantity, opt => opt.MapFrom(src => src.GetTotalQuantity()))
            .ForMember(dest => dest.Supplier, opt => opt.Ignore());
        
        CreateMap<SupplierReturnItem, SupplierReturnItemResponseDto>()
            .ForMember(dest => dest.InventoryItem, opt => opt.Ignore());

        // Device hierarchy mappings
        CreateMap<DeviceCategory, DeviceCategoryResponseDto>();
        CreateMap<DeviceBrand, DeviceBrandResponseDto>();
        CreateMap<DeviceModel, DeviceModelResponseDto>();
        CreateMap<DeviceVariant, DeviceVariantResponseDto>();
        CreateMap<DeviceProblem, DeviceProblemResponseDto>();

        CreateMap<CreateDeviceCategoryRequestDto, DeviceCategory>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Brands, opt => opt.Ignore());

        CreateMap<CreateDeviceBrandRequestDto, DeviceBrand>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.Models, opt => opt.Ignore());

        CreateMap<CreateDeviceModelRequestDto, DeviceModel>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Brand, opt => opt.Ignore())
            .ForMember(dest => dest.Variants, opt => opt.Ignore())
            .ForMember(dest => dest.Problems, opt => opt.Ignore());

        // Service mappings
        CreateMap<Service, ServiceResponseDto>()
            .ForMember(dest => dest.Parts, opt => opt.Ignore()) // Will be set manually
            .ForMember(dest => dest.Labor, opt => opt.MapFrom(src => src.Labor))
            .ForMember(dest => dest.Warranties, opt => opt.MapFrom(src => src.Warranties))
            .ForMember(dest => dest.TotalCost, opt => opt.Ignore());

        CreateMap<ServicePart, ServicePartResponseDto>()
            .ForMember(dest => dest.ProductName, opt => opt.Ignore());

        CreateMap<ServiceLabor, ServiceLaborResponseDto>();
        CreateMap<ServiceWarranty, ServiceWarrantyResponseDto>();

        // PreCheckItem mappings
        CreateMap<PreCheckItem, PreCheckItemResponseDto>();

        CreateMap<CreatePreCheckItemRequestDto, PreCheckItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        // SpecialOrderPart mappings
        CreateMap<SpecialOrderPart, SpecialOrderPartResponseDto>()
            .ForMember(dest => dest.TotalCost, opt => opt.Ignore())
            .ForMember(dest => dest.ManufacturerName, opt => opt.Ignore())
            .ForMember(dest => dest.DeviceModelName, opt => opt.Ignore())
            .ForMember(dest => dest.SupplierName, opt => opt.Ignore());

        CreateMap<CreateSpecialOrderPartRequestDto, SpecialOrderPart>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        // Customer to CustomerResponseDto
        CreateMap<Customer, CustomerResponseDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address != null ? new AddressDto
            {
                StreetNumber = src.Address.StreetNumber,
                StreetName = src.Address.StreetName,
                City = src.Address.City,
                State = src.Address.State,
                PostalCode = src.Address.PostalCode
            } : null));

        // CreateCustomerRequestDto to Customer
        CreateMap<CreateCustomerRequestDto, Customer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore()) // Will be set to default Active
            .ForMember(dest => dest.TotalOrders, opt => opt.Ignore())
            .ForMember(dest => dest.TotalSpent, opt => opt.Ignore())
            .ForMember(dest => dest.IsSyncedWithExternal, opt => opt.Ignore())
            .ForMember(dest => dest.ExternalId, opt => opt.Ignore())
            .ForMember(dest => dest.Invoices, opt => opt.Ignore())
            .ForMember(dest => dest.Address, opt => opt.Ignore()); // Will be set manually

        // UpdateCustomerRequestDto to Customer (for updates)
        CreateMap<UpdateCustomerRequestDto, Customer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.StoreId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.TotalOrders, opt => opt.Ignore())
            .ForMember(dest => dest.TotalSpent, opt => opt.Ignore())
            .ForMember(dest => dest.IsSyncedWithExternal, opt => opt.Ignore())
            .ForMember(dest => dest.ExternalId, opt => opt.Ignore())
            .ForMember(dest => dest.Invoices, opt => opt.Ignore())
            .ForMember(dest => dest.Address, opt => opt.Ignore()) // Will be set manually
            .ForMember(dest => dest.Status, opt => opt.Ignore()); // Will be set manually if provided

        // Order to OrderResponseDto
        CreateMap<Order, OrderResponseDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod));

        // OrderItem to OrderItemResponseDto
        CreateMap<OrderItem, OrderItemResponseDto>();

        // Employee to EmployeeResponseDto
        CreateMap<Employee, EmployeeResponseDto>()
            .ForMember(dest => dest.Phone, opt => opt.Ignore()); // Phone is not in Employee entity

        // CreateEmployeeRequestDto to Employee
        // Note: Password field is ignored automatically (Employee doesn't have Password property)
        // Password is stored in UserAccount, not Employee
        CreateMap<CreateEmployeeRequestDto, Employee>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Address, opt => opt.Ignore()) // Will be set manually
            .ForMember(dest => dest.IsActive, opt => opt.Ignore()) // Will be set manually
            .ForMember(dest => dest.CanSwitchRole, opt => opt.Ignore()) // Will be set manually
            .ForMember(dest => dest.AvatarUrl, opt => opt.Ignore())
            .ForMember(dest => dest.AccessPinHash, opt => opt.Ignore());

        // RepairTicket to RepairTicketResponseDto
        CreateMap<RepairTicket, RepairTicketResponseDto>();

        // UserAccount to RegisterResponseDto
        CreateMap<UserAccount, RegisterResponseDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.EmailSent, opt => opt.Ignore())
            .ForMember(dest => dest.Message, opt => opt.Ignore());

        // Store + StoreSettings to StoreSettingsDto (complex mapping)
        CreateMap<Store, StoreSettingsDto>()
            .ForMember(dest => dest.BusinessName, opt => opt.MapFrom(src => src.StoreName))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.StoreEmail, opt => opt.MapFrom(src => src.Settings.StoreEmail))
            .ForMember(dest => dest.AlternateName, opt => opt.MapFrom(src => src.Settings.AlternateName))
            .ForMember(dest => dest.StoreLogoUrl, opt => opt.MapFrom(src => src.Settings.StoreLogoUrl))
            .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.Settings.Mobile))
            .ForMember(dest => dest.Website, opt => opt.MapFrom(src => src.Settings.Website))
            .ForMember(dest => dest.TimeZone, opt => opt.MapFrom(src => src.Settings.TimeZone ?? "UTC"))
            .ForMember(dest => dest.TimeFormat, opt => opt.MapFrom(src => src.Settings.TimeFormat ?? "12h"))
            .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.Settings.Language ?? "en"))
            .ForMember(dest => dest.DefaultCurrency, opt => opt.MapFrom(src => src.Settings.DefaultCurrency ?? "USD"))
            .ForMember(dest => dest.PriceFormat, opt => opt.MapFrom(src => src.Settings.PriceFormat ?? "$0.00"))
            .ForMember(dest => dest.DecimalFormat, opt => opt.MapFrom(src => src.Settings.DecimalFormat ?? "2"))
            .ForMember(dest => dest.ChargeSalesTax, opt => opt.MapFrom(src => src.Settings.ChargeSalesTax))
            .ForMember(dest => dest.DefaultTaxClass, opt => opt.MapFrom(src => src.Settings.DefaultTaxClass))
            .ForMember(dest => dest.TaxPercentage, opt => opt.MapFrom(src => src.Settings.TaxPercentage))
            .ForMember(dest => dest.RegistrationNumber, opt => opt.MapFrom(src => src.Settings.RegistrationNumber))
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.Settings.StartTime))
            .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.Settings.EndTime))
            .ForMember(dest => dest.DefaultAddress, opt => opt.MapFrom(src => src.Settings.DefaultAddress))
            .ForMember(dest => dest.ApiKey, opt => opt.Ignore()) // Will be handled manually for masking
            .ForMember(dest => dest.AccountingMethod, opt => opt.MapFrom(src => src.Settings.AccountingMethod ?? "Cash Basis"))
            .ForMember(dest => dest.CompanyEmail, opt => opt.MapFrom(src => src.Settings.CompanyEmail))
            .ForMember(dest => dest.CompanyEmailVerified, opt => opt.MapFrom(src => src.Settings.CompanyEmailVerified))
            .ForMember(dest => dest.EmailNotifications, opt => opt.MapFrom(src => src.Settings.EmailNotifications))
            .ForMember(dest => dest.RequireTwoFactorForAllUsers, opt => opt.MapFrom(src => src.Settings.RequireTwoFactorForAllUsers))
            .ForMember(dest => dest.ChargeRestockingFee, opt => opt.MapFrom(src => src.Settings.ChargeRestockingFee))
            .ForMember(dest => dest.DiagnosticBenchFee, opt => opt.MapFrom(src => src.Settings.DiagnosticBenchFee))
            .ForMember(dest => dest.ChargeDepositOnRepairs, opt => opt.MapFrom(src => src.Settings.ChargeDepositOnRepairs))
            .ForMember(dest => dest.LockScreenTimeoutMinutes, opt => opt.MapFrom(src => src.Settings.LockScreenTimeoutMinutes));

        // Reverse mappings for updates (Request DTOs to Entities)
        // Note: These are typically used for updates, so we map only specific fields
        CreateMap<AppUserProfileRequestDto, AppUser>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Email, opt => opt.Ignore()) // Email should not be updated via profile
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.StoreIds, opt => opt.Ignore())
            .ForMember(dest => dest.SubscriptionId, opt => opt.Ignore())
            .ForMember(dest => dest.TrialStartDate, opt => opt.Ignore())
            .ForMember(dest => dest.TrialEndDate, opt => opt.Ignore())
            .ForMember(dest => dest.TrialStatus, opt => opt.Ignore())
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Mobile ?? src.Phone))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => 
                src.StreetNumber != null || src.StreetName != null || src.City != null || 
                src.State != null || src.PostalCode != null
                    ? new Address(
                        src.StreetNumber ?? string.Empty,
                        src.StreetName ?? string.Empty,
                        src.City ?? string.Empty,
                        src.State ?? string.Empty,
                        src.PostalCode ?? string.Empty)
                    : null))
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}

