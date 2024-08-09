
namespace Domain.Models.DTos;

public class OrderDto
{
    public Guid Id { get; set; }
    public DateTime OrderDate { get; set; }
    public CustomerDto? Customer { get; set; }
    public List<OrderProductDto>? OrderProducts { get; set; }
}

public class OrderProductDto
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public ProductDto? Product { get; set; }
    public int Quantity { get; set; }
}

public class CustomerDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
}

public class ProductDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
}
