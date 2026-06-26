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
    public class IndexModel : PageModel
    {
        private readonly pharmacy.Data.ApplicationDbContext _context;

        public IndexModel(pharmacy.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<CartItem> CartItem { get; set; } = default!;

        public async Task OnGetAsync()
        {
            CartItem = await _context
                .CartItems.Include(c => c.Cart)
                .Include(c => c.Medicine)
                .ToListAsync();
        }
    }
}
