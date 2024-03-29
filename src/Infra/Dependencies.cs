using System.Diagnostics.CodeAnalysis;
using Application.Interfaces;
using Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Domain.Repositories.Base;
using Infra.MessageQueue;
using Infra.Repositories;
using MongoDB.Driver;
using RabbitMQ.Client;
using Infra.Data.Repositories;

namespace Infra
{
    [ExcludeFromCodeCoverage]
    public class Dependencies
    {
        public static IServiceCollection ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddSingleton<IMessageQueueService, RabbitMQService>();

            services.AddSingleton<IMongoClient>(sp =>
            {
                return new MongoClient(configuration.GetConnectionString("mongoDb"));
            });

            services.AddSingleton<IConnectionFactory>(sp =>
            {
                var rabbitConfig = configuration.GetConnectionString("RabbitMQ");
                var factory = new ConnectionFactory
                {
                    Uri = new Uri(rabbitConfig)
                };
                return factory;
            });

            return services;
        }
    }
}
