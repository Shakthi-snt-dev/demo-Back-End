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

            services.AddDbContext<AppDbContext>(options =>
            {
                if (!string.IsNullOrEmpty(connectionString) && connectionString.Contains("postgres", StringComparison.OrdinalIgnoreCase))
                {
                    options.UseNpgsql(connectionString);
                }
                else
                {
                    // Fallback to in-memory for development
                    options.UseInMemoryDatabase("FlowtapDb");
                }
            });

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
            
        // Service Context
        services.AddScoped<IRepairTicketRepository, RepairTicketRepository>();

        // Integration Context
        services.AddScoped<Flowtap_Domain.BoundedContexts.Integration.Interfaces.IIntegrationRepository, IntegrationRepository>();

            return services;
        }
    }
}
