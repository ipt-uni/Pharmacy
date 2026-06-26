namespace pharmacy.Services;

public class ImageUploadResult
{
    public bool Success { get; set; }
    public string? ImageSrc { get; set; }
    public string? ErrorMessage { get; set; }
    public string? ErrorField { get; set; } // Model error field name
}

public class MedicineImageService
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public MedicineImageService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<ImageUploadResult> ProcessImageAsync(IFormFile? imageFile, string? imageUrl)
    {
        Console.WriteLine($"Debug: ProcessImageAsync: {imageUrl}");
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
        if (imageFile != null && !string.IsNullOrEmpty(imageUrl))
        {
            Console.WriteLine("Debug: ImageFile with ImageURL");
            return new ImageUploadResult
            {
                Success = false,
                ErrorField = "ImageFile",
                ErrorMessage = "You can't upload an image and an image url at the same time",
            };
        }

        if (imageFile != null && imageFile.Length > 0)
        {
            Console.WriteLine("Debug: ImageFile");
            if (!(imageFile.ContentType == "image/jpeg" || imageFile.ContentType == "image/png"))
            {
                return new ImageUploadResult
                {
                    Success = false,
                    ErrorField = "ImageFile",
                    ErrorMessage = "The file is not an image",
                };
            }

            string imageName =
                Guid.NewGuid().ToString()
                + Path.GetExtension(imageFile.FileName).ToLowerInvariant();

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string imageDir = Path.Combine(wwwRootPath, "images");
            string savePath = Path.Combine(imageDir, imageName);

            try
            {
                if (!Directory.Exists(imageDir))
                {
                    Directory.CreateDirectory(imageDir);
                }

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
            }
            catch (Exception)
            {
                return new ImageUploadResult
                {
                    Success = false,
                    ErrorField = string.Empty,
                    ErrorMessage = "An error occurred while saving the image. Please try again.",
                };
            }

            return new ImageUploadResult { Success = true, ImageSrc = "/images/" + imageName };
        }

        if (!string.IsNullOrEmpty(imageUrl))
        {
            Console.WriteLine("Debug: ImageURL");
            return new ImageUploadResult { Success = true, ImageSrc = imageUrl };
        }

        // No file, no URL — nothing to do, not necessarily an error
        return new ImageUploadResult { Success = true, ImageSrc = null };
    }
}
