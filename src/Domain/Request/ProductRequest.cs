using Domain.Base;
using Domain.Entities;

namespace Domain.Request;

public class ProductRequest
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public Category Category { get; set; }
    public string ImageURL { get; set; }
}