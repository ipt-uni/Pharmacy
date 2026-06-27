using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using pharmacy.Data;
using pharmacy.Data.Models;

namespace pharmacy.Pages;

public class IndexModel : PageModel
{
    ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
        : base()
    {
        _context = context;
    }

    public ICollection<Medicine> Medicines = new List<Medicine>();

    public void OnGet()
    {
        Medicines = _context
            .Medicines.Include(m => m.Company)
            .Include(m => m.Suppliers)
            .ToList();
    }
}
