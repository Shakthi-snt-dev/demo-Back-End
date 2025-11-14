using Flowtap_Domain.DtoModel;

namespace Flowtap_Application.Interfaces;

public interface IOrderService
{
    Task<OrderResponseDto> CreateOrderAsync(CreateOrderRequestDto request);
    Task<OrderResponseDto> GetOrderByIdAsync(Guid id);
    Task<OrderResponseDto?> GetOrderByNumberAsync(string orderNumber);
    Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync();
    Task<IEnumerable<OrderResponseDto>> GetOrdersByCustomerIdAsync(Guid customerId);
    Task<IEnumerable<OrderResponseDto>> GetOrdersByStoreIdAsync(Guid storeId);
    Task<IEnumerable<OrderResponseDto>> GetOrdersByStatusAsync(string status);
    Task<IEnumerable<OrderResponseDto>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<OrderResponseDto> UpdateOrderAsync(Guid id, UpdateOrderRequestDto request);
    Task<OrderResponseDto> CompleteOrderAsync(Guid id);
    Task<OrderResponseDto> CancelOrderAsync(Guid id);
    Task<bool> DeleteOrderAsync(Guid id);
}

