using BLL.Interfaces;
using DTO.Basket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HalFood.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BasketController : ControllerBase
{
    private readonly IBasketService _basketService;

    public BasketController(IBasketService basketService)
    {
        _basketService = basketService;
    }

    [HttpPost("add")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> Add([FromBody] BasketAddDto dto)
    {
        var userId = GetUserId();
        var result = await _basketService.AddItemAsync(userId, dto);
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> Get()
    {
        var userId = GetUserId();
        var items = await _basketService.GetUserBasketAsync(userId);
        return Ok(items);
    }

    [HttpPut("{itemId}")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> Update(Guid itemId, [FromQuery] int quantity)
    {
        var userId = GetUserId();
        var result = await _basketService.UpdateItemAsync(userId, itemId, quantity);
        return Ok(result);
    }

    [HttpDelete("{itemId}")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> Delete(Guid itemId)
    {
        var userId = GetUserId();
        var result = await _basketService.DeleteItemAsync(userId, itemId);
        return Ok(new { deleted = result });
    }

    [HttpDelete("clear")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> Clear()
    {
        var userId = GetUserId();
        await _basketService.ClearBasketAsync(userId);
        return Ok(new { cleared = true });
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
    }
}
