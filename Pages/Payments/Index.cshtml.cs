using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using pharmacy.Data;
using pharmacy.Data.Models;

namespace pharmacy.Pages.Payments
{
    /// <summary>
    /// Lists all payments with associated cart and customer info.
    /// </summary>
    public class IndexModel : PageModel
    {
        private readonly pharmacy.Data.ApplicationDbContext _context;

        public IndexModel(pharmacy.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Payment> Payment { get; set; } = default!;

        /// <summary>
        /// Loads all payments including cart and customer details.
        /// </summary>
        public async Task OnGetAsync()
        {
            Payment = await _context
                .Payments.Include(p => p.Cart)
                .Include(p => p.Cart.Customer)
                .ToListAsync();
        }
    }
}
