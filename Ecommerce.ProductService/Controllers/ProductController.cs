using Ecommerce.Model;
using Ecommerce.ProductService.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Confluent.Kafka.ConfigPropertyNames;

namespace Ecommerce.ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(ProductDbContext dbContext) : ControllerBase
    {
        [HttpPost]
        public IActionResult AddProduct([FromBody] object product)
        {
            return Ok(new { Message = "Product added successfully", Product = product });
        }

        [HttpGet("list")]//Task<List<ProductModel>>
        public async Task<IActionResult> GetProductsAsync()
        {
            try
            {
                var pauseNc = await dbContext.Products.ToListAsync();
                return Ok(pauseNc);
               // return pauseNc;
            }
            catch (Exception ex)
            {
                var responseValue = new List<ProductModel>();
                return Ok(responseValue);
               // throw;
            }
        }

        [HttpGet("{id}")]// Task<ProductModel>
        public async Task<IActionResult> GetProductById(int id)
        {
            var response = await dbContext.Products.FindAsync(id);
            return Ok(response);
        }

    }
}
