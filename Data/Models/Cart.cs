using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pharmacy.Data.Models;

public class Cart
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// The total cost of the cart: sum(cartitems.cost)
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalCost { get; set; }

    /// <summary>
    /// The total quantity of the cart: sum(cartitems.quantity)
    /// </summary>
    [Required]
    public int TotalQuantity { get; set; }

    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    /// <summary>
    /// A null payment means that the cart is not paid yet
    /// </summary>
    public Payment? Payment { get; set; }
}
