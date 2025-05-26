using BLL.Interfaces;
using DAL.Data;
using DAL.Models;
using DTO.Shop;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ShopService : IShopService
    {
        private readonly ApplicationDbContext _db;

        public ShopService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<ShopResponseDto> RegisterShopAsync(Guid ownerId, ShopRegisterDto dto)
        {
            // Optional: Prevent duplicate shop per owner
            var existing = await _db.Shops.FirstOrDefaultAsync(s => s.OwnerId == ownerId);
            if (existing != null)
                throw new Exception("This user already has a shop registered.");

            var shop = new Shop
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                Address = dto.Address,
                ImageUrl = dto.ImageUrl,
                Type = dto.Type,
                OwnerId = ownerId
            };

            _db.Shops.Add(shop);
            await _db.SaveChangesAsync();

            return new ShopResponseDto
            {
                Id = shop.Id,
                Name = shop.Name,
                Description = shop.Description,
                Address = shop.Address,
                ImageUrl = shop.ImageUrl,
                Type = shop.Type
            };
        }

        public async Task<ShopResponseDto> GetMyShopAsync(Guid ownerId)
        {
            var shop = await _db.Shops
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.OwnerId == ownerId);

            if (shop == null)
                throw new Exception("No shop found for this user.");

            return new ShopResponseDto
            {
                Id = shop.Id,
                Name = shop.Name,
                Description = shop.Description,
                Address = shop.Address,
                ImageUrl = shop.ImageUrl,
                Type = shop.Type
            };
        }


        public async Task<ShopResponseDto> UpdateMyShopAsync(Guid ownerId, ShopUpdateDto dto)
        {
            var shop = await _db.Shops.FirstOrDefaultAsync(s => s.OwnerId == ownerId);
            if (shop == null)
                throw new Exception("No shop found for this user.");

            shop.Name = dto.Name;
            shop.Description = dto.Description;
            shop.Address = dto.Address;
            shop.ImageUrl = dto.ImageUrl;

            _db.Shops.Update(shop);
            await _db.SaveChangesAsync();

            return new ShopResponseDto
            {
                Id = shop.Id,
                Name = shop.Name,
                Description = shop.Description,
                Address = shop.Address,
                ImageUrl = shop.ImageUrl,
                Type = shop.Type
            };
        }


    }
}
