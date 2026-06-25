using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pharmacy.Data.Models;

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
