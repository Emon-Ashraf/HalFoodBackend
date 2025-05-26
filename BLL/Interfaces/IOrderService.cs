using DTO.Order;

namespace BLL.Interfaces;

public interface IOrderService
{
    Task<OrderResponseDto> PlaceOrderAsync(Guid userId, OrderPlaceDto dto);
    Task<List<OrderResponseDto>> GetCustomerOrdersAsync(Guid userId);
    Task<List<OrderResponseDto>> GetOrdersForShopAsync(Guid shopId);
}
