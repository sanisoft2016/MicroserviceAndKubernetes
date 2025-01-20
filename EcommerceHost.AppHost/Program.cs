using Aspire.Hosting;
//using Ecommerce.OrderService.Kafka;
//using Ecommerce.OrderService.Kafka;

var builder = DistributedApplication.CreateBuilder(args);

var kafkaConnection = builder.AddKafka("myKafkaConnection")
    .WithKafkaUI(kafkaUI => kafkaUI.WithHostPort(9100));

var postgres = builder.AddPostgres("postgres28th")
    .WithDataVolume(isReadOnly: false)
    .WithPgAdmin();


//var postgres = builder.AddPostgres("postgres")
//                      .WithDataVolume(isReadOnly: false);

var orderDb = postgres.AddDatabase("MicroServiceOrderDb");
var orderservice = builder.AddProject<Projects.Ecommerce_OrderService>("ecommerce-orderservice")
.WithReference(kafkaConnection)
    .WithReference(orderDb);

var productDb = postgres.AddDatabase("MicroServiceProductDb");
var productservice = builder.AddProject<Projects.Ecommerce_ProductService>("ecommerce-productservice")
        .WithReference(kafkaConnection)
    .WithReference(productDb);


//var dbOrder = builder.AddPostgres("dbOrder").WithPgAdmin();

//var orderServer = builder.AddPostgres("orderServer")
//    .WithDataVolume()
//    .WithPgAdmin();


//var cache = builder.AddRedis("cache");
//var kafka = builder.Add

builder.AddProject<Projects.Ecommerce_Web>("ecommerce-web")
        .WithReference(orderservice)
    .WithReference(productservice);
//.WithReference(cache);

try
{
    builder.Build().Run();
}
catch (Exception ex)
{
    throw;
}

