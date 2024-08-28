using System.ComponentModel.DataAnnotations;

namespace YourNuts.Domain.Models;
public class Product
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [StringLength(60)]
    public string Name { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public int Quantity { get; set; } = 1;

    [StringLength(250)]
    public string Description { get; set; }

    public byte[] Image { get; set; }

    public bool isOutOfStock { get; set; } = false;

}