using Domain.Entities;
using Domain.Request;
using Application.Interfaces;
using Application.UseCases;
using Moq;
using Xunit;
using Catalog.UnitTests.Fakes;
using Domain.Base;
using Domain.Repositories;

namespace Catalog.UnitTests.UseCases;

public class ProductUseCaseTests
{
    [Fact]
    public async Task Get_ReturnsListOfProducts()
    {
        // Arrange
        var products = ProductFake.Create(10);
        var productRepositoryMock = new Mock<IProductRepository>();
        var messageQueueServiceMock = new Mock<IMessageQueueService>();

        var productUseCase = new ProductUseCase(productRepositoryMock.Object, messageQueueServiceMock.Object);

        productRepositoryMock.Setup(repo => repo.Get()).ReturnsAsync(products);


        // Act
        var result = await productUseCase.Get();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<Product>>(result);
    }

    [Fact]
    public async Task GetById_ReturnsProduct()
    {
        // Arrange
        var productId = "1";
        var productRepositoryMock = new Mock<IProductRepository>();
        productRepositoryMock.Setup(repo => repo.GetEntityByIdAsync(productId)).ReturnsAsync(new Product { Id = productId });
        var messageQueueServiceMock = new Mock<IMessageQueueService>();

        var productUseCase = new ProductUseCase(productRepositoryMock.Object, messageQueueServiceMock.Object);

        // Act
        var result = await productUseCase.GetById(productId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(productId, result.Id);
    }

    // Add similar tests for GetByCategory, Add, Update, and Delete methods

    [Fact]
    public async Task Add_CallsRepositoryAndMessageQueueService()
    {
        // Arrange
        var productRequest = ProductRequestFake.Create(1).FirstOrDefault();

        var productRepositoryMock = new Mock<IProductRepository>();
        var messageQueueServiceMock = new Mock<IMessageQueueService>();

        var productUseCase = new ProductUseCase(productRepositoryMock.Object, messageQueueServiceMock.Object);

        // Act
        await productUseCase.Add(productRequest);

        // Assert
        productRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Once);
        messageQueueServiceMock.Verify(msgQueue => msgQueue.PublishMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Update_ValidProduct_ShouldUpdateAndPublishMessage()
    {
        // Arrange
        var productId = "123";
        var productRequest = new ProductRequest
        {
            Name = "Updated Product",
            Price = 20.5m,
            Description = "Updated description",
            Category = Category.Acompanhamento,
            ImageURL = "https://example.com/updated-image.jpg"
        };

        var product = new Product
        {
            Id = productId,
            Name = "Original Product",
            Price = 15.99m,
            Description = "Original description",
            Category = Category.Lanche,
            ImageURL = "https://example.com/original-image.jpg"
        };

        var productRepositoryMock = new Mock<IProductRepository>();
        productRepositoryMock.Setup(repo => repo.GetEntityByIdAsync(productId)).ReturnsAsync(product);
        productRepositoryMock.Setup(repo => repo.UpdateAsync(productId, It.IsAny<Product>())).ReturnsAsync(true);

        var messageQueueServiceMock = new Mock<IMessageQueueService>();

        var productUseCase = new ProductUseCase(productRepositoryMock.Object, messageQueueServiceMock.Object);

        // Act
        await productUseCase.Update(productId, productRequest);

        // Assert
        productRepositoryMock.Verify(repo => repo.UpdateAsync(productId, It.IsAny<Product>()), Times.Once);
        messageQueueServiceMock.Verify(
            mq => mq.PublishMessage("Totem.Products.Updated", It.IsAny<string>()),
            Times.Once);
    }

    [Fact]
    public async Task Delete_ExistingProduct_ShouldDeleteAndPublishMessage()
    {
        // Arrange
        var productId = "123";

        var product = new Product
        {
            Id = productId,
            Name = "Product to be deleted",
            Price = 19.99m,
            Description = "Description of product to be deleted",
            Category = Category.Acompanhamento,
            ImageURL = "https://example.com/delete-image.jpg"
        };

        var productRepositoryMock = new Mock<IProductRepository>();
        productRepositoryMock.Setup(repo => repo.GetEntityByIdAsync(productId)).ReturnsAsync(product);
        productRepositoryMock.Setup(repo => repo.DeleteAsync(productId)).ReturnsAsync(true);

        var messageQueueServiceMock = new Mock<IMessageQueueService>();

        var productUseCase = new ProductUseCase(productRepositoryMock.Object, messageQueueServiceMock.Object);

        // Act
        await productUseCase.Delete(productId);

        // Assert
        productRepositoryMock.Verify(repo => repo.DeleteAsync(productId), Times.Once);
        messageQueueServiceMock.Verify(
            mq => mq.PublishMessage("Totem.Products.Deleted", It.IsAny<string>()),
            Times.Once);
    }

    [Fact]
    public async Task Delete_NonExistingProduct_ShouldThrowDomainException()
    {
        // Arrange
        var productId = "123";

        var productRepositoryMock = new Mock<IProductRepository>();
        productRepositoryMock.Setup(repo => repo.GetEntityByIdAsync(productId)).ReturnsAsync((Product)null);

        var messageQueueServiceMock = new Mock<IMessageQueueService>();

        var productUseCase = new ProductUseCase(productRepositoryMock.Object, messageQueueServiceMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => productUseCase.Delete(productId));
        productRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<string>()), Times.Never);
        messageQueueServiceMock.Verify(
            mq => mq.PublishMessage("Totem.Products.Deleted", It.IsAny<string>()),
            Times.Never);
    }

}


