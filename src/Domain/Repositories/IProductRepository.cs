using Domain.Entities;
using Domain.Repositories.Base;

namespace Domain.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<List<Product>> GetByCategory(Category category);
}
