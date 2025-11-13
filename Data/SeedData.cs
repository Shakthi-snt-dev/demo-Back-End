using FlowTap.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FlowTap.Api.Data;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // Ensure database is created
        await context.Database.EnsureCreatedAsync();

        // Seed Roles
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            var adminRole = new ApplicationRole
            {
                Name = "Admin",
                IsSuperUser = true,
                Permissions = new Dictionary<string, Dictionary<string, bool>>
                {
                    { "Dashboard", new Dictionary<string, bool> { { "View", true }, { "Edit", true } } },
                    { "POS", new Dictionary<string, bool> { { "View", true }, { "Edit", true }, { "Delete", true } } },
                    { "Repairs", new Dictionary<string, bool> { { "View", true }, { "Edit", true }, { "Delete", true } } },
                    { "Inventory", new Dictionary<string, bool> { { "View", true }, { "Edit", true }, { "Delete", true } } },
                    { "Customers", new Dictionary<string, bool> { { "View", true }, { "Edit", true }, { "Delete", true } } },
                    { "Employees", new Dictionary<string, bool> { { "View", true }, { "Edit", true }, { "Delete", true } } },
                    { "Reports", new Dictionary<string, bool> { { "View", true }, { "Edit", true } } },
                    { "Settings", new Dictionary<string, bool> { { "View", true }, { "Edit", true } } },
                    { "Integrations", new Dictionary<string, bool> { { "View", true }, { "Edit", true } } }
                }
            };
            await roleManager.CreateAsync(adminRole);
        }

        if (!await roleManager.RoleExistsAsync("Manager"))
        {
            var managerRole = new ApplicationRole
            {
                Name = "Manager",
                Permissions = new Dictionary<string, Dictionary<string, bool>>
                {
                    { "Dashboard", new Dictionary<string, bool> { { "View", true } } },
                    { "POS", new Dictionary<string, bool> { { "View", true }, { "Edit", true } } },
                    { "Repairs", new Dictionary<string, bool> { { "View", true }, { "Edit", true } } },
                    { "Inventory", new Dictionary<string, bool> { { "View", true }, { "Edit", true } } },
                    { "Customers", new Dictionary<string, bool> { { "View", true }, { "Edit", true } } },
                    { "Reports", new Dictionary<string, bool> { { "View", true } } }
                }
            };
            await roleManager.CreateAsync(managerRole);
        }

        if (!await roleManager.RoleExistsAsync("Technician"))
        {
            var technicianRole = new ApplicationRole
            {
                Name = "Technician",
                Permissions = new Dictionary<string, Dictionary<string, bool>>
                {
                    { "Repairs", new Dictionary<string, bool> { { "View", true }, { "Edit", true } } },
                    { "Inventory", new Dictionary<string, bool> { { "View", true } } }
                }
            };
            await roleManager.CreateAsync(technicianRole);
        }

        if (!await roleManager.RoleExistsAsync("Cashier"))
        {
            var cashierRole = new ApplicationRole
            {
                Name = "Cashier",
                Permissions = new Dictionary<string, Dictionary<string, bool>>
                {
                    { "POS", new Dictionary<string, bool> { { "View", true }, { "Edit", true } } },
                    { "Customers", new Dictionary<string, bool> { { "View", true }, { "Edit", true } } }
                }
            };
            await roleManager.CreateAsync(cashierRole);
        }

        // Seed Default Store
        if (!await context.Stores.AnyAsync())
        {
            var store = new Store
            {
                Name = "Main Store",
                Email = "store@flowtap.com",
                Phone = "+1234567890",
                Address = "123 Main St",
                City = "New York",
                State = "NY",
                Country = "USA",
                Zip = "10001",
                TimeZone = "America/New_York",
                Currency = "USD",
                TaxRate = 8.5m,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            context.Stores.Add(store);
            await context.SaveChangesAsync();
        }

        // Seed Default Admin User
        var adminEmail = "admin@flowtap.com";
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var adminRole = await roleManager.FindByNameAsync("Admin");
            var adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                Username = "admin",
                RoleId = adminRole?.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(adminUser, "Admin@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}

