using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using pharmacy.Data;
using pharmacy.Data.Models;

namespace pharmacy.Pages.Companies
{
    /// <summary>
    /// Create a new company.
    /// </summary>
    [Authorize(Roles = "Staff")]
    public class CreateModel : PageModel
    {
        private readonly pharmacy.Data.ApplicationDbContext _context;

        public CreateModel(pharmacy.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays the create form.
        /// </summary>
        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Company Company { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            /// We don't want empty company names and companies that already exist.
            /// But we also are not going to just do return Page because then user will not
            /// know what is the problem.
            if (string.IsNullOrWhiteSpace(Company.Name))
            {
                ModelState.AddModelError("Company.Name", "Company name cannot be empty.");
            }
            if (_context.Companies.Any(c => c.Name == Company.Name))
            {
                ModelState.AddModelError(
                    "Company.Name",
                    "A company with that name already exists."
                );
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Companies.Add(Company);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
