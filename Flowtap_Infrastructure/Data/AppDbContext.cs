using Microsoft.EntityFrameworkCore;
using Flowtap_Domain.BoundedContexts.Identity.Entities;
using Flowtap_Domain.BoundedContexts.Owner.Entities;
using Flowtap_Domain.BoundedContexts.Billing.Entities;
using Flowtap_Domain.BoundedContexts.Store.Entities;
using Flowtap_Domain.BoundedContexts.HR.Entities;
using Flowtap_Domain.BoundedContexts.Audit.Entities;
using Flowtap_Domain.BoundedContexts.Sales.Entities;
using Flowtap_Domain.BoundedContexts.Sales.ValueObjects;
using Flowtap_Domain.BoundedContexts.Inventory.Entities;
using Flowtap_Domain.BoundedContexts.Service.Entities;
using Flowtap_Domain.BoundedContexts.Integration.Entities;
using Flowtap_Domain.SharedKernel.ValueObjects;
using Flowtap_Domain.SharedKernel.Enums;

namespace Flowtap_Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Identity Context
    public DbSet<UserAccount> UserAccounts { get; set; }

    // Owner Context
    public DbSet<AppUser> AppUsers { get; set; }

    // Billing Context
    public DbSet<Subscription> Subscriptions { get; set; }

    // Store Context
    public DbSet<Store> Stores { get; set; }
    public DbSet<StoreSettings> StoreSettings { get; set; }

    // HR Context
    public DbSet<Employee> Employees { get; set; }

    // Audit Context
    public DbSet<ActivityLog> ActivityLogs { get; set; }

    // Sales Context
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    // Inventory Context
    public DbSet<Product> Products { get; set; }

        // Service Context
        public DbSet<RepairTicket> RepairTickets { get; set; }

        // Integration Context
        public DbSet<Flowtap_Domain.BoundedContexts.Integration.Entities.Integration> Integrations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Ignore AppUserAdmin - it's kept in domain but not mapped to database
        modelBuilder.Ignore<AppUserAdmin>();

        // ===========================
        // IDENTITY CONTEXT
        // ===========================
        modelBuilder.Entity<UserAccount>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(320);
            entity.Property(e => e.Username).HasMaxLength(50);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.UserType).IsRequired();
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.Username).IsUnique().HasFilter("[Username] IS NOT NULL");
            entity.HasIndex(e => e.EmailVerificationToken).IsUnique().HasFilter("[EmailVerificationToken] IS NOT NULL");
        });

        // ===========================
        // OWNER CONTEXT
        // ===========================
        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(320);
            entity.Property(e => e.Currency).HasDefaultValue("USD");
            entity.Property(e => e.TimeZone).HasDefaultValue("UTC");
            entity.Property(e => e.TrialStatus).HasDefaultValue(TrialStatus.NotStarted);
            
            // Configure Address as owned entity
            entity.OwnsOne(e => e.Address, a =>
            {
                a.Property(p => p.StreetNumber).HasColumnName("StreetNumber").HasMaxLength(50);
                a.Property(p => p.StreetName).HasColumnName("StreetName").HasMaxLength(200);
                a.Property(p => p.City).HasColumnName("City").HasMaxLength(100);
                a.Property(p => p.State).HasColumnName("State").HasMaxLength(100);
                a.Property(p => p.PostalCode).HasColumnName("PostalCode").HasMaxLength(20);
            });

            // Configure StoreIds as JSON array
            entity.Property(e => e.StoreIds)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(Guid.Parse)
                        .ToList())
                .HasColumnType("text");

        });

        // ===========================
        // BILLING CONTEXT
        // ===========================
        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.AppUserId).IsRequired();
            entity.Property(e => e.PlanName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Status).HasDefaultValue(SubscriptionStatus.Active);
            entity.Property(e => e.BillingInterval).HasDefaultValue(BillingInterval.Monthly);
            entity.Property(e => e.PricePerMonth).HasPrecision(18, 2);
            entity.HasIndex(e => e.ExternalSubscriptionId).IsUnique().HasFilter("[ExternalSubscriptionId] IS NOT NULL");
        });

        // ===========================
        // STORE CONTEXT
        // ===========================
        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.AppUserId).IsRequired();
            entity.Property(e => e.StoreName).IsRequired().HasMaxLength(200);
            
            // Configure Address as owned entity
            entity.OwnsOne(e => e.Address, a =>
            {
                a.Property(p => p.StreetNumber).HasColumnName("StreetNumber").HasMaxLength(50);
                a.Property(p => p.StreetName).HasColumnName("StreetName").HasMaxLength(200);
                a.Property(p => p.City).HasColumnName("City").HasMaxLength(100);
                a.Property(p => p.State).HasColumnName("State").HasMaxLength(100);
                a.Property(p => p.PostalCode).HasColumnName("PostalCode").HasMaxLength(20);
            });

            // Configure EmployeeIds as JSON array
            entity.Property(e => e.EmployeeIds)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(Guid.Parse)
                        .ToList())
                .HasColumnType("text");

            entity.HasOne(e => e.Settings)
                .WithOne()
                .HasForeignKey<StoreSettings>(s => s.StoreId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<StoreSettings>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.StoreId).IsRequired();
            entity.Property(e => e.EnablePOS).HasDefaultValue(true);
            entity.Property(e => e.EnableInventory).HasDefaultValue(true);
            entity.Property(e => e.TimeZone).HasDefaultValue("UTC");
        });

        // ===========================
        // HR CONTEXT
        // ===========================
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.StoreId).IsRequired();
            entity.Property(e => e.Email).IsRequired().HasMaxLength(320);
            entity.Property(e => e.HourlyRate).HasPrecision(18, 2);
            entity.HasIndex(e => e.StoreId);
            entity.HasIndex(e => e.Email);
            entity.HasIndex(e => e.EmployeeCode).IsUnique().HasFilter("[EmployeeCode] IS NOT NULL");
            entity.HasIndex(e => new { e.StoreId, e.Email }); // Composite index for store-specific email lookup
            entity.HasIndex(e => new { e.StoreId, e.IsActive }); // Composite index for active employees per store
            entity.HasIndex(e => new { e.StoreId, e.Role }); // Composite index for employees by role per store
            
            // Configure Address as owned entity
            entity.OwnsOne(e => e.Address, a =>
            {
                a.Property(p => p.StreetNumber).HasColumnName("StreetNumber").HasMaxLength(50);
                a.Property(p => p.StreetName).HasColumnName("StreetName").HasMaxLength(200);
                a.Property(p => p.City).HasColumnName("City").HasMaxLength(100);
                a.Property(p => p.State).HasColumnName("State").HasMaxLength(100);
                a.Property(p => p.PostalCode).HasColumnName("PostalCode").HasMaxLength(20);
            });
        });

        // ===========================
        // AUDIT CONTEXT
        // ===========================
        modelBuilder.Entity<ActivityLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Action).IsRequired().HasMaxLength(200);
            entity.HasIndex(e => e.AppUserId);
            entity.HasIndex(e => e.StoreId);
            entity.HasIndex(e => e.EmployeeId);
            entity.HasIndex(e => e.CreatedAt);
        });

        // ===========================
        // SALES CONTEXT
        // ===========================
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.StoreId).IsRequired();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(320);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50).HasDefaultValue("active");
            entity.Property(e => e.TotalSpent).HasPrecision(18, 2);
            entity.HasIndex(e => e.StoreId);
            entity.HasIndex(e => e.Email);
            entity.HasIndex(e => e.Phone);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => new { e.StoreId, e.Email }); // Composite index for store-specific email lookup
            
            // Configure Address as owned entity
            entity.OwnsOne(e => e.Address, a =>
            {
                a.Property(p => p.StreetNumber).HasColumnName("StreetNumber").HasMaxLength(50);
                a.Property(p => p.StreetName).HasColumnName("StreetName").HasMaxLength(200);
                a.Property(p => p.City).HasColumnName("City").HasMaxLength(100);
                a.Property(p => p.State).HasColumnName("State").HasMaxLength(100);
                a.Property(p => p.PostalCode).HasColumnName("PostalCode").HasMaxLength(20);
            });
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.OrderNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.StoreId).IsRequired();
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50).HasDefaultValue("pending");
            entity.Property(e => e.PaymentMethod).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Subtotal).HasPrecision(18, 2);
            entity.Property(e => e.Tax).HasPrecision(18, 2);
            entity.Property(e => e.Total).HasPrecision(18, 2);
            entity.HasIndex(e => e.OrderNumber).IsUnique();
            entity.HasIndex(e => e.CustomerId);
            entity.HasIndex(e => e.StoreId);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.CreatedAt);
            entity.HasIndex(e => new { e.StoreId, e.Status }); // Composite index for store-specific status queries
            entity.HasIndex(e => new { e.StoreId, e.CreatedAt }); // Composite index for store-specific date range queries
            entity.HasIndex(e => new { e.CustomerId, e.CreatedAt }); // Composite index for customer order history

            entity.HasMany(e => e.Items)
                .WithOne()
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.OrderId).IsRequired();
            entity.Property(e => e.ProductId).IsRequired();
            entity.Property(e => e.ProductName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.SKU).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Price).HasPrecision(18, 2);
            entity.Property(e => e.Total).HasPrecision(18, 2);
            entity.HasIndex(e => e.OrderId);
            entity.HasIndex(e => e.ProductId);
        });

        // ===========================
        // INVENTORY CONTEXT
        // ===========================
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.StoreId).IsRequired();
            entity.Property(e => e.SKU).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.Price).HasPrecision(18, 2);
            entity.Property(e => e.Cost).HasPrecision(18, 2);
            entity.HasIndex(e => e.StoreId);
            entity.HasIndex(e => new { e.StoreId, e.SKU }).IsUnique(); // SKU unique per store
            entity.HasIndex(e => e.Category);
            entity.HasIndex(e => new { e.StoreId, e.Category }); // Composite index for store-specific category lookup
        });

        // ===========================
        // SERVICE CONTEXT
        // ===========================
        modelBuilder.Entity<RepairTicket>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.StoreId).IsRequired();
            entity.Property(e => e.TicketNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.CustomerName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.CustomerPhone).HasMaxLength(20);
            entity.Property(e => e.CustomerEmail).HasMaxLength(320);
            entity.Property(e => e.Device).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50).HasDefaultValue("pending");
            entity.Property(e => e.Priority).IsRequired().HasMaxLength(50).HasDefaultValue("medium");
            entity.Property(e => e.EstimatedCost).HasPrecision(18, 2);
            entity.Property(e => e.DepositPaid).HasPrecision(18, 2);
            entity.HasIndex(e => e.StoreId);
            entity.HasIndex(e => e.CustomerId);
            entity.HasIndex(e => e.TicketNumber).IsUnique();
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.Priority);
            entity.HasIndex(e => e.AssignedToEmployeeId);
            entity.HasIndex(e => e.CreatedDate);
            entity.HasIndex(e => e.DueDate);
            entity.HasIndex(e => new { e.StoreId, e.Status }); // Composite index for store-specific status queries
            entity.HasIndex(e => new { e.StoreId, e.AssignedToEmployeeId }); // Composite index for employee tickets per store
        });

        // ===========================
        // INTEGRATION CONTEXT
        // ===========================
        modelBuilder.Entity<Integration>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.AppUserId).IsRequired();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
            entity.Property(e => e.SettingsJson).HasColumnType("text");
            entity.HasIndex(e => new { e.AppUserId, e.Type }).IsUnique();
            entity.HasIndex(e => e.AppUserId);
        });
    }
}

