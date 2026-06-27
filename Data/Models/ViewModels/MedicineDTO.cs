namespace pharmacy.Data.Models.ViewModels;

/// <summary>
/// API DTO for Medicine, flattening the company name and supplier list.
/// </summary>
public class MedicineDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal RetailPrice { get; set; }
    public string? ImageSrc { get; set; }
    public string CompanyName { get; set; }
    public List<string> Suppliers { get; set; }
}
