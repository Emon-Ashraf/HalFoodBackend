using DTO.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IShopService
    {
        Task<ShopResponseDto> RegisterShopAsync(Guid ownerId, ShopRegisterDto dto);
        Task<ShopResponseDto> GetMyShopAsync(Guid ownerId);
        Task<ShopResponseDto> UpdateMyShopAsync(Guid ownerId, ShopUpdateDto dto);


    }
}
