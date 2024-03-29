using Domain.Entities;
using Domain.Repositories;
using Infra.Repositories;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Infra.Data.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(IConfiguration config, IMongoClient mongoClient) : base(config, mongoClient)
    {
    }

    public async Task<List<Product>> GetByCategory(Category category)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.Category, category);

        return await _collection.Find(filter).ToListAsync();
    }
}