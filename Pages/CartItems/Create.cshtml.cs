using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using pharmacy.Data;
using pharmacy.Data.Models;

namespace pharmacy.Pages.CartItems
{
    public class CreateModel : PageModel
    {
        private readonly pharmacy.Data.ApplicationDbContext _context;

        public CreateModel(pharmacy.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["CartId"] = new SelectList(_context.Carts, "Id", "Id");
        ViewData["MedicineId"] = new SelectList(_context.Medicines, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public CartItem CartItem { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.CartItems.Add(CartItem);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
