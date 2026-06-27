using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pharmacy.Data.Models;

/// <summary>
/// A single line item within a cart, linking a medicine with a quantity and price snapshot.
/// </summary>
public class CartItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int CartId { get; set; }
    public Cart? Cart { get; set; }

    [Required]
    [ForeignKey(nameof(Medicine))]
    public int MedicineId { get; set; }
    public Medicine Medicine { get; set; } = null!;

    /// <summary>
    /// The how many medicine to buy
    /// </summary>
    [Required]
    public int Quantity { get; set; }

    /// <summary>
    /// The unit price at time of adding to cart (snapshot from Medicine.RetailPrice)
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }
}
