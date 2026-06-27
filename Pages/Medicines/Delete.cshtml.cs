using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using pharmacy.Data;
using pharmacy.Data.Models;

namespace pharmacy.Pages.Medicines
{
    /// <summary>
    /// Delete confirmation and execution page for medicines (Staff only).
    /// </summary>
    [Authorize(Roles = "Staff")]
    public class DeleteModel : PageModel
    {
        private readonly pharmacy.Data.ApplicationDbContext _context;

        public DeleteModel(pharmacy.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Medicine Medicine { get; set; } = default!;

        /// <summary>
        /// Shows the medicine details for deletion confirmation.
        /// </summary>
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicine = await _context.Medicines.FirstOrDefaultAsync(m => m.Id == id);

            if (medicine is not null)
            {
                Medicine = medicine;

                return Page();
            }

            return NotFound();
        }

        /// <summary>
        /// Deletes the medicine. Returns 404 if already removed.
        /// </summary>
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine != null)
            {
                Medicine = medicine;
                _context.Medicines.Remove(Medicine);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
