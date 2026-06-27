using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using pharmacy.Data;
using pharmacy.Data.Models;

namespace pharmacy.Pages;

/// <summary>
/// Home page — displays all medicines as a card grid with Add to Cart and Details actions.
/// </summary>
public class IndexModel : PageModel
{
    ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
        : base()
    {
        _context = context;
    }

    public ICollection<Medicine> Medicines = new List<Medicine>();

    /// <summary>
    /// Loads all medicines with their company and supplier data for display.
    /// </summary>
    public void OnGet()
    {
        Medicines = _context
            .Medicines.Include(m => m.Company)
            .Include(m => m.Suppliers)
            .ToList();
    }
}
