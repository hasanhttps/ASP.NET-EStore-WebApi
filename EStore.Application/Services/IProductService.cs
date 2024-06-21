using System.Net;
using EStore.Domain.ViewModels;

namespace ECommerce.Application.Services;

public interface IProductService {
    Task<ICollection<GetProductVM>> GetAllProductsAsync();
    Task<GetProductVM?> GetProductByIdAsync(int productId);
    Task AddProductAsync(AddProductVM productVM);
    Task<HttpStatusCode> UpdateProductAsync(int id, UpdateProductVM updateProductVM);
    Task<bool> DeleteProductAsync(int productId);
}
