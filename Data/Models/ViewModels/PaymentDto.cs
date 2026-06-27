namespace pharmacy.Data.Models.ViewModels;

public class PaymentDto
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public decimal Amount { get; set; }
}
