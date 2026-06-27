namespace pharmacy.Data.Models.ViewModels;

/// <summary>
/// API DTO for Payment.
/// </summary>
public class PaymentDto
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public decimal Amount { get; set; }
}
