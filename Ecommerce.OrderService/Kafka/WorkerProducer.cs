using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using static Confluent.Kafka.ConfigPropertyNames;

namespace Ecommerce.OrderService.Kafka
{
    public interface IWorkerProducer
    {
        Task produceAsync(string topic, Message<string, string> message);
    }
    public  class WorkerProducer(IProducer<string, string> producer):IWorkerProducer
    {

        public Task produceAsync(string topic, Message<string, string> message)
        {
            try
            {
                var response = producer.ProduceAsync(topic, message);
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
    
}
