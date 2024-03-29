using Domain.Entities;
using Domain.Request;
using Xunit;

namespace Catalog.UnitTests.Entities;

public class ProductTests
{
    [Fact]
    public void Product_Properties_SetCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var product = new Product
        {
            Id = id,
            Name = "Coca Cola",
            Price = 9.99m,
            Description = "A description",
            Category = Category.Bebida,
            ImageURL = "https://example.com/image.jpg"
        };

        // Act - No action needed as this is just object initialization

        // Assert
        Assert.Equal(id, product.Id);
        Assert.Equal("Coca Cola", product.Name);
        Assert.Equal(9.99m, product.Price);
        Assert.Equal("A description", product.Description);
        Assert.Equal(Category.Bebida, product.Category);
        Assert.Equal("https://example.com/image.jpg", product.ImageURL);
    }

    [Fact]
    public void FromProductRequest_CreatesProductCorrectly()
    {
        // Arrange
        var productRequest = new ProductRequest
        {
            Name = "Test Product",
            Price = 29.99m,
            Description = "A description",
            Category = Category.Bebida,
            ImageURL = "https://example.com/image.jpg"
        };

        // Act
        var product = Product.FromProductRequest(productRequest);

        // Assert
        Assert.NotNull(product.Id);
        Assert.Equal("Test Product", product.Name);
        Assert.Equal(29.99m, product.Price);
        Assert.Equal("A description", product.Description);
        Assert.Equal(Category.Bebida, product.Category);
        Assert.Equal("https://example.com/image.jpg", product.ImageURL);
    }
}

