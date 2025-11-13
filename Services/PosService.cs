using FlowTap.Api.Data;
using FlowTap.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FlowTap.Api.Services;

public class PosService : IPosService
{
    private readonly ApplicationDbContext _context;

    public PosService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Sale> CreateSaleAsync(CreateSaleRequest request)
    {
        var sale = new Sale
        {
            StoreId = request.StoreId,
            CustomerId = request.CustomerId,
            EmployeeId = request.EmployeeId,
            Discount = request.Discount,
            PaymentMethod = request.PaymentMethod,
            CreatedAt = DateTime.UtcNow
        };

        decimal subtotal = 0;
        foreach (var itemRequest in request.Items)
        {
            var product = await _context.Products.FindAsync(itemRequest.ProductId);
            if (product == null)
                throw new Exception($"Product {itemRequest.ProductId} not found");

            if (product.Stock < itemRequest.Quantity)
                throw new Exception($"Insufficient stock for product {product.Name}");

            var item = new SaleItem
            {
                ProductId = itemRequest.ProductId,
                Quantity = itemRequest.Quantity,
                UnitPrice = itemRequest.UnitPrice,
                Total = itemRequest.Quantity * itemRequest.UnitPrice
            };

            subtotal += item.Total;
            sale.Items.Add(item);

            // Update inventory
            product.Stock -= itemRequest.Quantity;
            product.UpdatedAt = DateTime.UtcNow;

            _context.InventoryTransactions.Add(new InventoryTransaction
            {
                ProductId = product.Id,
                StoreId = request.StoreId,
                Quantity = -itemRequest.Quantity,
                Type = InventoryTransactionType.Sale,
                CreatedAt = DateTime.UtcNow
            });
        }

        var store = await _context.Stores.FindAsync(request.StoreId);
        sale.Tax = (subtotal - request.Discount) * (store?.TaxRate ?? 0) / 100;
        sale.Total = subtotal - request.Discount + sale.Tax;

        sale.Payments.Add(new Payment
        {
            Amount = sale.Total,
            Method = request.PaymentMethod,
            Status = "Completed",
            CreatedAt = DateTime.UtcNow
        });

        // Update customer total spent
        if (request.CustomerId.HasValue)
        {
            var customer = await _context.Customers.FindAsync(request.CustomerId.Value);
            if (customer != null)
            {
                customer.TotalSpent += sale.Total;
                customer.UpdatedAt = DateTime.UtcNow;
            }
        }

        _context.Sales.Add(sale);
        await _context.SaveChangesAsync();

        return sale;
    }

    public async Task<List<SaleDto>> GetSalesAsync(DateTime? startDate, DateTime? endDate)
    {
        var query = _context.Sales.AsQueryable();

        if (startDate.HasValue)
            query = query.Where(s => s.CreatedAt >= startDate.Value);
        if (endDate.HasValue)
            query = query.Where(s => s.CreatedAt <= endDate.Value);

        var sales = await query
            .Include(s => s.Customer)
            .Include(s => s.Employee)
            .Include(s => s.Items)
                .ThenInclude(i => i.Product)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();

        return sales.Select(s => new SaleDto
        {
            Id = s.Id,
            StoreId = s.StoreId,
            CustomerName = s.Customer?.Name,
            EmployeeName = s.Employee?.Name,
            Total = s.Total,
            Tax = s.Tax,
            Discount = s.Discount,
            PaymentMethod = s.PaymentMethod,
            CreatedAt = s.CreatedAt,
            Items = s.Items.Select(i => new SaleItemDto
            {
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Total = i.Total
            }).ToList()
        }).ToList();
    }

    public async Task<SaleDto> GetSaleByIdAsync(Guid id)
    {
        var sale = await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.Employee)
            .Include(s => s.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (sale == null)
            throw new Exception("Sale not found");

        return new SaleDto
        {
            Id = sale.Id,
            StoreId = sale.StoreId,
            CustomerName = sale.Customer?.Name,
            EmployeeName = sale.Employee?.Name,
            Total = sale.Total,
            Tax = sale.Tax,
            Discount = sale.Discount,
            PaymentMethod = sale.PaymentMethod,
            CreatedAt = sale.CreatedAt,
            Items = sale.Items.Select(i => new SaleItemDto
            {
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Total = i.Total
            }).ToList()
        };
    }

    public async Task<bool> ProcessRefundAsync(Guid saleId)
    {
        var sale = await _context.Sales
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == saleId);

        if (sale == null)
            throw new Exception("Sale not found");

        // Restore inventory
        foreach (var item in sale.Items)
        {
            var product = await _context.Products.FindAsync(item.ProductId);
            if (product != null)
            {
                product.Stock += item.Quantity;
                product.UpdatedAt = DateTime.UtcNow;
            }

            _context.InventoryTransactions.Add(new InventoryTransaction
            {
                ProductId = item.ProductId,
                StoreId = sale.StoreId,
                Quantity = item.Quantity,
                Type = InventoryTransactionType.Return,
                Notes = $"Refund for sale {saleId}",
                CreatedAt = DateTime.UtcNow
            });
        }

        // Update customer total spent
        if (sale.CustomerId.HasValue)
        {
            var customer = await _context.Customers.FindAsync(sale.CustomerId.Value);
            if (customer != null)
            {
                customer.TotalSpent -= sale.Total;
                customer.UpdatedAt = DateTime.UtcNow;
            }
        }

        await _context.SaveChangesAsync();
        return true;
    }
}

