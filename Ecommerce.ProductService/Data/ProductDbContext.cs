using Ecommerce.Model;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.ProductService.Data
{
    public class ProductDbContext : DbContext
    {


        //public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        //{
        //}

        

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);
        //}

        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {
             Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductModel>().HasData(new ProductModel { Id = 1, Name = "Shirt", Price = 200, Quantity = 2 });
            modelBuilder.Entity<ProductModel>().HasData(new ProductModel { Id = 2, Name = "Pant", Price = 300, Quantity = 5 });
            modelBuilder.Entity<ProductModel>().HasData(new ProductModel { Id = 3, Name = "Shoes", Price = 100, Quantity = 7 });

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<ProductModel> Products { get; set; }
    }



    public static class Extensions
    {
        public static void CreateDbIfNotExists(this IHost host)
        {
            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<ProductDbContext>();
            try
            {
                context.Database.EnsureCreated();
                DbInitializer.Initialize(context);

            }
            catch (Exception ex)
            {
            }
        }


        public static class DbInitializer
        {
            public static void Initialize(ProductDbContext context)
            {
                if (context.Products.Any())
                    return;

                var products = new List<ProductModel>
                {
                    new ProductModel {  Id = 1, Name = "Shirt", Price = 200, Quantity = 2 },
                    new ProductModel { Id = 2, Name = "Pant", Price = 300, Quantity = 5 },
                    new ProductModel {Id = 1, Name = "Shirt", Price = 200, Quantity = 2},

                };

                context.AddRange(products);

                context.SaveChanges();
            }
        }
    }
}
