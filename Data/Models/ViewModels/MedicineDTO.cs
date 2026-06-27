namespace pharmacy.Data.Models.ViewModels;

public class MedicineDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal RetailPrice { get; set; }
    public string? ImageSrc { get; set; }
    public string CompanyName { get; set; }
    public List<string> Suppliers { get; set; }
}
