using Application.Interfaces;
using Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Domain.Request;

namespace Catalog.UnitTests.Controllers;

public class ProductControllerTests
{
    [Fact]
    public async Task Get_ReturnsOkResultWithListOfProducts()
    {
        // Arrange
        var productUseCaseMock = new Mock<IProductUseCase>();
        var controller = new ProductController(productUseCaseMock.Object);

        var products = new List<Product> { new Product(), new Product() };
        productUseCaseMock.Setup(useCase => useCase.Get()).ReturnsAsync(products);

        // Act
        var result = await controller.Get();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
        Assert.Equal(products.Count, returnedProducts.Count());
    }

    [Fact]
    public async Task GetById_ExistingId_ReturnsOkResultWithProduct()
    {
        // Arrange
        var productId = "1";
        var productUseCaseMock = new Mock<IProductUseCase>();
        var controller = new ProductController(productUseCaseMock.Object);

        var product = new Product { Id = productId };
        productUseCaseMock.Setup(useCase => useCase.GetById(productId)).ReturnsAsync(product);

        // Act
        var result = await controller.GetById(productId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedProduct = Assert.IsAssignableFrom<Product>(okResult.Value);
        Assert.Equal(productId, returnedProduct.Id);
    }

    [Fact]
    public async Task GetById_NonExistingId_ReturnsNotFoundResult()
    {
        // Arrange
        var nonExistingProductId = "non-existing-id";
        var productUseCaseMock = new Mock<IProductUseCase>();
        var controller = new ProductController(productUseCaseMock.Object);

        productUseCaseMock.Setup(useCase => useCase.GetById(nonExistingProductId)).ReturnsAsync((Product)null);

        // Act
        var result = await controller.GetById(nonExistingProductId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void GetProductCategories_ReturnsOkResultWithCategories()
    {
        // Arrange
        var productUseCaseMock = new Mock<IProductUseCase>();
        var controller = new ProductController(productUseCaseMock.Object);

        // Act
        var result = controller.GetProductCategories();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCategories = Assert.IsAssignableFrom<Dictionary<int, string>>(okResult.Value);

        Assert.NotEmpty(returnedCategories);
    }

    [Fact]
    public async Task Create_ValidProduct_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var productUseCaseMock = new Mock<IProductUseCase>();
        var controller = new ProductController(productUseCaseMock.Object);

        var productRequest = new ProductRequest(); // Pode ser criado usando o seu método de criação fictícia

        // Configurar mock para retornar um produto fictício ao chamar o método Add
        var createdProduct = new Product { Id = "1" };
        productUseCaseMock.Setup(useCase => useCase.Add(productRequest)).ReturnsAsync(createdProduct);

        // Act
        var result = await controller.Create(productRequest);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("GetById", createdAtActionResult.ActionName);
        Assert.Equal("1", createdAtActionResult.RouteValues["id"]);
    }

    [Fact]
    public async Task Update_ValidIdAndProduct_ReturnsNoContentResult()
    {
        // Arrange
        var productId = "1";
        var productUseCaseMock = new Mock<IProductUseCase>();
        var controller = new ProductController(productUseCaseMock.Object);

        var updatedProductRequest = new ProductRequest();

        // Act
        var result = await controller.Update(productId, updatedProductRequest);

        // Assert
        Assert.IsType<NoContentResult>(result);
        productUseCaseMock.Verify(useCase => useCase.Update(productId, updatedProductRequest), Times.Once);
    }

    [Fact]
    public async Task Delete_ValidId_ReturnsNoContentResult()
    {
        // Arrange
        var productId = "1";
        var productUseCaseMock = new Mock<IProductUseCase>();
        var controller = new ProductController(productUseCaseMock.Object);

        // Act
        var result = await controller.Delete(productId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        productUseCaseMock.Verify(useCase => useCase.Delete(productId), Times.Once);
    }

}
