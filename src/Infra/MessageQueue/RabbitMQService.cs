using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infra.MessageQueue;

public class RabbitMQService : IMessageQueueService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMQService(IConfiguration configuration, IConnectionFactory connectionFactory)
    {

        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public void PublishMessage(string queue, string message)
    {
        _channel.QueueDeclare(queue, durable: true, false, false, null);

        var body = System.Text.Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(
            exchange: "",
            routingKey: queue,
            basicProperties: null,
            body: body
        );
    }

    public void ConsumeMessages(string queue, Action<string> callback)
    {
        _channel.QueueDeclare(queue, durable: true, false, false, null);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = System.Text.Encoding.UTF8.GetString(body);
            callback(message);
        };

        _channel.BasicConsume(queue, autoAck: true, consumer: consumer);
    }

    public void CloseConnection()
    {
        _channel.Close();
        _connection.Close();
    }
}
