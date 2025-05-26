using BLL.Interfaces;
using DAL.Data;
using DAL.Enums;
using DAL.Models;
using DTO.Product;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _db;

    public ProductService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<ProductResponseDto> AddProductAsync(Guid ownerId, ProductCreateDto dto)
    {
        var shop = await _db.Shops.FirstOrDefaultAsync(s => s.OwnerId == ownerId);
        if (shop == null)
            throw new Exception("Shop not found for this user.");

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            ImageUrl = dto.ImageUrl,
            Category = dto.Category,
            Type = dto.Type,
            IsFeatured = dto.IsFeatured,
            CreatedAt = DateTime.UtcNow,
            ShopId = shop.Id
        };

        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        return MapProduct(product);
    }

    public async Task<List<ProductResponseDto>> GetProductsByShopAsync(Guid shopId)
    {
        var products = await _db.Products
            .Where(p => p.ShopId == shopId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        return products.Select(MapProduct).ToList();
    }

    public async Task<List<ProductResponseDto>> GetPopularProductsAsync(ProductType? type = null)
    {
        var query = _db.Products
            .Where(p => p.IsFeatured);

        if (type.HasValue)
            query = query.Where(p => p.Type == type.Value);

        var products = await query
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        return products.Select(MapProduct).ToList();
    }


    private ProductResponseDto MapProduct(Product product)
    {
        return new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            ImageUrl = product.ImageUrl,
            Category = product.Category,
            Type = product.Type,
            IsFeatured = product.IsFeatured,
            CreatedAt = product.CreatedAt
        };
    }

    public async Task<ProductResponseDto> UpdateProductAsync(Guid userId, Guid productId, ProductUpdateDto dto)
    {
        var shop = await _db.Shops.FirstOrDefaultAsync(s => s.OwnerId == userId);
        if (shop == null) throw new Exception("Shop not found.");

        var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == productId && p.ShopId == shop.Id);
        if (product == null) throw new Exception("Product not found or unauthorized.");

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.ImageUrl = dto.ImageUrl;
        product.Category = dto.Category;
        product.IsFeatured = dto.IsFeatured;

        _db.Products.Update(product);
        await _db.SaveChangesAsync();

        return MapProduct(product);
    }

    public async Task<bool> DeleteProductAsync(Guid userId, Guid productId)
    {
        var shop = await _db.Shops.FirstOrDefaultAsync(s => s.OwnerId == userId);
        if (shop == null) throw new Exception("Shop not found.");

        var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == productId && p.ShopId == shop.Id);
        if (product == null) throw new Exception("Product not found or unauthorized.");

        _db.Products.Remove(product);
        await _db.SaveChangesAsync();

        return true;
    }


}
