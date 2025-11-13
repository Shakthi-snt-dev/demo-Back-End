using FlowTap.Api.Data;
using FlowTap.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FlowTap.Api.Services;

public class InventoryService : IInventoryService
{
    private readonly ApplicationDbContext _context;

    public InventoryService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProductDto>> GetProductsAsync(string? category, string? search)
    {
        var query = _context.Products.AsQueryable();

        if (!string.IsNullOrEmpty(category))
            query = query.Where(p => p.Category == category);

        if (!string.IsNullOrEmpty(search))
            query = query.Where(p => p.Name.Contains(search) || p.SKU.Contains(search));

        var products = await query.OrderBy(p => p.Name).ToListAsync();

        return products.Select(p => new ProductDto
        {
            Id = p.Id,
            SKU = p.SKU,
            Name = p.Name,
            Description = p.Description,
            Category = p.Category,
            Cost = p.Cost,
            RetailPrice = p.RetailPrice,
            Stock = p.Stock,
            LowStockAlert = p.LowStockAlert,
            Supplier = p.Supplier,
            SyncShopify = p.SyncShopify,
            SyncWooCommerce = p.SyncWooCommerce,
            CreatedAt = p.CreatedAt
        }).ToList();
    }

    public async Task<Product> CreateProductAsync(CreateProductRequest request)
    {
        var product = new Product
        {
            SKU = request.SKU,
            Name = request.Name,
            Description = request.Description,
            Category = request.Category,
            Cost = request.Cost,
            RetailPrice = request.RetailPrice,
            Stock = request.Stock,
            LowStockAlert = request.LowStockAlert,
            Supplier = request.Supplier,
            SyncShopify = request.SyncShopify,
            SyncWooCommerce = request.SyncWooCommerce,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return product;
    }

    public async Task<Product> UpdateProductAsync(Guid id, UpdateProductRequest request)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            throw new Exception("Product not found");

        if (!string.IsNullOrEmpty(request.Name))
            product.Name = request.Name;
        if (request.Description != null)
            product.Description = request.Description;
        if (request.Category != null)
            product.Category = request.Category;
        if (request.Cost.HasValue)
            product.Cost = request.Cost.Value;
        if (request.RetailPrice.HasValue)
            product.RetailPrice = request.RetailPrice.Value;
        if (request.Stock.HasValue)
            product.Stock = request.Stock.Value;
        if (request.LowStockAlert.HasValue)
            product.LowStockAlert = request.LowStockAlert.Value;
        if (request.Supplier != null)
            product.Supplier = request.Supplier;
        if (request.SyncShopify.HasValue)
            product.SyncShopify = request.SyncShopify.Value;
        if (request.SyncWooCommerce.HasValue)
            product.SyncWooCommerce = request.SyncWooCommerce.Value;

        product.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return product;
    }

    public async Task<bool> DeleteProductAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            throw new Exception("Product not found");

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return true;
    }
}

