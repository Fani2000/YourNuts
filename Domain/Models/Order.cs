using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourNuts.Domain.Models;

public class Order
{
    [Key]
    public Guid Id { get; set; }

    public DateTime OrderDate = DateTime.Now;

    public double TotalPrice { get; set; } = 100;

    [Required]
    [StringLength(50)]
    public string? FirstName { get; set; }

    [Required]
    [StringLength(50)]
    public string? LastName { get; set; }

    [Required]
    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(20)]
    public string? PhoneNumber { get; set; }
}
