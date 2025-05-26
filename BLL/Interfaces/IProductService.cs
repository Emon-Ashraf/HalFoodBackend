using DAL.Enums;
using DTO.Product;

namespace BLL.Interfaces;

public interface IProductService
{
    Task<ProductResponseDto> AddProductAsync(Guid ownerId, ProductCreateDto dto);
    Task<List<ProductResponseDto>> GetProductsByShopAsync(Guid shopId);
    Task<List<ProductResponseDto>> GetPopularProductsAsync(ProductType? type = null);
    Task<ProductResponseDto> UpdateProductAsync(Guid userId, Guid productId, ProductUpdateDto dto);
    Task<bool> DeleteProductAsync(Guid userId, Guid productId);

}
