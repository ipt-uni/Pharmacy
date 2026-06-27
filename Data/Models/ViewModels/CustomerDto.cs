namespace pharmacy.Data.Models.ViewModels;

public class CustomerDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Address { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
}
