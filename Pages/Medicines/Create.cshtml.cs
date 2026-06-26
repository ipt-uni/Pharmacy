using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using pharmacy.Data;
using pharmacy.Data.Models;

namespace pharmacy.Pages.Medicines
{
    public class CreateModel : PageModel
    {
        public string ImageUrl { get; set; } = "";
        public IFormFile? ImageFile { get; set; }

        private readonly pharmacy.Data.ApplicationDbContext _context;

        /// <summary>
        /// To access the web host environment for determining uploaded
        /// image file should be saved
        /// </summary>
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CreateModel(
            pharmacy.Data.ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment
        )
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult OnGet()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Medicine Medicine { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            /// 1. is to check whether there is a file upload or image url.
            /// They can't be both
            /// 2. if it is file:
            ///    2.1 if the file is an image
            ///        Then specify it's name
            ///        assign the full image path to Medicine object
            ///        save the file
            ///     otherwise:
            ///       throw an error indicating that the file is not an image
            /// 3. if it is url:
            ///    just set it and don't check much
            /// 4. there can't be both image url and image file.
            if (ImageFile != null && !string.IsNullOrEmpty(ImageUrl))
            {
                ModelState.AddModelError(
                    "ImageFile",
                    "You can't upload an image and an image url at the same time"
                );
                return Page();
            }
            if (ImageFile != null && ImageFile.Length > 0)
            {
                if (
                    !(ImageFile.ContentType == "image/jpeg" || ImageFile.ContentType == "image/png")
                )
                {
                    ModelState.AddModelError("ImageFile", "The file is not an image");
                    return Page();
                }
                string imageName =
                    Guid.NewGuid().ToString()
                    + Path.GetExtension(ImageFile.FileName).ToLowerInvariant();
                // save the image file to the server
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string imageDir = Path.Combine(wwwRootPath, "images");
                string savePath = Path.Combine(imageDir, imageName);
                Medicine.imageSrc = "/images/" + imageName;
                try
                {
                    if (!Directory.Exists(imageDir))
                    {
                        Directory.CreateDirectory(imageDir);
                    }
                    // specify the path where to save the file
                    using (var stream = new FileStream(savePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }
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
            }
            if (!string.IsNullOrEmpty(ImageUrl))
            {
                Medicine.imageSrc = ImageUrl;
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
