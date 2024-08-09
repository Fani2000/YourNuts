using YourNuts.KafkaFlowIntegration.Options;
using Microsoft.EntityFrameworkCore;
using YourNuts.Domain;
using KafkaFlow;
using Microsoft.Extensions.Options;
using ProductService.Service;
using KafkaFlow.Serializer;

var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<YourNutsDbContext>(options =>
{
    options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
});

// builder.Services.AddScoped<ProductEventsProducer>();

builder.Services.AddOptions<KafkaOptions>().Bind(Configuration.GetSection(KafkaOptions.SectionName));

builder.Services.AddKafka(kafka =>
{
    var options = builder
        .Services?.BuildServiceProvider()
        .GetRequiredService<IOptions<KafkaOptions>>()
        .Value;

    options?.Register(kafka, cluster =>
            cluster
                .WithBrokers(new[] { "localhost:9092" })
                .CreateTopicIfNotExists("product-udated")
                .AddProducer("product-producer", producer => producer
                .WithAcks(Acks.Leader)
                .DefaultTopic("product-udated")
                .AddMiddlewares(middlewares=>
                            middlewares
                                .AddSerializer<JsonCoreSerializer>())), 
                topics: ["product-updated", "product-deleted"]);
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    // Start Kafka
    var kafkaOptions = scope.ServiceProvider.GetRequiredService<IOptions<KafkaOptions>>();
    if (kafkaOptions.Value.Enabled)
    {
        var kafkaBus = app.Services.CreateKafkaBus();
        await kafkaBus.StartAsync();
    }
}

app.Run();
