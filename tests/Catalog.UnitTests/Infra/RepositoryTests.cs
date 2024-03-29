using Infra.Repositories;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using Xunit;

namespace Catalog.UnitTests.Infra;

public class RepositoryTests
{

    [Fact]
    public async Task AddAsync_ShouldAddEntityToCollection()
    {
        // Arrange
        var entity = new Product();

        var databaseMock = new Mock<IMongoDatabase>();
        var collectionMock = new Mock<IMongoCollection<Product>>();

        var mongoClientMock = new Mock<IMongoClient>();
        mongoClientMock.Setup(client => client.GetDatabase(It.IsAny<string>(), null)).Returns(databaseMock.Object);
        databaseMock.Setup(db => db.GetCollection<Product>(It.IsAny<string>(), null)).Returns(collectionMock.Object);

        var configMock = new Mock<IConfiguration>();

        var repository = new Repository<Product>(configMock.Object, mongoClientMock.Object)
        {
        };

        // Act
        await repository.AddAsync(entity);

        // Assert
        collectionMock.Verify(
            c => c.InsertOneAsync(It.IsAny<Product>(), It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Get_ShouldReturnListOfEntities()
    {
        // Arrange
        var entities = new List<Product> { new Product(), new Product() };

        var cursorMock = new Mock<IAsyncCursor<Product>>(); 
        cursorMock.Setup(c => c.Current).Returns(entities);


        cursorMock.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);

        var collectionMock = new Mock<IMongoCollection<Product>>();
        collectionMock.Setup(c => c.FindAsync(It.IsAny<FilterDefinition<Product>>(), It.IsAny<FindOptions<Product, Product>>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(cursorMock.Object);
        
        var databaseMock = new Mock<IMongoDatabase>();
        databaseMock.Setup(db => db.GetCollection<Product>(It.IsAny<string>(), null)).Returns(collectionMock.Object);

        var mongoClientMock = new Mock<IMongoClient>();
        mongoClientMock.Setup(client => client.GetDatabase(It.IsAny<string>(), null)).Returns(databaseMock.Object);

        var configMock = new Mock<IConfiguration>();


        var repository = new Repository<Product>(configMock.Object, mongoClientMock.Object)
        {
        };

        // Act
        var result = await repository.Get();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entities.Count, result.Count);
    }

}