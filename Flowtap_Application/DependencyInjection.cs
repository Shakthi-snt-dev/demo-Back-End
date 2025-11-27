using Microsoft.Extensions.DependencyInjection;
 using Flowtap_Application.Interfaces;
 using Flowtap_Application.Services;

namespace Flowtap_Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IJwtService, JwtService>();

            // Sales Context
            services.AddScoped<ICustomerService, CustomerService>();
            // services.AddScoped<IOrderService, OrderService>();

            // Inventory Context
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IInventoryItemService, InventoryItemService>();
            
            // Procurement Context
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();

            // Service Context
            services.AddScoped<IDeviceCategoryService, DeviceCategoryService>();
            services.AddScoped<IDeviceBrandService, DeviceBrandService>();
            services.AddScoped<IDeviceModelService, DeviceModelService>();
            services.AddScoped<IServiceService, ServiceService>();
            services.AddScoped<IPreCheckItemService, PreCheckItemService>();
            services.AddScoped<ISpecialOrderPartService, SpecialOrderPartService>();

            // Service Context
            // services.AddScoped<IRepairTicketService, RepairTicketService>();

            // HR Context
            services.AddScoped<IEmployeeService, EmployeeService>();

            // Dashboard
            // services.AddScoped<IDashboardService, DashboardService>();

            // Integration
            // services.AddScoped<IIntegrationService, IntegrationService>();

            // Profile Service
            services.AddScoped<IProfileService, ProfileService>();
            
            // Store Settings Service
            services.AddScoped<IStoreSettingsService, StoreSettingsService>();

            // AppUserAdmin
            // services.AddScoped<IAppUserAdminService, AppUserAdminService>();

            // Form Configuration
            // services.AddScoped<IFormConfigurationService, FormConfigurationService>();

            // Store
            services.AddScoped<IStoreService, StoreService>();

            // Role
            services.AddScoped<IRoleService, RoleService>();

            // HTTP Accessor
            services.AddScoped<IHttpAccessorService, HttpAccessorService>();

            // Authorization
            services.AddScoped<IAuthorizationService, AuthorizationService>();

            return services;
        }
    }
}