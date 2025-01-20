using Confluent.Kafka;

namespace Ecommerce.OrderService.Kafka
{
    public interface IKafkaProducer
    {
        Task produceAsync(string topic, Message<string, string> message);
    }
    public class KafkaProducer : IKafkaProducer
    {
        private readonly IProducer<string, string> _producer;
        public KafkaProducer()
        {
            var config = new ConsumerConfig
            {
                GroupId = "order-group",
                BootstrapServers = "Kafker",
                AutoOffsetReset = AutoOffsetReset.Earliest,
            };
            _producer = new ProducerBuilder<string, string>(config).Build();

        }
        public Task produceAsync(string topic, Message<string, string> message)
        {
            return _producer.ProduceAsync(topic, message);
        }
    }
}
