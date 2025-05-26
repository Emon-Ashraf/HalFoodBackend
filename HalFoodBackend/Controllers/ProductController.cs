using BLL.Interfaces;
using DAL.Enums;
using DTO.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HalFood.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost("add")]
    [Authorize(Roles = "Butcher,Restaurant")]
    public async Task<IActionResult> Add([FromBody] ProductCreateDto dto)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out Guid userId))
            return Unauthorized();

        try
        {
            var result = await _productService.AddProductAsync(userId, dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("shop/{shopId}")]
    public async Task<IActionResult> GetByShop(Guid shopId)
    {
        var products = await _productService.GetProductsByShopAsync(shopId);
        return Ok(products);
    }

    [HttpGet("popular")]
    public async Task<IActionResult> GetPopular([FromQuery] ProductType? type = null)
    {
        var products = await _productService.GetPopularProductsAsync(type);
        return Ok(products);
    }

    [HttpPut("update/{id}")]
    [Authorize(Roles = "Butcher,Restaurant")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ProductUpdateDto dto)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out Guid userId))
            return Unauthorized();

        try
        {
            var result = await _productService.UpdateProductAsync(userId, id, dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("delete/{id}")]
    [Authorize(Roles = "Butcher,Restaurant")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out Guid userId))
            return Unauthorized();

        try
        {
            var success = await _productService.DeleteProductAsync(userId, id);
            return Ok(new { deleted = success });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }


}
