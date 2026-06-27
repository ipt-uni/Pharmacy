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
    /// Displays full details of a single medicine, including company and suppliers.
    /// </summary>
    public class DetailsModel : PageModel
    {
        private readonly pharmacy.Data.ApplicationDbContext _context;

        public DetailsModel(pharmacy.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Medicine Medicine { get; set; } = default!;

        /// <summary>
        /// Loads the medicine by id with its company and suppliers. Returns 404 if not found.
        /// </summary>
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicine = await _context
                .Medicines.Include(m => m.Company)
                .Include(m => m.Suppliers)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medicine is not null)
            {
                Medicine = medicine;

                return Page();
            }

            return NotFound();
        }
    }
}
