using Confluent.Kafka;

namespace Ecommerce.OrderService.Kafka
{
    public interface IKafkaProducer
    {
        DeliveryResult<string, string>? produceAsync(string topic, Message<string, string> message);
    }
    public class KafkaProducer(IProducer<string, string> producer): IKafkaProducer
    {
        public DeliveryResult<string, string>? produceAsync(string topic, Message<string, string> message)
        {
            var resukt = producer.ProduceAsync(topic, message).Result;
            return resukt;
        }

        
    }
}
