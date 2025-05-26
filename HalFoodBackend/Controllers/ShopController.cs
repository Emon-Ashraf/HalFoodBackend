using BLL.Interfaces;
using DTO.Shop;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HalFood.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShopController : ControllerBase
{
    private readonly IShopService _shopService;

    public ShopController(IShopService shopService)
    {
        _shopService = shopService;
    }

    [HttpPost("register")]
    [Authorize(Roles = "Butcher,Restaurant")]
    public async Task<IActionResult> RegisterShop([FromBody] ShopRegisterDto dto)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out Guid userId))
            return Unauthorized();

        try
        {
            var result = await _shopService.RegisterShopAsync(userId, dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("my")]
    [Authorize(Roles = "Butcher,Restaurant")]
    public async Task<IActionResult> GetMyShop()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out Guid userId))
            return Unauthorized();

        try
        {
            var shop = await _shopService.GetMyShopAsync(userId);
            return Ok(shop);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPut("update")]
    [Authorize(Roles = "Butcher,Restaurant")]
    public async Task<IActionResult> UpdateShop([FromBody] ShopUpdateDto dto)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out Guid userId))
            return Unauthorized();

        try
        {
            var result = await _shopService.UpdateMyShopAsync(userId, dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }



}
