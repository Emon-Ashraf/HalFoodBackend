using BLL.Interfaces;
using DTO.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HalFood.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // ✅ Place order (Customer only)
    [HttpPost("place")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> PlaceOrder([FromBody] OrderPlaceDto dto)
    {
        var userId = GetUserId();
        var result = await _orderService.PlaceOrderAsync(userId, dto);
        return Ok(result);
    }

    // ✅ View own orders (Customer)
    [HttpGet("my")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> MyOrders()
    {
        var userId = GetUserId();
        var result = await _orderService.GetCustomerOrdersAsync(userId);
        return Ok(result);
    }

    // ✅ Seller view orders by shop
    [HttpGet("shop/{shopId}")]
    [Authorize(Roles = "Butcher,Restaurant")]
    public async Task<IActionResult> ShopOrders(Guid shopId)
    {
        var result = await _orderService.GetOrdersForShopAsync(shopId);
        return Ok(result);
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
    }
}
