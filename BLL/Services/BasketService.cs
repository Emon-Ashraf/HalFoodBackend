using BLL.Interfaces;
using DAL.Data;
using DAL.Models;
using DTO.Basket;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;

public class BasketService : IBasketService
{
    private readonly ApplicationDbContext _db;

    public BasketService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<BasketItemResponseDto> AddItemAsync(Guid userId, BasketAddDto dto)
    {
        var product = await _db.Products.FindAsync(dto.ProductId);
        if (product == null) throw new Exception("Product not found.");

        var existingItem = await _db.BasketItems
            .FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == dto.ProductId);

        if (existingItem != null)
        {
            existingItem.Quantity += dto.Quantity;
            _db.BasketItems.Update(existingItem);
        }
        else
        {
            var newItem = new BasketItem
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            };
            _db.BasketItems.Add(newItem);
        }

        await _db.SaveChangesAsync();

        var item = await _db.BasketItems
            .Include(x => x.Product)
            .FirstAsync(x => x.UserId == userId && x.ProductId == dto.ProductId);

        return Map(item);
    }

    public async Task<List<BasketItemResponseDto>> GetUserBasketAsync(Guid userId)
    {
        var items = await _db.BasketItems
            .Include(x => x.Product)
            .Where(x => x.UserId == userId)
            .ToListAsync();

        return items.Select(Map).ToList();
    }

    public async Task<BasketItemResponseDto> UpdateItemAsync(Guid userId, Guid itemId, int quantity)
    {
        var item = await _db.BasketItems
            .Include(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == itemId && x.UserId == userId);

        if (item == null) throw new Exception("Item not found.");

        item.Quantity = quantity;
        _db.BasketItems.Update(item);
        await _db.SaveChangesAsync();

        return Map(item);
    }

    public async Task<bool> DeleteItemAsync(Guid userId, Guid itemId)
    {
        var item = await _db.BasketItems.FirstOrDefaultAsync(x => x.Id == itemId && x.UserId == userId);
        if (item == null) throw new Exception("Item not found.");

        _db.BasketItems.Remove(item);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task ClearBasketAsync(Guid userId)
    {
        var items = _db.BasketItems.Where(x => x.UserId == userId);
        _db.BasketItems.RemoveRange(items);
        await _db.SaveChangesAsync();
    }

    private BasketItemResponseDto Map(BasketItem item)
    {
        return new BasketItemResponseDto
        {
            Id = item.Id,
            ProductId = item.ProductId,
            ProductName = item.Product.Name,
            ProductPrice = item.Product.Price,
            ImageUrl = item.Product.ImageUrl,
            Quantity = item.Quantity
        };
    }
}
