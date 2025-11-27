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
using Flowtap_Domain.BoundedContexts.Procurement.Entities;
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
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<ProductSubCategory> ProductSubCategories { get; set; }
    public DbSet<ProductVariant> ProductVariants { get; set; }
    public DbSet<InventoryItem> InventoryItems { get; set; }
    public DbSet<SerialNumber> SerialNumbers { get; set; }
    public DbSet<InventoryTransaction> InventoryTransactions { get; set; }
    public DbSet<InventoryAdjustment> InventoryAdjustments { get; set; }
    public DbSet<StockTransfer> StockTransfers { get; set; }
    public DbSet<StockTransferItem> StockTransferItems { get; set; }
    public DbSet<InventorySettings> InventorySettings { get; set; }
    public DbSet<BarcodeTemplate> BarcodeTemplates { get; set; }

    // Procurement Context
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
    public DbSet<PurchaseOrderLine> PurchaseOrderLines { get; set; }
    public DbSet<SupplierReturn> SupplierReturns { get; set; }
    public DbSet<SupplierReturnItem> SupplierReturnItems { get; set; }

    // Service Context
    public DbSet<RepairTicket> RepairTickets { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<DeviceCategory> DeviceCategories { get; set; }
    public DbSet<DeviceBrand> DeviceBrands { get; set; }
    public DbSet<DeviceModel> DeviceModels { get; set; }
    public DbSet<DeviceVariant> DeviceVariants { get; set; }
    public DbSet<DeviceProblem> DeviceProblems { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<ServicePart> ServiceParts { get; set; }
    public DbSet<ServiceLabor> ServiceLabors { get; set; }
    public DbSet<ServiceWarranty> ServiceWarranties { get; set; }
    public DbSet<ServicePriceMatrix> ServicePriceMatrices { get; set; }
    public DbSet<PreCheckItem> PreCheckItems { get; set; }
    public DbSet<TicketPreCheck> TicketPreChecks { get; set; }
    public DbSet<ServiceDiagnosis> ServiceDiagnoses { get; set; }
    public DbSet<TicketConditionImage> TicketConditionImages { get; set; }
    public DbSet<SpecialOrderPart> SpecialOrderParts { get; set; }
    public DbSet<PartUsed> PartsUsed { get; set; }

    // Sales Context - Additional entities
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceItem> InvoiceItems { get; set; }
    public DbSet<Payment> Payments { get; set; }

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
            entity.Property(e => e.TimeFormat).HasDefaultValue("12h");
            entity.Property(e => e.Language).HasDefaultValue("en");
            entity.Property(e => e.DefaultCurrency).HasDefaultValue("USD");
            entity.Property(e => e.PriceFormat).HasDefaultValue("$0.00");
            entity.Property(e => e.DecimalFormat).HasDefaultValue("2");
            entity.Property(e => e.AccountingMethod).HasDefaultValue("Cash Basis");
            entity.Property(e => e.EmailNotifications).HasDefaultValue(true);
            entity.Property(e => e.LockScreenTimeoutMinutes).HasDefaultValue(15);
            entity.Property(e => e.TaxPercentage).HasPrecision(18, 2);
            entity.Property(e => e.DiagnosticBenchFee).HasPrecision(18, 2);
            
            // Configure DefaultAddress as owned entity
            entity.OwnsOne(e => e.DefaultAddress, a =>
            {
                a.Property(p => p.StreetNumber).HasColumnName("DefaultAddress_StreetNumber").HasMaxLength(50);
                a.Property(p => p.StreetName).HasColumnName("DefaultAddress_StreetName").HasMaxLength(200);
                a.Property(p => p.City).HasColumnName("DefaultAddress_City").HasMaxLength(100);
                a.Property(p => p.State).HasColumnName("DefaultAddress_State").HasMaxLength(100);
                a.Property(p => p.PostalCode).HasColumnName("DefaultAddress_PostalCode").HasMaxLength(20);
            });
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
            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50).HasDefaultValue(CustomerStatus.Active);
            entity.Property(e => e.TotalSpent).HasPrecision(18, 2);
            entity.Property(e => e.ExternalId).HasMaxLength(100);
            entity.HasIndex(e => e.StoreId);
            entity.HasIndex(e => e.Email);
            entity.HasIndex(e => e.Phone);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.ExternalId);
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

            // Configure relationship with Invoices (within Sales context)
            entity.HasMany(e => e.Invoices)
                .WithOne(i => i.Customer)
                .HasForeignKey(i => i.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
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

        // Sales Context - Invoice entities
        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.StoreId).IsRequired();
            entity.Property(e => e.CustomerId).IsRequired();
            entity.Property(e => e.InvoiceNumber).HasMaxLength(100);
            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50)
                .HasDefaultValue(InvoiceStatus.Draft);
            entity.Property(e => e.Subtotal).HasPrecision(18, 2);
            entity.Property(e => e.Tax).HasPrecision(18, 2);
            entity.Property(e => e.Total).HasPrecision(18, 2);
            entity.HasIndex(e => e.StoreId);
            entity.HasIndex(e => e.CustomerId);
            entity.HasIndex(e => e.InvoiceNumber).IsUnique().HasFilter("[InvoiceNumber] IS NOT NULL AND [InvoiceNumber] != ''");
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.CreatedAt);
            entity.HasIndex(e => e.DueDate);
            entity.HasIndex(e => new { e.StoreId, e.Status }); // Composite index for store-specific status queries
            entity.HasIndex(e => new { e.CustomerId, e.Status }); // Composite index for customer invoices

            // Configure relationships within Sales context
            entity.HasMany(e => e.Items)
                .WithOne(i => i.Invoice)
                .HasForeignKey(i => i.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Payments)
                .WithOne(p => p.Invoice)
                .HasForeignKey(p => p.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<InvoiceItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.InvoiceId).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
            entity.HasIndex(e => e.InvoiceId);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.InvoiceId).IsRequired();
            entity.Property(e => e.Method)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50)
                .HasDefaultValue(PaymentMethod.Cash);
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.TransactionReference).HasMaxLength(500);
            entity.HasIndex(e => e.InvoiceId);
            entity.HasIndex(e => e.Method);
            entity.HasIndex(e => e.PaidAt);
            entity.HasIndex(e => e.TransactionReference);
        });

        // ===========================
        // INVENTORY CONTEXT
        // ===========================
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SKU).HasMaxLength(100);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.CostPrice).HasPrecision(18, 2);
            entity.Property(e => e.SalePrice).HasPrecision(18, 2);
            entity.Property(e => e.MinimumPrice).HasPrecision(18, 2);
            entity.HasIndex(e => e.SKU);
            entity.HasIndex(e => e.CategoryId);
            entity.HasIndex(e => e.SubCategoryId);
            entity.HasIndex(e => e.SupplierId);
        });

        modelBuilder.Entity<InventoryItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.StoreId).IsRequired();
            entity.Property(e => e.ProductId).IsRequired();
            entity.Property(e => e.QuantityOnHand).IsRequired();
            entity.Property(e => e.QuantityReserved).IsRequired();
            entity.HasIndex(e => e.StoreId);
            entity.HasIndex(e => e.ProductId);
            entity.HasIndex(e => new { e.ProductId, e.StoreId }); // Composite index
        });

        // ===========================
        // SERVICE CONTEXT
        // ===========================
        modelBuilder.Entity<Device>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.StoreId).IsRequired();
            entity.Property(e => e.Brand).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Model).HasMaxLength(200);
            entity.Property(e => e.SerialNumber).HasMaxLength(200);
            entity.Property(e => e.IMEI).HasMaxLength(200);
            entity.HasIndex(e => e.StoreId);
            entity.HasIndex(e => e.SerialNumber);
            entity.HasIndex(e => e.IMEI);
            entity.HasIndex(e => new { e.StoreId, e.SerialNumber }).IsUnique().HasFilter("[SerialNumber] IS NOT NULL");
            entity.HasIndex(e => new { e.StoreId, e.IMEI }).IsUnique().HasFilter("[IMEI] IS NOT NULL");

            // Configure relationship with RepairTickets (within Service context)
            entity.HasMany(e => e.RepairTickets)
                .WithOne(t => t.Device)
                .HasForeignKey(t => t.DeviceId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<RepairTicket>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.StoreId).IsRequired();
            entity.Property(e => e.CustomerId).IsRequired();
            entity.Property(e => e.TicketNumber).HasMaxLength(100);
            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50)
                .HasDefaultValue(TicketStatus.Open);
            entity.Property(e => e.ProblemDescription).HasMaxLength(500);
            entity.Property(e => e.ResolutionNotes).HasMaxLength(500);
            entity.Property(e => e.Priority).HasMaxLength(50).HasDefaultValue("medium");
            entity.Property(e => e.CustomerName).HasMaxLength(200);
            entity.Property(e => e.CustomerPhone).HasMaxLength(20);
            entity.Property(e => e.CustomerEmail).HasMaxLength(320);
            entity.Property(e => e.DeviceDescription).HasMaxLength(200);
            entity.Property(e => e.EstimatedCost).HasPrecision(18, 2);
            entity.Property(e => e.ActualCost).HasPrecision(18, 2);
            entity.Property(e => e.DepositPaid).HasPrecision(18, 2);
            entity.HasIndex(e => e.StoreId);
            entity.HasIndex(e => e.CustomerId);
            entity.HasIndex(e => e.DeviceId);
            entity.HasIndex(e => e.TechnicianId);
            entity.HasIndex(e => e.TicketNumber);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.Priority);
            entity.HasIndex(e => e.CreatedAt);
            entity.HasIndex(e => e.EstimatedCompletionAt);
            entity.HasIndex(e => new { e.StoreId, e.Status }); // Composite index for store-specific status queries
            entity.HasIndex(e => new { e.StoreId, e.TechnicianId }); // Composite index for technician tickets per store
            entity.HasIndex(e => new { e.CustomerId, e.Status }); // Composite index for customer tickets

            // Configure relationship with PartsUsed (within Service context)
            entity.HasMany(e => e.PartsUsed)
                .WithOne(p => p.RepairTicket)
                .HasForeignKey(p => p.RepairTicketId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PartUsed>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RepairTicketId).IsRequired();
            entity.Property(e => e.InventoryItemId).IsRequired();
            entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
            entity.HasIndex(e => e.RepairTicketId);
            entity.HasIndex(e => e.InventoryItemId);
            entity.HasIndex(e => new { e.RepairTicketId, e.InventoryItemId }); // Composite index for part usage tracking
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

