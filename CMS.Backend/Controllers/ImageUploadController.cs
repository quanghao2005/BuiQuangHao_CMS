using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CMS.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageUploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public ImageUploadController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost("upload-ckeditor")]
        public async Task<IActionResult> UploadImage(IFormFile upload)
        {
            if (upload == null || upload.Length == 0)
                return BadRequest(new { error = new { message = "Không có file nào được tải lên." } });

            string extension = Path.GetExtension(upload.FileName);
            string newFileName = Guid.NewGuid().ToString() + extension;
            string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "ckeditor");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string filePath = Path.Combine(uploadsFolder, newFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await upload.CopyToAsync(stream);
            }

            string url = $"/uploads/ckeditor/{newFileName}";

            // CKEditor yêu cầu trả về chuẩn JSON này
            return Ok(new
            {
                uploaded = 1,
                fileName = newFileName,
                url = url
            });
        }
    }
}
