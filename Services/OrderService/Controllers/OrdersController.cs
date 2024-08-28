using Domain;
using Domain.Models.DTos;
using KafkaFlowIntegration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using YourNuts.Domain;
using YourNuts.Domain.Models;

namespace OrderService.Controllers;



[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly YourNutsDbContext _context;
    private readonly KafkaService kafkaContext;
    private readonly IRepository<Order> _ordersRepository;
    private readonly IRepository<OrderProduct> _orderProductsRepository;
    private readonly IRepository<Product> _productRepository;

    public OrdersController(YourNutsDbContext context, KafkaService kafkaContext, IRepository<Product> productRepository, IRepository<OrderProduct> orderProductsRepository, IRepository<Order> ordersRepository)
    {
        _context = context;
        this.kafkaContext = kafkaContext;
        _productRepository = productRepository;
        _orderProductsRepository = orderProductsRepository;
        _ordersRepository = ordersRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _context.OrderProducts.Include(o => o.Order).Include(o => o.Product).ToListAsync();

        return Ok(orders);
    }
    // POST api/orders
    [HttpPost]
    public async Task<ActionResult> CreateOrder(CreateOrderDTO order)
    {
        try
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Create a new order
            var newOrder = new Order
            {
                Id = Guid.NewGuid(),
                TotalPrice = order.TotalPrice,
                Email = order.Email,
                FirstName = order.FirstName,
                LastName = order.LastName,
                PhoneNumber = order.PhoneNumber
            };

            // Add the order to the database
            _context.Orders.Add(newOrder);

            var newOrderProduct = new OrderProduct
            {
                OrderId = newOrder.Id,
                ProductId = order.ProductId,
                Quantity = order.Quantity,
                Id = Guid.NewGuid()
            };

            // Add the order product to the database
            _context.OrderProducts.Add(newOrderProduct);

            await kafkaContext.PublishAsync("order-created", new { ProductId = newOrderProduct.ProductId, Quantity = newOrderProduct.Quantity });

            await _context.SaveChangesAsync();

            return Ok(new { orderId = newOrder.Id, status = "Successfull" });
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to create a new order");
            return Ok(new { orderId = order.ProductId, status = "Failed to show data.", message= ex });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(Guid id)
    {
        var order = await _context.OrderProducts
            .Include(o => o.Order)
            .Include(o => o.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }
      
        return Ok(order);
    }

}

public record class CreateOrderDTO(Guid ProductId, double TotalPrice, int Quantity, string Email, string FirstName, string LastName, string PhoneNumber );
public record class UpdateOrderDTO(Guid ProductId, double? TotalPrice, int? Quantity, string? Email, string? FirstName, string? LastName, string? PhoneNumber );