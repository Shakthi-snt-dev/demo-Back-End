using Flowtap_Application.Interfaces;
using Flowtap_Domain.BoundedContexts.Sales.Entities;
using Flowtap_Domain.BoundedContexts.Sales.Interfaces;
using Flowtap_Domain.BoundedContexts.Sales.ValueObjects;
using Flowtap_Domain.BoundedContexts.Inventory.Interfaces;
using Flowtap_Domain.DtoModel;
using Flowtap_Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Flowtap_Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<OrderService> _logger;

    public OrderService(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        ICustomerRepository customerRepository,
        ILogger<OrderService> logger)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
        _logger = logger;
    }

    public async Task<OrderResponseDto> CreateOrderAsync(CreateOrderRequestDto request)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            CustomerName = request.CustomerName,
            StoreId = request.StoreId,
            PaymentMethod = request.PaymentMethod,
            Status = "pending",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Add items and validate stock
        foreach (var itemRequest in request.Items)
        {
            var product = await _productRepository.GetByIdAsync(itemRequest.ProductId);
            if (product == null)
                throw new EntityNotFoundException("Product", itemRequest.ProductId);

            if (!product.HasSufficientStock(itemRequest.Quantity))
                throw new Flowtap_Domain.Exceptions.InvalidOperationException(
                    $"Insufficient stock for product {product.Name}. Available: {product.Stock}, Requested: {itemRequest.Quantity}",
                    "Product",
                    new Dictionary<string, string> { { "ProductId", product.Id.ToString() } });

            var price = itemRequest.Price ?? product.Price;
            var orderItem = new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                ProductId = product.Id,
                ProductName = product.Name,
                SKU = product.SKU,
                Quantity = itemRequest.Quantity,
                Price = price,
                CreatedAt = DateTime.UtcNow
            };
            orderItem.RecalculateTotal();

            order.AddItem(orderItem);

            // Update product stock
            product.RemoveStock(itemRequest.Quantity);
            await _productRepository.UpdateAsync(product);
        }

        // Update customer total if customer exists
        if (request.CustomerId.HasValue)
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId.Value);
            if (customer != null)
            {
                customer.AddOrder(order.Total);
                await _customerRepository.UpdateAsync(customer);
            }
        }

        var created = await _orderRepository.CreateAsync(order);
        return MapToDto(created);
    }

    public async Task<OrderResponseDto> GetOrderByIdAsync(Guid id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null)
            throw new EntityNotFoundException("Order", id);

        return MapToDto(order);
    }

    public async Task<OrderResponseDto?> GetOrderByNumberAsync(string orderNumber)
    {
        var order = await _orderRepository.GetByOrderNumberAsync(orderNumber);
        return order != null ? MapToDto(order) : null;
    }

    public async Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync()
    {
        var orders = await _orderRepository.GetAllAsync();
        return orders.Select(MapToDto);
    }

    public async Task<IEnumerable<OrderResponseDto>> GetOrdersByCustomerIdAsync(Guid customerId)
    {
        var orders = await _orderRepository.GetByCustomerIdAsync(customerId);
        return orders.Select(MapToDto);
    }

    public async Task<IEnumerable<OrderResponseDto>> GetOrdersByStoreIdAsync(Guid storeId)
    {
        var orders = await _orderRepository.GetByStoreIdAsync(storeId);
        return orders.Select(MapToDto);
    }

    public async Task<IEnumerable<OrderResponseDto>> GetOrdersByStatusAsync(string status)
    {
        var orders = await _orderRepository.GetByStatusAsync(status);
        return orders.Select(MapToDto);
    }

    public async Task<IEnumerable<OrderResponseDto>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var orders = await _orderRepository.GetByDateRangeAsync(startDate, endDate);
        return orders.Select(MapToDto);
    }

    public async Task<OrderResponseDto> UpdateOrderAsync(Guid id, UpdateOrderRequestDto request)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null)
            throw new EntityNotFoundException("Order", id);

        if (!string.IsNullOrWhiteSpace(request.Status))
            order.UpdateStatus(request.Status);

        if (!string.IsNullOrWhiteSpace(request.PaymentMethod))
            order.SetPaymentMethod(request.PaymentMethod);

        if (request.Items != null && request.Items.Any())
        {
            foreach (var itemUpdate in request.Items)
            {
                var item = order.Items.FirstOrDefault(i => i.Id == itemUpdate.ItemId);
                if (item == null)
                    continue;

                if (itemUpdate.Quantity.HasValue)
                    order.UpdateItemQuantity(itemUpdate.ItemId, itemUpdate.Quantity.Value);

                if (itemUpdate.Price.HasValue)
                {
                    item.UpdatePrice(itemUpdate.Price.Value);
                    order.RecalculateTotals();
                }
            }
        }

        var updated = await _orderRepository.UpdateAsync(order);
        return MapToDto(updated);
    }

    public async Task<OrderResponseDto> CompleteOrderAsync(Guid id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null)
            throw new EntityNotFoundException("Order", id);

        order.Complete();
        var updated = await _orderRepository.UpdateAsync(order);
        return MapToDto(updated);
    }

    public async Task<OrderResponseDto> CancelOrderAsync(Guid id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null)
            throw new EntityNotFoundException("Order", id);

        // Restore stock for cancelled orders
        foreach (var item in order.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product != null)
            {
                product.AddStock(item.Quantity);
                await _productRepository.UpdateAsync(product);
            }
        }

        order.Cancel();
        var updated = await _orderRepository.UpdateAsync(order);
        return MapToDto(updated);
    }

    public async Task<bool> DeleteOrderAsync(Guid id)
    {
        return await _orderRepository.DeleteAsync(id);
    }

    private static OrderResponseDto MapToDto(Order order)
    {
        return new OrderResponseDto
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            CustomerId = order.CustomerId,
            CustomerName = order.CustomerName,
            StoreId = order.StoreId,
            Items = order.Items.Select(i => new OrderItemResponseDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                SKU = i.SKU,
                Quantity = i.Quantity,
                Price = i.Price,
                Total = i.Total
            }).ToList(),
            Subtotal = order.Subtotal,
            Tax = order.Tax,
            Total = order.Total,
            Status = order.Status,
            PaymentMethod = order.PaymentMethod,
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt
        };
    }
}

