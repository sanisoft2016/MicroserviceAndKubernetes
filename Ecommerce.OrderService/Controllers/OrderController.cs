using Ecommerce.Model;
using Ecommerce.OrderService.Data;
using Ecommerce.OrderService.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Ecommerce.OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // public class OrderController(OrderDbContext dbContext) : ControllerBase
    public class OrderController(OrderDbContext dbContext, IKafkaProducer kafkaProducer) : ControllerBase
    {
        [HttpGet("list")]
        public async Task<IActionResult> GetOrders()
        {
            var result = await dbContext.Orders.ToListAsync();
            return Ok(result);
        }

        [HttpPost("CreateOrder")]//async Task<OrderModel> CreateOrder(OrderModel order)
        public async Task<IActionResult> CreateOrder(OrderModel order) 
        {

            order.OrderDate = DateTime.Now.ToUniversalTime();
            dbContext.Orders.Add(order);

            try
            {
                await dbContext.SaveChangesAsync();
                //var workerProducer  = workerProducer.ExecuteAsync();

                var response = kafkaProducer.produceAsync("orderTopic", new Confluent.Kafka.Message<string, string>
                {
                    Key = order.Id.ToString(),
                    Value = JsonConvert.SerializeObject(order)

                });
                return Ok(order);
            }
            catch (Exception ex)
            {
                return Ok(new OrderModel());
                throw;
            }
            
        }
    }
}
