using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using pharmacy.Data;
using pharmacy.Data.Models;
using pharmacy.Services;

namespace pharmacy.Pages.Medicines
{
    public class EditModel : PageModel
    {
        private readonly pharmacy.Data.ApplicationDbContext _context;
        private readonly MedicineImageService _medicineImageService;

        public EditModel(
            pharmacy.Data.ApplicationDbContext context,
            MedicineImageService medicineImageService
        )
        {
            _context = context;
            _medicineImageService = medicineImageService;
        }

        [BindProperty]
        public string? ImageUrl { get; set; }

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        [BindProperty]
        public bool RemoveImage { get; set; }

        [BindProperty]
        public Medicine Medicine { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicine = await _context.Medicines.FirstOrDefaultAsync(m => m.Id == id);
            if (medicine == null)
            {
                return NotFound();
            }
            Medicine = medicine;
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name");
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (!RemoveImage)
            {
                var result = await _medicineImageService.ProcessImageAsync(ImageFile, ImageUrl);
                if (!result.Success)
                {
                    ModelState.AddModelError(result.ErrorField, result.ErrorMessage);
                    return Page();
                }
                if (!string.IsNullOrEmpty(result.ImageSrc))
                {
                    Medicine.imageSrc = result.ImageSrc;
                }
            }
            else
            {
                Medicine.imageSrc = null;
            }

            _context.Attach(Medicine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicineExists(Medicine.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool MedicineExists(int id)
        {
            return _context.Medicines.Any(e => e.Id == id);
        }
    }
}
