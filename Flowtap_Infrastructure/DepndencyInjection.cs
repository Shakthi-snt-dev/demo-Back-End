using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Flowtap_Domain.BoundedContexts.Identity.Interfaces;
using Flowtap_Domain.BoundedContexts.Owner.Interfaces;
using Flowtap_Domain.BoundedContexts.Billing.Interfaces;
using Flowtap_Domain.BoundedContexts.Store.Interfaces;
using Flowtap_Domain.BoundedContexts.HR.Interfaces;
using Flowtap_Domain.BoundedContexts.Audit.Interfaces;
using Flowtap_Domain.BoundedContexts.Sales.Interfaces;
using Flowtap_Domain.BoundedContexts.Inventory.Interfaces;
using Flowtap_Domain.BoundedContexts.Procurement.Interfaces;
using Flowtap_Domain.BoundedContexts.Service.Interfaces;
using Flowtap_Infrastructure.Data;
using Flowtap_Infrastructure.Repositories;

namespace Flowtap_Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured. Please check appsettings.json");
            }

            services.AddDbContext<AppDbContext>(options =>
            {
                // Always use PostgreSQL - no fallback to in-memory
                Console.WriteLine($"[DATABASE] Configuring PostgreSQL connection...");
                Console.WriteLine($"[DATABASE] Connection string: {connectionString.Replace("Password=744888", "Password=***")}");
                
                options.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorCodesToAdd: null);
                });
                
                // Enable detailed logging in development
                #if DEBUG
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
                #endif
            });
            
            Console.WriteLine("[DATABASE] PostgreSQL configuration completed successfully.");

            // Register all repositories
            services.AddScoped<IUserAccountRepository, UserAccountRepository>();
            services.AddScoped<IAppUserRepository, AppUserRepository>();
            services.AddScoped<IAppUserAdminRepository, AppUserAdminRepository>();
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            services.AddScoped<IStoreRepository, StoreRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IActivityLogRepository, ActivityLogRepository>();
            
            // Sales Context
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            
            // Inventory Context
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IInventoryItemRepository, InventoryItemRepository>();
            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
            services.AddScoped<IProductSubCategoryRepository, ProductSubCategoryRepository>();
            services.AddScoped<ISerialNumberRepository, SerialNumberRepository>();
            services.AddScoped<IInventoryTransactionRepository, InventoryTransactionRepository>();
            services.AddScoped<IStockTransferRepository, StockTransferRepository>();
            
            // Procurement Context
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
            
        // Service Context
        services.AddScoped<IRepairTicketRepository, RepairTicketRepository>();
        services.AddScoped<IDeviceCategoryRepository, DeviceCategoryRepository>();
        services.AddScoped<IDeviceBrandRepository, DeviceBrandRepository>();
        services.AddScoped<IDeviceModelRepository, DeviceModelRepository>();
        services.AddScoped<IDeviceVariantRepository, DeviceVariantRepository>();
        services.AddScoped<IDeviceProblemRepository, DeviceProblemRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IPreCheckItemRepository, PreCheckItemRepository>();
        services.AddScoped<ISpecialOrderPartRepository, SpecialOrderPartRepository>();

        // Integration Context
        services.AddScoped<Flowtap_Domain.BoundedContexts.Integration.Interfaces.IIntegrationRepository, IntegrationRepository>();

            return services;
        }
    }
}
