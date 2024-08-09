using Domain.Models.DTos;
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

    public OrdersController(YourNutsDbContext context)
    {
        _context = context;
    }

    // POST api/orders
    [HttpPost]
    public async Task<ActionResult> CreateOrder(Order order)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Create a new order
        var newOrder = new Order
        {
            CustomerId = order.CustomerId,
            Id = Guid.NewGuid(),
        };

        // Add the order to the database
        _context.Orders.Add(newOrder);

        await _context.SaveChangesAsync();

        // Create order products
        foreach (var orderProduct in order.OrderProducts!)
        {
            // Find the product
            var product = await _context.Products.FindAsync(orderProduct.ProductId);

            if (product == null)
            {
                return NotFound("Product not found");
            }

            // Create a new order product
            var newOrderProduct = new OrderProduct
            {
                Id = Guid.NewGuid(),
                OrderId = newOrder.Id,
                ProductId = orderProduct.ProductId,
                Quantity = orderProduct.Quantity
            };

            // Add the order product to the database
            _context.OrderProducts.Add(newOrderProduct);

            await _context.SaveChangesAsync();
        }


        return Ok(new {orderId = newOrder.Id, status= "Successfull"});
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetOrder(Guid id)
    {
        var order = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderProducts!)
            .ThenInclude(op => op.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        var orderDto = new OrderDto
        {
            Id = order.Id,
            OrderDate = order.OrderDate,
            Customer = new CustomerDto
            {
                Id = order.Customer!.Id,
                Name = order.Customer.FirstName,
            },
            OrderProducts = order.OrderProducts!.Select(op => new OrderProductDto
            {
                Id = op.Id,
                OrderId = op.OrderId,
                Product = new ProductDto
                {
                    Id = op.Product!.Id,
                    Name = op.Product.Name
                },
                Quantity = op.Quantity
            }).ToList()
        };

        return orderDto;
    }
}