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
    [Authorize(Roles = "Staff")]
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

        [BindProperty]
        public List<int> SelectedSupplierIds { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicine = await _context
                .Medicines.Include(m => m.Suppliers)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medicine == null)
            {
                return NotFound();
            }
            Medicine = medicine;
            SelectedSupplierIds = medicine.Suppliers.Select(s => s.Id).ToList();
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name");
            ViewData["Suppliers"] = new SelectList(_context.Suppliers, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name");
            ViewData["Suppliers"] = new SelectList(_context.Suppliers, "Id", "Name");
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var medicineToUpdate = await _context
                .Medicines.Include(m => m.Suppliers)
                .FirstOrDefaultAsync(m => m.Id == Medicine.Id);
            if (medicineToUpdate == null)
            {
                return NotFound();
            }

            medicineToUpdate.Name = Medicine.Name;
            medicineToUpdate.CompanyId = Medicine.CompanyId;
            medicineToUpdate.RetailPrice = Medicine.RetailPrice;

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
                    medicineToUpdate.imageSrc = result.ImageSrc;
                }
            }
            else
            {
                medicineToUpdate.imageSrc = null;
            }

            var selectedSuppliers = await _context.Suppliers
                .Where(s => SelectedSupplierIds.Contains(s.Id))
                .ToListAsync();
            medicineToUpdate.Suppliers.Clear();
            foreach (var s in selectedSuppliers)
                medicineToUpdate.Suppliers.Add(s);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Medicines.Any(e => e.Id == Medicine.Id))
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
    }
}
