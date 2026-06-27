using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using pharmacy.Data;
using pharmacy.Data.Models;

namespace pharmacy.Pages.Companies
{
    /// <summary>
    /// Displays details of a single company.
    /// </summary>
    public class DetailsModel : PageModel
    {
        private readonly pharmacy.Data.ApplicationDbContext _context;

        public DetailsModel(pharmacy.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Company Company { get; set; } = default!;

        /// <summary>
        /// Loads the company by id. Returns 404 if not found.
        /// </summary>
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies.FirstOrDefaultAsync(m => m.Id == id);

            if (company is not null)
            {
                Company = company;

                return Page();
            }

            return NotFound();
        }
    }
}
