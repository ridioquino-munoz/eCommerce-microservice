
using API.Inventory.DTO;
using Business.Inventory;
using Domain.Inventory;
using Microsoft.Identity.Client;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace API.Inventory
{
    public class ProductCreatedConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public ProductCreatedConsumer(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };

            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: "product-created",
                durable: true,
                exclusive: false,
                autoDelete: false
            );

           var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (sender, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);

                var product = JsonSerializer.Deserialize<ProductCreatedEvent>(json);

                using var scope = _serviceScopeFactory.CreateScope();

                var db = scope.ServiceProvider.GetRequiredService<InventoryDBContext>();

                db.ProductInventories.Add(
                    new ProductInventory { 
                        ProductID = product!.ProductId,
                        Quantity = 0
                    });

                await db.SaveChangesAsync();

            };

            await channel.BasicConsumeAsync(
                queue: "product-created",
                autoAck: true,
                consumer: consumer
            );

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
