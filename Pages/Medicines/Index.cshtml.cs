using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using pharmacy.Data;
using pharmacy.Data.Models;

namespace pharmacy.Pages.Medicines
{
    /// <summary>
    /// Medicine list page (staff-only Create/Edit/Delete, public Details).
    /// </summary>
    public class IndexModel : PageModel
    {
        private readonly pharmacy.Data.ApplicationDbContext _context;

        public IndexModel(pharmacy.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Medicine> Medicine { get; set; } = default!;

        /// <summary>
        /// Loads all medicines ordered by id, including company and supplier data.
        /// </summary>
        public async Task OnGetAsync()
        {
            Medicine = await _context
                .Medicines.Include(m => m.Company)
                .Include(m => m.Suppliers)
                .OrderBy(f => f.Id)
                .ToListAsync();
        }
    }
}
