using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourNuts.Domain.Models;

public class Order
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid CustomerId { get; set; }

    public DateTime OrderDate = DateTime.Now;

    [ForeignKey("CustomerId")]
    public Customer? Customer { get; set; }

    public ICollection<OrderProduct>? OrderProducts { get; set; }
}
