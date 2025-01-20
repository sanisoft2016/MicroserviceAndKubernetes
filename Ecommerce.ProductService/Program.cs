using Confluent.Kafka;
using Ecommerce.ProductService.Data;
using Ecommerce.ProductService.Kafka;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddKafkaConsumer<string, string>("myKafkaConnection", 
    options =>
    {
        options.ConnectionString = "mykafkaconnection.default.svc.cluster.local:9092";//Minikube IP Address :192.168.49.2
        options.Config.GroupId = "order-group";
        options.Config.AutoOffsetReset = AutoOffsetReset.Earliest;
        options.Config.EnableAutoCommit = false;
    });


builder.Services.AddHostedService<KafkaConsumer>();

//builder.AddNpgsqlDbContext<ProductDbContext>("MicroServiceProductDb");

//builder.AddNpgsqlDbContext<ProductDbContext>("MicroServiceProductDb");



builder.Services.AddDbContext<ProductDbContext>(option => option.UseNpgsql(
    builder.Configuration.GetConnectionString("MicroServiceProductDb")));

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
