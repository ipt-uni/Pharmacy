using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pharmacy.Data.Models;

public class Customer : MyUser
{
    [Required]
    [StringLength(200)]
    public string Address { get; set; } = string.Empty;

    [Phone]
    [StringLength(20)]
    public string? PhoneNumber { get; set; }
}
