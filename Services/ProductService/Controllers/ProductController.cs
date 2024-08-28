using KafkaFlowIntegration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using YourNuts.Domain;
using YourNuts.Domain.Models;

namespace ProductService.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class ProductController : ControllerBase
{
    private readonly YourNutsDbContext _context;
    private readonly KafkaService kafkaContext;

    public ProductController(YourNutsDbContext context, KafkaService kafkaContext)
    {
        _context = context;
        this.kafkaContext = kafkaContext;
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
    public async Task<IActionResult> UpdateProduct(Guid id, UpdateProductDTO product)
    {
        var product_ = await _context.Products.FindAsync(id);
        
        if(product_ == null) return NotFound();

        _context.Entry(product_).State = EntityState.Modified;

        product_.Price = product.Price ?? product_.Price;
        product_.Name = product.Name ?? product_.Name;
        product_.Description = product.Description ?? product_.Description;
        product_.Image = product.Image ?? product_.Image;
        product_.Quantity= product.Quantity > 0 ? product.Quantity ?? product_.Quantity : 1;

        _context.Products.Update(product_);

        await _context.SaveChangesAsync();

        await kafkaContext.PublishAsync("product-updated", product_);

        return CreatedAtAction(nameof(GetProduct), new { id = product_.Id }, product_);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null) return NotFound();

        _context.Products.Remove(product);

       await _context.SaveChangesAsync();

        await kafkaContext.PublishAsync("product-deleted", product);

        return NoContent();
        
    }
}

public record class UpdateProductDTO(string? Name, string? Description, decimal? Price, byte[]? Image, int? Quantity);

