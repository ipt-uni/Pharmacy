using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace pharmacy.Data.Models;

/// <summary>
/// A pharmaceutical company that manufactures medicines.
/// </summary>
public class Company
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The list of medicines that belong to this company
    /// </summary>
    public ICollection<Medicine> Medicines { get; set; } = new List<Medicine>();
}
