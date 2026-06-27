using System.ComponentModel.DataAnnotations;

namespace pharmacy.Data.Models;

/// <summary>
/// Base profile for all application users (both customers and staff).
/// Links to the ASP.NET Identity User via UserId.
/// </summary>
public class MyUser
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// The ASP.NET Identity User Id this profile belongs to.
    /// </summary>
    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    public int Age { get; set; }
}
