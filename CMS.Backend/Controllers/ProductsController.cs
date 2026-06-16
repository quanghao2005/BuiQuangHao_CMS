/*
 * Họ Tên : Bùi Quang Hào
 * MSSV : 2123110043
 */
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMS.Data;
using System.Threading.Tasks;
using System.Linq;

namespace CMS.Backend.Controllers
{
    // 1. Định nghĩa đường dẫn gọi API: https://localhost:xxxx/api/products
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // Hàm khởi tạo tiêm ngữ cảnh dữ liệu
        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ====================================================================================
        // HÀM 1: API LẤY TOÀN BỘ SẢN PHẨM (GET METHOD - CÓ GỌT TỈA DỮ LIỆU)
        // Đường dẫn chạy thử: GET api/products
        // ====================================================================================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _context.Products
                .OrderByDescending(p => p.Id)
                .Select(p => new {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.StockQuantity,
                    p.ImageUrl, // Thuộc tính chuẩn đã đồng bộ
                    p.CategoryProductId
                })
                .ToListAsync();

            return Ok(products);
        }

        // ====================================================================================
        // HÀM 2: API LỌC SẢN PHẨM THEO DANH MỤC SẢN PHẨM (SỬA LỖI GẠCH ĐỎ HÌNH ẢNH)
        // Đường dẫn chạy thử: GET api/products/categoryproduct/1
        // ====================================================================================
        [HttpGet("categoryproduct/{categoryProductId}")]
        public async Task<IActionResult> GetByCategoryProduct(int categoryProductId)
        {
            // Lọc các sản phẩm theo thuộc tính khóa ngoại khớp với cơ sở dữ liệu
            var products = await _context.Products
                .Where(p => p.CategoryProductId == categoryProductId)
                .OrderByDescending(p => p.Id)
                .Select(p => new {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.StockQuantity,
                    p.ImageUrl // Đã sửa lỗi đồng bộ thuộc tính ImageUrl ở đây
                })
                .ToListAsync();

            return Ok(products);
        }

        // ====================================================================================
        // HÀM 3: API XEM CHI TIẾT MỘT SẢN PHẨM (GET DETAIL BY ID)
        // Đường dẫn chạy thử: GET api/products/1
        // ====================================================================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm này trong hệ thống" });
            }

            return Ok(product);
        }
    }
}