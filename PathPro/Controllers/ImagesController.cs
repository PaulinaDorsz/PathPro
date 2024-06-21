using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PathPro.Models.DTO;
using PathPro.Repositories;
using PathPro.Models.Domain;


namespace PathPro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {

        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        //Upload 
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto request)
        {
            try
            {
                ValidateFileUpload(request);

                if (ModelState.IsValid)
                {
                    // Convert DTO to Domain model
                    var imageDomainModel = new Image
                    {
                        File = request.File,
                        FileExtension = Path.GetExtension(request.File.FileName),
                        FileSizeInBytes = request.File.Length,
                        FileName = request.FileName,
                        FileDescription = request.FileDescription,
                    };

                    // upload image repository 
                    await imageRepository.Upload(imageDomainModel);

                    return Ok(imageDomainModel);
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
         
                return StatusCode(500, "Internal server error");
            }
        }

        private void ValidateFileUpload(ImageUploadRequestDto request)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
            {
                ModelState.AddModelError("file", "Unsupported file extension");
            }

            if (request.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size more than 10MB, please upload a smaller size file.");
            }
        }
        }
    }
