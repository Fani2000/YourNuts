
using Microsoft.EntityFrameworkCore;
using YourNuts.Domain.Models;

namespace YourNuts.Domain;

public class YourNutsDbContext : DbContext
{

    public YourNutsDbContext(DbContextOptions<YourNutsDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<OrderProduct>()
                   .HasKey(op => new { op.OrderId, op.ProductId });

    }

    public DbSet<Product> Products{ get; set; }
    public DbSet<Order> Orders{ get; set; }
    public DbSet<OrderProduct> OrderProducts{ get; set; }
    public DbSet<Customer> Customers{ get; set; }
}
