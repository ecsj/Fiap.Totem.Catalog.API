using Domain.Base;
using Domain.Request;

namespace Domain.Entities;

public class Product : Entity, IAggregateRoot
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public Category Category { get; set; }
    public string ImageURL { get; set; }

    public static Product FromProductRequest(ProductRequest productRequest)
    {
        return new Product
        {
            Id = Guid.NewGuid().ToString(),
            Name = productRequest.Name,
            Price = productRequest.Price,
            Description = productRequest.Description,
            Category = productRequest.Category,
            ImageURL = productRequest.ImageURL
        };
    }
}