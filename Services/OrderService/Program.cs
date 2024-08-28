using YourNuts.KafkaFlowIntegration.Options;
using Microsoft.EntityFrameworkCore;
using YourNuts.Domain;
using KafkaFlow;
using Microsoft.Extensions.Options;
using KafkaFlow.Serializer;
using Domain;
using KafkaFlowIntegration;

var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<YourNutsDbContext>(options =>
{
    options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<KafkaService>();

builder.Services.AddOptions<KafkaOptions>().Bind(Configuration.GetSection(KafkaOptions.SectionName));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddKafka(kafka =>
{
    var options = builder
        .Services?.BuildServiceProvider()
        .GetRequiredService<IOptions<KafkaOptions>>()
        .Value;

    options?.Register(kafka, cluster =>
            cluster
                .AddProducer("product-producer", producer => producer
                .WithAcks(Acks.Leader)
                .DefaultTopic("order-created")
                .AddMiddlewares(middlewares =>
                            middlewares
                                .AddSerializer<JsonCoreSerializer>()))
                 .AddConsumer(consumer => consumer
                    .Topic("product-updated")
                    .WithGroupId("products")
                    .WithBufferSize(100)
                    .WithWorkersCount(10)
                    .AddMiddlewares(middleware => middleware
                        .AddDeserializer<JsonCoreDeserializer>()
                        .AddTypedHandlers(handler => handler
                        .AddHandlersFromAssemblyOf<ProductConsumerHandler>()
                        .WithHandlerLifetime(InstanceLifetime.Scoped)
                        .WhenNoHandlerFound(context => Console.WriteLine(
                             "Message not handled > Partition: {0} | Offset: {1}",
                              context.ConsumerContext.Partition,
                              context.ConsumerContext.Offset)))))
                ,topics: ["product-updated", "product-deleted", "order-created", "order-updated"]);
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var kafkaBus = app.Services.CreateKafkaBus();
await kafkaBus.StartAsync();

/*
using (var scope = app.Services.CreateScope())
{
    // Start Kafka
    var kafkaOptions = scope.ServiceProvider.GetRequiredService<IOptions<KafkaOptions>>();
    if (kafkaOptions.Value.Enabled)
    {
    }
}
*/

app.Run();