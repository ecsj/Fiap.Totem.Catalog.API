using Infra.MessageQueue;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Xunit;
using Microsoft.Extensions.Configuration;

namespace Catalog.UnitTests.Infra;
public class RabbitMQServiceTests
{
    [Fact]
    public void PublishMessage_ShouldDeclareQueueAndPublishMessage()
    {
        // Arrange
        var configurationMock = new Mock<IConfiguration>();
        //configurationMock.Setup(c => c.GetConnectionString("RabbitMQ")).Returns("fake_connection_string");

        var connectionFactoryMock = new Mock<IConnectionFactory>();
        var connectionMock = new Mock<IConnection>();
        var channelMock = new Mock<IModel>();

        connectionFactoryMock.Setup(f => f.CreateConnection()).Returns(connectionMock.Object);
        connectionMock.Setup(c => c.CreateModel()).Returns(channelMock.Object);

        var rabbitMQService = new RabbitMQService(configurationMock.Object, connectionFactoryMock.Object);

        // Act
        rabbitMQService.PublishMessage("test_queue", "Hello, RabbitMQ!");

        // Assert
        channelMock.Verify(c => c.QueueDeclare("test_queue", true, false, false, null), Times.Once);
        //channelMock.Verify(c => c.BasicPublish("", "test_queue", null, It.IsAny<byte[]>()), Times.Once);
    }

    //[Fact]
    //public void ConsumeMessages_ShouldDeclareQueueAndInvokeCallback()
    //{
    //    // Arrange
    //    var configurationMock = new Mock<IConfiguration>();
    //    //configurationMock.Setup(c => c.GetConnectionString("RabbitMQ")).Returns("fake_connection_string");

    //    var connectionFactoryMock = new Mock<IConnectionFactory>();
    //    var connectionMock = new Mock<IConnection>();
    //    var channelMock = new Mock<IModel>();
    //    var consumerMock = new Mock<EventingBasicConsumer>(channelMock.Object);

    //    connectionFactoryMock.Setup(f => f.CreateConnection()).Returns(connectionMock.Object);
    //    connectionMock.Setup(c => c.CreateModel()).Returns(channelMock.Object);
    //    channelMock.Setup(c => c.BasicConsume("test_queue", true, It.IsAny<IBasicConsumer>())).Callback<string, bool, IBasicConsumer>((queue, autoAck, consumer) =>
    //    {
    //        consumer.HandleBasicDeliver("consumer_tag", 1, false, "", "", null, new byte[] { });
    //    });

    //    var rabbitMQService = new RabbitMQService(configurationMock.Object, connectionFactoryMock.Object);

    //    // Act
    //    rabbitMQService.ConsumeMessages("test_queue", message => { });

    //    // Assert
    //    channelMock.Verify(c => c.QueueDeclare("test_queue", true, false, false, null), Times.Once);
    //    //channelMock.Verify(c => c.BasicConsume("test_queue", true, It.IsAny<EventingBasicConsumer>()), Times.Once);
    //    consumerMock.VerifyAdd(c => c.Received += It.IsAny<EventHandler<BasicDeliverEventArgs>>(), Times.Once);
    //}

    [Fact]
    public void CloseConnection_ShouldCloseChannelAndConnection()
    {
        // Arrange
        var configurationMock = new Mock<IConfiguration>();
        //configurationMock.Setup(c => c.GetConnectionString("RabbitMQ")).Returns("fake_connection_string");

        var connectionFactoryMock = new Mock<IConnectionFactory>();
        var connectionMock = new Mock<IConnection>();
        var channelMock = new Mock<IModel>();

        connectionFactoryMock.Setup(f => f.CreateConnection()).Returns(connectionMock.Object);
        connectionMock.Setup(c => c.CreateModel()).Returns(channelMock.Object);

        var rabbitMQService = new RabbitMQService(configurationMock.Object, connectionFactoryMock.Object);

        // Act
        rabbitMQService.CloseConnection();

        // Assert
        channelMock.Verify(c => c.Close(), Times.Once);
        connectionMock.Verify(c => c.Close(), Times.Once);
    }
}