using KafkaFlow;
using KafkaFlow.Producers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Service;
using System.Text.Json;
using YourNuts.Domain;
using YourNuts.Domain.Models;

namespace ProductService.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class ProductController : ControllerBase
{
    private readonly YourNutsDbContext _context;

    private readonly IProducerAccessor producer;

    public ProductController(YourNutsDbContext context, IProducerAccessor producer)
    {
        _context = context;
        this.producer = producer;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _context.Products.ToListAsync();

        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(Guid id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null) return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(Product product)
    {
        _context.Products.Add(product);

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(Guid id, Product product)
    {
        // if (id != product.Id) return BadRequest();

        var product_ = await _context.Products.FindAsync(id);
        
        if(product_ == null) return NotFound();

        _context.Entry(product_).State = EntityState.Modified;

        product_.Price = product.Price;
        product_.Name = product.Name;
        product_.Description = product.Description;
        product_.Image = product.Image;

        _context.Products.Update(product_);

        await _context.SaveChangesAsync();

        var json = JsonSerializer.Serialize(product_);

        var pd = producer.GetProducer("product-producer");

        await pd.ProduceAsync("product-updated", json);

        return CreatedAtAction(nameof(GetProduct), new { id = product_.Id }, product_);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null) return NotFound();

        _context.Products.Remove(product);

       await _context.SaveChangesAsync();

        await producer["product-deleted"].ProduceAsync("product-deleted", product);

        return NoContent();
        
    }
}
