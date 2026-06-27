using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace pharmacy.Data.Models;

public class Cart
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int CustomerId { get; set; }

    [ForeignKey(nameof(CustomerId))]
    public Customer Customer { get; set; } = null!;

    /// <summary>
    /// The total cost of the cart: sum(cartitems.cost)
    /// </summary>
    [NotMapped]
    public decimal TotalCost => CartItems?.Sum(ci => ci.Quantity * ci.UnitPrice) ?? 0;

    /// <summary>
    /// The total quantity of the cart: sum(cartitems.quantity)
    /// </summary>
    [NotMapped]
    public int TotalQuantity => CartItems?.Sum(ci => ci.Quantity) ?? 0;

    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    /// <summary>
    /// A null payment means that the cart is not paid yet
    /// </summary>
    public Payment? Payment { get; set; }
}
