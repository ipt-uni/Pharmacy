namespace pharmacy.Services;

/// <summary>
/// Result returned by ProcessImageAsync indicating success/failure
/// with the final image path or an error message.
/// </summary>
public class ImageUploadResult
{
    public bool Success { get; set; }
    public string? ImageSrc { get; set; }
    public string? ErrorMessage { get; set; }
    public string? ErrorField { get; set; } // Model error field name
}

/// <summary>
/// Handles processing of medicine images from file uploads or URLs.
/// </summary>
public class MedicineImageService
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public MedicineImageService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    /// <summary>
    /// Determines the image source for a medicine from either a file upload or a URL.
    /// Returns an ImageUploadResult indicating success/failure with the final image path or an error message.
    /// </summary>
    public async Task<ImageUploadResult> ProcessImageAsync(IFormFile? imageFile, string? imageUrl)
    {
        Console.WriteLine($"Debug: ProcessImageAsync: {imageUrl}");
        // 1. Ensure only one source is provided — reject if both file and URL are present
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

        // 2. Handle file upload: validate content type, generate unique filename, save to disk
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

        // 3. Handle URL: accept as-is
        if (!string.IsNullOrEmpty(imageUrl))
        {
            Console.WriteLine("Debug: ImageURL");
            return new ImageUploadResult { Success = true, ImageSrc = imageUrl };
        }

        // 4. Neither provided — not an error, just leave imageSrc as null
        return new ImageUploadResult { Success = true, ImageSrc = null };
    }
}
