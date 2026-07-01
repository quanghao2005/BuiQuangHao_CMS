using Microsoft.AspNetCore.Mvc;
using CMS.Data; // Đảm bảo namespace này khớp với project chứa DbContext của bạn
using Microsoft.EntityFrameworkCore;

namespace CMS.Backend.Controllers
{
    [Route("api/[controller]")] // Đường dẫn sẽ là /api/Categories
    [ApiController] // Bắt buộc phải có để báo đây là API Controller
    public class CategoriesController : ControllerBase // Dùng ControllerBase thay vì Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult> GetCategories()
        {
            // Lấy dữ liệu từ bảng Categories trong database
            var categories = await _context.Categories.ToListAsync();
            return Ok(categories); // Trả về JSON
        }
    }
}