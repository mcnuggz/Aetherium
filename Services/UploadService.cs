using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Aetherium.Services
{
    public class UploadService
    {
        private readonly IWebHostEnvironment _env;
        private readonly string[] _allowedTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/jpg", "image/webp" };
        private const long MaxFileSizeBytes = 8 * 1024 * 1024; // 8 MB

        public UploadService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> UploadImageAsync(IFormFile file, string subfolder)
        {
            if (file == null || file.Length == 0 || !_allowedTypes.Contains(file.ContentType.ToLower()))
            {
                throw new InvalidOperationException("Invalid file.");
            }

            if (file.Length > MaxFileSizeBytes) {
                throw new InvalidDataException("File is too large. Max file size allowed is 8 MB.");
            }

            var fileName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);
            var savePath = Path.Combine(_env.WebRootPath, "uploads", subfolder);
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            var outputPath = Path.Combine(savePath, fileName);

            using (var stream = file.OpenReadStream()) 
            using (var image = Image.Load(stream))
            {
                image.Mutate(x => x.AutoOrient());
                await image.SaveAsync(outputPath);
            }

            return $"/uploads/{subfolder}/{fileName}";
        }
    }
}
