using Confluent.Kafka;
using Ecommerce.Model;
using Ecommerce.ProductService.Data;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Ecommerce.ProductService
{
    public interface IWorker
    {
        Task ConsumeAsync(string topic, CancellationToken stoppingToken);
    }
    internal sealed class Worker(IConsumer<string, string> consumer, IServiceScopeFactory serviceScope) : BackgroundService, IWorker
    {
        // Use consumer...
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() =>
            {
                _ = ConsumeAsync("order-topic", stoppingToken);

            }, stoppingToken);
        }

        public async Task ConsumeAsync(string topic, CancellationToken stoppingToken)
        {
            //var config = new ConsumerConfig
            //{
            //    GroupId = "order-group",
            //    BootstrapServers = "localhost:29092",
            //    AutoOffsetReset = AutoOffsetReset.Earliest,
            //};
            //using var consumer = new ConsumerBuilder<string, string>(config).Build();
            
            consumer.Subscribe(topic);

            while (!stoppingToken.IsCancellationRequested)
            {
                var consummerResult = consumer.Consume(stoppingToken);
                var order = JsonConvert.DeserializeObject<OrderModel>(consummerResult.Message.Value);
                using var scope = serviceScope.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
                var product = await dbContext.Products.FindAsync(order.ProductId);

                if (product != null)
                {
                    product.Quantity -= order.Quantity;
                    _ = dbContext.SaveChangesAsync();
                }
            }
            consumer.Close();
        }
    }
}
