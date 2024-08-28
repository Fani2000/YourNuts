using System.ComponentModel.DataAnnotations;

namespace YourNuts.Domain.Models;

// Customer.cs
public class Customer
{
    [Key]
    public Guid Id { get; set; }

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

    public Order? Orders { get; set; }
}