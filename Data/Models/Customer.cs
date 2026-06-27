using System.ComponentModel.DataAnnotations;

namespace pharmacy.Data.Models;

/// <summary>
/// Profile extension for users with the Customer role.
/// Includes address and phone details needed for orders.
/// </summary>
public class Customer : MyUser
{
    [Required]
    [StringLength(200)]
    public string Address { get; set; } = string.Empty;

    [Phone]
    [StringLength(20)]
    public string? PhoneNumber { get; set; }
}
