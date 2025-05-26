using BLL.Interfaces;
using DAL.Data;
using DAL.Enums;
using DAL.Models;
using DTO.Order;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _db;

    public OrderService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<OrderResponseDto> PlaceOrderAsync(Guid userId, OrderPlaceDto dto)
    {
        var basketItems = await _db.BasketItems
            .Include(b => b.Product)
            .Where(b => b.UserId == userId)
            .ToListAsync();

        if (!basketItems.Any())
            throw new Exception("Basket is empty.");

        var order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            DeliveryAddress = dto.DeliveryAddress,
            CreatedAt = DateTime.UtcNow,
            Status = OrderStatus.Pending,
            TotalPrice = basketItems.Sum(i => i.Product.Price * i.Quantity)
        };

        order.Items = basketItems.Select(i => new OrderItem
        {
            Id = Guid.NewGuid(),
            ProductId = i.ProductId,
            Quantity = i.Quantity,
            UnitPrice = i.Product.Price
        }).ToList();

        _db.Orders.Add(order);
        _db.BasketItems.RemoveRange(basketItems);

        await _db.SaveChangesAsync();

        return await GetCustomerOrdersAsync(userId)
    .ContinueWith(t => t.Result.First(o => o.Id == order.Id));

    }

    public async Task<List<OrderResponseDto>> GetCustomerOrdersAsync(Guid userId)
    {
        var orders = await _db.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        return orders.Select(MapOrder).ToList();
    }

    public async Task<List<OrderResponseDto>> GetOrdersForShopAsync(Guid shopId)
    {
        var orders = await _db.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .Where(o => o.Items.Any(i => i.Product.ShopId == shopId))
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        return orders.Select(MapOrder).ToList();
    }

    private OrderResponseDto MapOrder(Order order)
    {
        var items = order.Items.Select(i => new OrderItemResponseDto
        {
            ProductId = i.ProductId,
            ProductName = i.Product?.Name ?? string.Empty,
            Quantity = i.Quantity,
            UnitPrice = i.UnitPrice
        }).ToList();

        return new OrderResponseDto
        {
            Id = order.Id,
            CreatedAt = order.CreatedAt,
            Status = order.Status,
            TotalPrice = order.TotalPrice,
            DeliveryAddress = order.DeliveryAddress,
            Items = items
        };
    }



}
