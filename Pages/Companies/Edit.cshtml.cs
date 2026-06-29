using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using pharmacy.Data;
using pharmacy.Data.Models;

namespace pharmacy.Pages.Companies
{
    /// <summary>
    /// Edit an existing company.
    /// </summary>
    [Authorize(Roles = "Staff")]
    public class EditModel : PageModel
    {
        private readonly pharmacy.Data.ApplicationDbContext _context;

        public EditModel(pharmacy.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Company Company { get; set; } = default!;

        /// <summary>
        /// Loads the company by id for editing. Returns 404 if not found.
        /// </summary>
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            Company = company;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var companyToUpdate = await _context.Companies.FindAsync(id);

            if (companyToUpdate == null)
            {
                return NotFound();
            }
            if (await TryUpdateModelAsync(companyToUpdate, "Company", c => c.Name))
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            return Page();
        }

        /// <summary>
        /// Returns true if a company with the given id exists.
        /// </summary>
        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.Id == id);
        }
    }
}
