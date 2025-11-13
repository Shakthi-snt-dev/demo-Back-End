using FlowTap.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace FlowTap.Api.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Store> Stores { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<SaleItem> SaleItems { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<RepairTicket> RepairTickets { get; set; }
    public DbSet<DiagnosticReport> DiagnosticReports { get; set; }
    public DbSet<DeviceChecklist> DeviceChecklists { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<InventoryTransaction> InventoryTransactions { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<CustomerHistory> CustomerHistories { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<CommissionRule> CommissionRules { get; set; }
    public DbSet<Integration> Integrations { get; set; }
    public DbSet<BusinessSettings> BusinessSettings { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // User Configuration
        builder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable("Users");
            entity.HasOne(u => u.Role)
                  .WithMany()
                  .HasForeignKey(u => u.RoleId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Role Configuration with JSONB Permissions
        builder.Entity<ApplicationRole>(entity =>
        {
            entity.ToTable("Roles");
            entity.Property(r => r.Permissions)
                  .HasColumnType("jsonb")
                  .HasConversion(
                      v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                      v => JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, bool>>>(v, (JsonSerializerOptions?)null) ?? new());
        });

        // Store Configuration
        builder.Entity<Store>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.HasMany(s => s.Sales)
                  .WithOne(s => s.Store)
                  .HasForeignKey(s => s.StoreId);
            entity.HasMany(s => s.RepairTickets)
                  .WithOne(r => r.Store)
                  .HasForeignKey(r => r.StoreId);
            entity.HasMany(s => s.Employees)
                  .WithOne(e => e.Store)
                  .HasForeignKey(e => e.StoreId);
        });

        // Sale Configuration
        builder.Entity<Sale>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.HasOne(s => s.Customer)
                  .WithMany(c => c.Sales)
                  .HasForeignKey(s => s.CustomerId)
                  .OnDelete(DeleteBehavior.SetNull);
            entity.HasOne(s => s.Employee)
                  .WithMany()
                  .HasForeignKey(s => s.EmployeeId)
                  .OnDelete(DeleteBehavior.SetNull);
            entity.HasMany(s => s.Items)
                  .WithOne(i => i.Sale)
                  .HasForeignKey(i => i.SaleId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(s => s.Payments)
                  .WithOne(p => p.Sale)
                  .HasForeignKey(p => p.SaleId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // SaleItem Configuration
        builder.Entity<SaleItem>(entity =>
        {
            entity.HasKey(si => si.Id);
            entity.HasOne(si => si.Product)
                  .WithMany()
                  .HasForeignKey(si => si.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // RepairTicket Configuration
        builder.Entity<RepairTicket>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.HasOne(r => r.Customer)
                  .WithMany(c => c.RepairTickets)
                  .HasForeignKey(r => r.CustomerId)
                  .OnDelete(DeleteBehavior.SetNull);
            entity.HasOne(r => r.Technician)
                  .WithMany()
                  .HasForeignKey(r => r.TechnicianId)
                  .OnDelete(DeleteBehavior.SetNull);
            entity.Property(r => r.Priority)
                  .HasConversion<string>();
            entity.Property(r => r.Status)
                  .HasConversion<string>();
        });

        // DiagnosticReport Configuration with JSONB
        builder.Entity<DiagnosticReport>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.HasOne(d => d.Ticket)
                  .WithOne(r => r.DiagnosticReport)
                  .HasForeignKey<DiagnosticReport>(d => d.TicketId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.Property(d => d.Results)
                  .HasColumnType("jsonb")
                  .HasConversion(
                      v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                      v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, (JsonSerializerOptions?)null) ?? new());
        });

        // DeviceChecklist Configuration with JSONB
        builder.Entity<DeviceChecklist>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.HasOne(d => d.Ticket)
                  .WithOne(r => r.DeviceChecklist)
                  .HasForeignKey<DeviceChecklist>(d => d.TicketId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.Property(d => d.ConditionChecks)
                  .HasColumnType("jsonb")
                  .HasConversion(
                      v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                      v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, (JsonSerializerOptions?)null) ?? new());
        });

        // Product Configuration
        builder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.HasIndex(p => p.SKU).IsUnique();
        });

        // InventoryTransaction Configuration
        builder.Entity<InventoryTransaction>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.HasOne(i => i.Product)
                  .WithMany()
                  .HasForeignKey(i => i.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.Property(i => i.Type)
                  .HasConversion<string>();
        });

        // Customer Configuration
        builder.Entity<Customer>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.HasMany(c => c.Sales)
                  .WithOne(s => s.Customer)
                  .HasForeignKey(s => s.CustomerId);
            entity.HasMany(c => c.RepairTickets)
                  .WithOne(r => r.Customer)
                  .HasForeignKey(r => r.CustomerId);
            entity.HasMany(c => c.History)
                  .WithOne(h => h.Customer)
                  .HasForeignKey(h => h.CustomerId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // CustomerHistory Configuration
        builder.Entity<CustomerHistory>(entity =>
        {
            entity.HasKey(ch => ch.Id);
            entity.Property(ch => ch.ReferenceType)
                  .HasConversion<string>();
        });

        // Employee Configuration
        builder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Role)
                  .WithMany()
                  .HasForeignKey(e => e.RoleId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.Property(e => e.Status)
                  .HasConversion<string>();
        });

        // CommissionRule Configuration
        builder.Entity<CommissionRule>(entity =>
        {
            entity.HasKey(cr => cr.Id);
            entity.HasOne(cr => cr.Employee)
                  .WithMany(e => e.CommissionRules)
                  .HasForeignKey(cr => cr.EmployeeId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.Property(cr => cr.Type)
                  .HasConversion<string>();
        });

        // Integration Configuration
        builder.Entity<Integration>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.Type)
                  .HasConversion<string>();
            entity.Property(i => i.Status)
                  .HasConversion<string>();
        });

        // BusinessSettings Configuration
        builder.Entity<BusinessSettings>(entity =>
        {
            entity.HasKey(bs => bs.Id);
            entity.HasOne(bs => bs.DefaultStore)
                  .WithMany()
                  .HasForeignKey(bs => bs.DefaultStoreId)
                  .OnDelete(DeleteBehavior.SetNull);
        });
    }
}

