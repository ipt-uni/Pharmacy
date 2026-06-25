using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pharmacy.Data.Models;

public class Medicine
{
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey(nameof(Company))]
    public int CompanyId { get; set; }
    public Company? Company { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Price of this medicine at time of sale
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal RetailPrice { get; set; }

    /// <summary>
    /// The list of suppliers that sell this medicine
    /// </summary>
    public ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();
}
