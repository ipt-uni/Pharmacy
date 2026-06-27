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
using pharmacy.Services;

namespace pharmacy.Pages.Medicines
{
    /// <summary>
    /// Create a new medicine (Staff only).
    /// Supports image upload from file or URL and multi-select suppliers.
    /// </summary>
    [Authorize(Roles = "Staff")]
    public class CreateModel : PageModel
    {
        [BindProperty]
        public string? ImageUrl { get; set; } = "";

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        private readonly ApplicationDbContext _context;
        private readonly MedicineImageService _medicineImageService;

        /// <summary>
        /// To access the web host environment for determining uploaded
        /// image file should be saved
        /// </summary>
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CreateModel(
            pharmacy.Data.ApplicationDbContext context,
            MedicineImageService medicineImageService,
            IWebHostEnvironment webHostEnvironment
        )
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _medicineImageService = medicineImageService;
        }

        /// <summary>
        /// Populates dropdowns for company and supplier selection.
        /// </summary>
        public IActionResult OnGet()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name");
            ViewData["Suppliers"] = new SelectList(_context.Suppliers, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Medicine Medicine { get; set; } = default!;

        [BindProperty]
        public List<int> SelectedSupplierIds { get; set; } = new();

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name");
            ViewData["Suppliers"] = new SelectList(_context.Suppliers, "Id", "Name");

            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    foreach (var error in entry.Value.Errors)
                    {
                        Console.WriteLine($"Key: {entry.Key}, Error: {error.ErrorMessage}");
                    }
                }
                Console.WriteLine("Debug: ModelState is not valid");
                return Page();
            }

            // 1. Process the image source (file upload or URL)
            var result = await _medicineImageService.ProcessImageAsync(ImageFile, ImageUrl);
            if (!result.Success)
            {
                ModelState.AddModelError(result.ErrorField, result.ErrorMessage);
                return Page();
            }
            Medicine.imageSrc = result.ImageSrc;

            // 2. Attach selected suppliers to the new medicine
            if (SelectedSupplierIds.Any())
            {
                var suppliers = await _context
                    .Suppliers.Where(s => SelectedSupplierIds.Contains(s.Id))
                    .ToListAsync<Supplier>();
                foreach (var supplier in suppliers)
                    Medicine.Suppliers.Add(supplier);
            }

            try
            {
                _context.Medicines.Add(Medicine);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                // throw;

                // in production, you should log the exception
                // and show a user-friendly message

                ModelState.AddModelError(
                    string.Empty,
                    "An error occurred while creating the medicine. Please try again."
                );
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
