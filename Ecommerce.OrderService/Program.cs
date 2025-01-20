using Ecommerce.OrderService.Data;
using Ecommerce.OrderService.Kafka;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



//builder.AddNpgsqlDbContext<OrderDbContext>("MicroServiceOrderDb");

builder.Services.AddDbContext<OrderDbContext>(option =>
option.UseNpgsql(builder.Configuration.GetConnectionString("MicroServiceOrderDb")));


//builder.Services.AddDbContext<OrderDbContext>(option => option.UseSqlServer("Data Source=DESKTOP-JGNBP4O;Initial Catalog=MicroServiceOrderDb;User ID=sa;Password=BasH;Trust Server Certificate=True"));
builder.Services.AddScoped<IKafkaProducer, KafkaProducer>();
builder.AddKafkaProducer<string, string>("myKafkaConnection",
        option =>
        {
            option.ConnectionString = "mykafkaconnection.default.svc.cluster.local:9092";//Minikube IP Address :192.168.49.2
        });

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
