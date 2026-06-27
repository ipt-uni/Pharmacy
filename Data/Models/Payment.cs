using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pharmacy.Data.Models;

/// <summary>
/// A payment record tied to a cart. Once created, the cart is considered paid.
/// </summary>
public class Payment
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Foreign key to the cart being paid for.
    /// </summary>
    [Required]
    [ForeignKey("Cart")]
    public int CartId { get; set; }
    public Cart? Cart { get; set; }

    /// <summary>
    /// The amount of the payment
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }
}
