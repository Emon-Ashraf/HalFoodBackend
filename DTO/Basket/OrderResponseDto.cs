using DAL.Enums;

namespace DTO.Order;

public class OrderResponseDto
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string DeliveryAddress { get; set; } = string.Empty;
    public OrderStatus Status { get; set; }
    public decimal TotalPrice { get; set; }
    public List<OrderItemResponseDto> Items { get; set; } = new();
}
