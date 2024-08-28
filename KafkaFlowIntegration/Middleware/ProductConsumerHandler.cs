using Domain;
using KafkaFlow;
using KafkaFlow.Consumers;
using System.Text.Json;
using YourNuts.Domain;
using YourNuts.Domain.Models;

namespace KafkaFlowIntegration;

public class ProductConsumerHandler : IMessageHandler<string>, IProductConsumerHandler
{
    private readonly IConsumerAccessor _consumerAccessor;
    private readonly IRepository<Order> _ordersRepository;
    private readonly IRepository<OrderProduct> _ordersProductRepository;
    private readonly YourNutsDbContext _context;
    private readonly IRepository<Product> _productRepository;

    public ProductConsumerHandler(IConsumerAccessor consumerAccessor, IRepository<Order> ordersRepository, IRepository<OrderProduct> ordersProductRepository, YourNutsDbContext context, IRepository<Product> productRepository)
    {
        _consumerAccessor = consumerAccessor;
        _ordersRepository = ordersRepository;
        _ordersProductRepository = ordersProductRepository;
        _context = context;
        _productRepository = productRepository;
    }

    public Task Handle(IMessageContext context, string message)
    {
        Console.WriteLine("Message: " + message);

        var messageInfo = JsonSerializer.Deserialize<ProductDTO>(message);

        if (messageInfo == null) return Task.CompletedTask;

        var product = _productRepository.Get(messageInfo.Id);

        if (messageInfo.Quantity > 0)
        {
            product!.Quantity = messageInfo.Quantity;
            product!.isOutOfStock = false;
        }else
        {
            product!.Quantity = messageInfo.Quantity;
            product!.isOutOfStock = true;
        }

        product.Name = messageInfo.Name ?? product.Name;
        product.Description = messageInfo.Description ?? product.Description;
        product.Image = messageInfo.Images ?? product.Image;
        product.Price = messageInfo.Price ?? product.Price;

        _productRepository.Update(product);

        return Task.CompletedTask;
    }
}
