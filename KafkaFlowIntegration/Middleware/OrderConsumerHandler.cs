using Domain;
using KafkaFlow;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using YourNuts.Domain.Models;

namespace KafkaFlowIntegration;

public class OrderConsumerHandler : IMessageHandler<string>, IOrderConsumerHandler
{
    private readonly IRepository<Product> _productRepository;

    public OrderConsumerHandler(IRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public Task Handle(IMessageContext context, string message)
    {
        Console.WriteLine("Message: " + message);

        var orderProduct = JsonSerializer.Deserialize<OrderDTO>(message);

        if (orderProduct == null) return Task.CompletedTask;

        var product = _productRepository.Get(orderProduct.ProductId);

        if (product == null) return Task.CompletedTask;

        if (product.Quantity == 0) product.isOutOfStock = true;
        else product.isOutOfStock = false;

        if (product.Quantity > 0)
            product.Quantity -= orderProduct.Quantity;
        else
            throw new Exception("Failed to create a new order");

        _productRepository.Update(product);

        return Task.CompletedTask;
    }
}
