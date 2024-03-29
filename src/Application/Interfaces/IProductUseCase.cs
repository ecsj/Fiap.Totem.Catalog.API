using Domain.Entities;
using Domain.Request;

namespace Application.Interfaces;

public interface IProductUseCase : IBaseUseCase<ProductRequest, Product>
{
    Task<List<Product>> GetByCategory(Category category);

}