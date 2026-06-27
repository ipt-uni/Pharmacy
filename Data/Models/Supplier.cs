using System.ComponentModel.DataAnnotations;

namespace pharmacy.Data.Models;

/// <summary>
/// A supplier that provides medicines to the pharmacy.
/// </summary>
public class Supplier
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Medicines this supplier provides (many-to-many relationship).
    /// </summary>
    public ICollection<Medicine> Medicines { get; set; } = new List<Medicine>();
}
