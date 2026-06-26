using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using pharmacy.Data;
using pharmacy.Data.Models;

namespace pharmacy.Pages.CartItems
{
    public class DetailsModel : PageModel
    {
        private readonly pharmacy.Data.ApplicationDbContext _context;

        public DetailsModel(pharmacy.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public CartItem CartItem { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartitem = await _context.CartItems.FirstOrDefaultAsync(m => m.Id == id);

            if (cartitem is not null)
            {
                CartItem = cartitem;

                return Page();
            }

            return NotFound();
        }
    }
}
