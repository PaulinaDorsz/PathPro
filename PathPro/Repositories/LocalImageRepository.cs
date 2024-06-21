using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PathPro.Models.Domain;
using PathPro.Data;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PathPro.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly PathProDbContext dbContext;

        public LocalImageRepository(IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,
            PathProDbContext dbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }

        public async Task<Image> Upload(Image image)
        {
            // Combine paths using Path.Combine for better cross-platform support
            var fileNameWithExtension = $"{image.FileName}{image.FileExtension}";
            var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", fileNameWithExtension);

            // Upload Image to Local Path
            using (var stream = new FileStream(localFilePath, FileMode.Create))
            {
                await image.File.CopyToAsync(stream);
            }

            // Construct URL path
            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Images/{fileNameWithExtension}";

            image.FilePath = urlFilePath;

            // Add Image to the Images table
            await dbContext.Images.AddAsync(image);
            await dbContext.SaveChangesAsync();

            return image;
        }
    }
}
