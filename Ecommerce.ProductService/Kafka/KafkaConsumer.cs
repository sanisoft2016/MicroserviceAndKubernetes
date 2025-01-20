
using Confluent.Kafka;
using Ecommerce.Model;
using Ecommerce.ProductService.Data;
using Newtonsoft.Json;

namespace Ecommerce.ProductService.Kafka
{
    public class KafkaConsumer(IServiceScopeFactory serviceScope, IConsumer<string, string> consumer) : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() =>
            {
                var topic = "orderTopic";//order-topic
                consumer.Subscribe(topic);
               // _ = ConsumeAsync(topic, stoppingToken);
            }, stoppingToken);
        }

        public async Task ConsumeAsync(string topic, CancellationToken stoppingToken)
        {
            //consumer.Subscribe(topic);
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consummerResult = consumer.Consume(stoppingToken);
                    var order = JsonConvert.DeserializeObject<OrderModel>(consummerResult.Message.Value);
                    using var scope = serviceScope.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
                    var product = await dbContext.Products.FindAsync(order.ProductId);

                    if (product != null)
                    {
                        product.Quantity -= order.Quantity;
                        _ = dbContext.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            consumer.Close();
        }
    }
}
