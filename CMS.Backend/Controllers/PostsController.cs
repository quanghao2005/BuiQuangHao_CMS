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
            // Lấy toàn bộ dữ liệu từ bảng Posts trong SQL Server
            var posts = await _context.Posts
                .Include(p => p.Category)       // Nạp chồng (Join) bảng Category để lấy tên danh mục
                .OrderByDescending(p => p.Id)   // Sắp xếp bài viết mới nhất lên đầu trang
                .Select(p => new {              // "Gọt tỉa" dữ liệu: Chỉ bóc tách những trường cần thiết
                    p.Id,
                    p.Title,
                    p.ImageUrl,
                    p.CreatedDate,
                    CategoryName = p.Category.Name // Kéo trực tiếp tên chuyên mục thay vì lấy mã ID cộc lốc
                })
                .ToListAsync(); // Chốt chặn bất đồng bộ để tải dữ liệu về mảng sạch

            // Trả về mảng dữ liệu JSON cho Frontend kèm mã trạng thái HTTP 200 OK
            return Ok(posts);
        }

        // ====================================================================================
        // PHẦN 2 - BƯỚC 2: VIẾT HÀM LẤY BÀI VIẾT THEO CHUYÊN MỤC TIN TỨC (ASYNC/AWAIT)
        // ====================================================================================
        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            // Lọc các bài viết có CategoryId trùng với ID truyền vào từ thanh URL
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

            // Trả về kết quả JSON sạch kèm trạng thái HTTP 200 OK
            return Ok(posts);
        }

        // ====================================================================================
        // PHẦN 3: API CHI TIẾT BÀI VIẾT (GET BY ID - ASYNC/AWAIT)
        // ====================================================================================

        // 3. Định nghĩa đường dẫn nhận ID trực tiếp: api/posts/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {
            // 3.1. Quét bảng Posts để tìm bài viết đầu tiên có Id khớp với tham số
            var post = await _context.Posts
                .Include(p => p.Category) // Lấy kèm thông tin danh mục nếu cần hiển thị ngoài UI
                .FirstOrDefaultAsync(p => p.Id == id);

            // 3.2 Xử lý kịch bản lỗi bảo vệ hệ thống: ID không tồn tại trong Database
            if (post == null)
            {
                // Trả về mã lỗi 404 kèm một "gói tin" JSON thông báo nhỏ gọn để Frontend tự xử lý UI
                return NotFound(new { message = "Không tìm thấy bài viết này trong hệ thống" });
            }

            // 3.3. Trả về toàn bộ đối tượng bài viết (bao gồm cả trường Content chứa mã HTML) kèm mã 200 OK
            return Ok(post);
        }
    }
}