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
    /// Lists all pharmaceutical companies.
    /// </summary>
    public class IndexModel : PageModel
    {
        private readonly pharmacy.Data.ApplicationDbContext _context;

        public IndexModel(pharmacy.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Company> Company { get;set; } = default!;

        /// <summary>
        /// Loads all companies ordered by id.
        /// </summary>
        public async Task OnGetAsync()
        {
            Company = await _context.Companies.ToListAsync();
        }
    }
}
