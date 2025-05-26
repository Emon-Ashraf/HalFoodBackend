using DTO.Basket;

namespace BLL.Interfaces;

public interface IBasketService
{
    Task<BasketItemResponseDto> AddItemAsync(Guid userId, BasketAddDto dto);
    Task<List<BasketItemResponseDto>> GetUserBasketAsync(Guid userId);
    Task<BasketItemResponseDto> UpdateItemAsync(Guid userId, Guid itemId, int quantity);
    Task<bool> DeleteItemAsync(Guid userId, Guid itemId);
    Task ClearBasketAsync(Guid userId);
}
