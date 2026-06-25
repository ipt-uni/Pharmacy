using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pharmacy.Data.Models;

public class Supplier
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    public ICollection<Medicine> Medicines { get; set; } = new List<Medicine>();
}
