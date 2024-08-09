using KafkaFlow;
using System.Text.Json;
using KafkaFlow.Producers;
using Microsoft.Extensions.DependencyInjection;

using YourNuts.Domain.Models;

namespace ProductService.Service;

public class ProductEventsProducer
{

    /*
    public ProductEventsProducer(IMessageProducer<ProductEventsProducer> producer)
    {
        _producer = producer;
    }    
    public async Task ProduceProductUpdatedAsync( Product product, string operation, string topic = "product-updated")
    {
        await _producer.ProduceAsync(topic, new {product=product, operation=operation});
    }
    */
}
