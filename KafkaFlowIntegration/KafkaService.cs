using KafkaFlow;
using KafkaFlow.Consumers;
using KafkaFlow.Producers;
using System.Text.Json;
using YourNuts.Domain;
using YourNuts.Domain.Models;

namespace KafkaFlowIntegration;

public class KafkaService
{
    private readonly IProducerAccessor producer;

    private const string ProducerName = "product-producer";

    public KafkaService(IProducerAccessor producer)
    {
        this.producer = producer;
    }

    public async Task PublishAsync(string topic, dynamic message) 
    {
        var json = JsonSerializer.Serialize(message);

        var pd = producer.GetProducer(ProducerName);

        await pd.ProduceAsync(topic, json);
    }
}





