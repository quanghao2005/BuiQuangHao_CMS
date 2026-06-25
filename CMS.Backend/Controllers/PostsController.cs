/*
 * Họ Tên : Bùi Quang Hào
 * MSSV : 2123110043
 */
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMS.Data;

namespace CMS.Backend.Controllers
{
    // 1. Định nghĩa đường dẫn gọi API. Địa chỉ truy cập sẽ là: https://localhost:xxxx/api/posts
    [Route("api/[controller]")]

    // 2. Đánh dấu API Controller để hệ thống tự động kiểm tra Validation dữ liệu đầu vào
    [ApiController]

    // 3. Kế thừa từ ControllerBase để tối ưu hóa hiệu năng, cắt bỏ render giao diện HTML
    public class PostsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // 4. Hàm khởi tạo (Constructor): "Tiêm" ngữ cảnh kết nối SQL Server vào để sử dụng
        public PostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ====================================================================================
        // PHẦN 2 - BƯỚC 1: VIẾT HÀM LẤY TOÀN BỘ BÀI VIẾT (ASYNC/AWAIT)
        // ====================================================================================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var posts = await _context.Posts
                .Include(p => p.Category)
                .OrderByDescending(p => p.Id)
                .Select(p => new {
                    p.Id,
                    p.Title,
                    p.ImageUrl,
                    p.CreatedDate,
                    CategoryName = p.Category.Name
                })
                .ToListAsync();

            return Ok(posts);
        }

        // ====================================================================================
        // PHẦN 2 - BƯỚC 2: VIẾT HÀM LẤY BÀI VIẾT THEO CHUYÊN MỤC TIN TỨC (ASYNC/AWAIT)
        // ====================================================================================
        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            var posts = await _context.Posts
                .Where(p => p.CategoryId == categoryId)
                .OrderByDescending(p => p.CreatedDate)
                .Select(p => new {
                    p.Id,
                    p.Title,
                    p.ImageUrl,
                    p.CreatedDate
                })
                .ToListAsync();

            return Ok(posts);
        }

        // ====================================================================================
        // PHẦN 3: API CHI TIẾT BÀI VIẾT (ĐÃ FIX LỖI VÒNG LẶP JSON CIRCULAR REFERENCE)
        // ====================================================================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {
            // Sử dụng Select để chỉ bốc tách những thông tin cần thiết, 
            // KHÔNG trả về nguyên entity gốc để tránh bị vòng lặp JSON
            var post = await _context.Posts
                .Where(p => p.Id == id)
                .Select(p => new {
                    p.Id,
                    p.Title,
                    p.Content, // Quan trọng: Đây là đoạn HTML nội dung bài viết
                    p.ImageUrl,
                    p.CreatedDate,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category != null ? p.Category.Name : "Chưa phân loại"
                })
                .FirstOrDefaultAsync();

            if (post == null)
            {
                return NotFound(new { message = "Không tìm thấy bài viết này trong hệ thống" });
            }

            return Ok(post);
        }
    }
}