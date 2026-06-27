namespace pharmacy.Data.Models.ViewModels;

/// <summary>
/// API DTO for Cart, including computed totals and nested items.
/// </summary>
public class CartDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalCost { get; set; }
    public int TotalQuantity { get; set; }
    public bool IsPaid { get; set; }
    public List<CartItemDto> Items { get; set; } = new();
}
