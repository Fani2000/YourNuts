using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourNuts.Domain;
using YourNuts.Domain.Models;

namespace OrderService.Controllers;

/*
[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly YourNutsDbContext _context;

    public CustomersController(YourNutsDbContext context)
    {
        _context = context;
    }

    // POST api/customers
    [HttpPost]
    public async Task<ActionResult<Customer>> CreateCustomer(Customer customer)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
    }

    // GET api/customers/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> GetCustomer(Guid id)
    {
        var customer = await _context.Customers.FindAsync(id);

        if (customer == null)
        {
            return NotFound();
        }

        return customer;
    }
}
*/