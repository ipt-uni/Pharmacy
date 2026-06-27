namespace pharmacy.Data.Models.ViewModels;

/// <summary>
/// API DTO for a single cart item, including medicine name and computed line total.
/// </summary>
public class CartItemDto
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public int MedicineId { get; set; }
    public string MedicineName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal => Quantity * UnitPrice;
}
