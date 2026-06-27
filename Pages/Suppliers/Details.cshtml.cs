using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using pharmacy.Data;
using pharmacy.Data.Models;

namespace pharmacy.Pages.Suppliers
{
    /// <summary>
    /// Displays details of a single supplier.
    /// </summary>
    public class DetailsModel : PageModel
    {
        private readonly pharmacy.Data.ApplicationDbContext _context;

        public DetailsModel(pharmacy.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Supplier Supplier { get; set; } = default!;

        /// <summary>
        /// Loads the supplier by id. Returns 404 if not found.
        /// </summary>
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers.FirstOrDefaultAsync(m => m.Id == id);

            if (supplier is not null)
            {
                Supplier = supplier;

                return Page();
            }

            return NotFound();
        }
    }
}
