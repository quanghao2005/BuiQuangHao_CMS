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
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ====================================================================================
        // HÀM 1: API LẤY TOÀN BỘ SẢN PHẨM (CÓ PHÂN TRANG)
        // ====================================================================================
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search = null, [FromQuery] int page = 1, [FromQuery] int limit = 10, [FromQuery] decimal? minPrice = null, [FromQuery] decimal? maxPrice = null)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Name.Contains(search));
            }
            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            query = query.OrderByDescending(p => p.Id);
            var totalItems = await query.CountAsync();
            var totalPages = (int)System.Math.Ceiling(totalItems / (double)limit);

            var products = await query
                .Skip((page - 1) * limit)
                .Take(limit)
                .Select(p => new {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.StockQuantity,
                    ImageUrl = string.IsNullOrEmpty(p.ImageUrl) ? "/images/products/no-image.png" : p.ImageUrl,
                    p.CategoryProductId
                })
                .ToListAsync();

            return Ok(new {
                products,
                totalItems,
                totalPages,
                currentPage = page
            });
        }

        // ====================================================================================
        // HÀM 2: API LỌC SẢN PHẨM THEO DANH MỤC (CÓ PHÂN TRANG)
        // ====================================================================================
        [HttpGet("Category/{categoryId}")]
        public async Task<IActionResult> GetByCategoryProduct(int categoryId, [FromQuery] int page = 1, [FromQuery] int limit = 10, [FromQuery] decimal? minPrice = null, [FromQuery] decimal? maxPrice = null)
        {
            var query = _context.Products.Where(p => p.CategoryProductId == categoryId).AsQueryable();
            
            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            query = query.OrderByDescending(p => p.Id);
            var totalItems = await query.CountAsync();
            var totalPages = (int)System.Math.Ceiling(totalItems / (double)limit);

            var products = await query
                .Skip((page - 1) * limit)
                .Take(limit)
                .Select(p => new {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.StockQuantity,
                    ImageUrl = string.IsNullOrEmpty(p.ImageUrl) ? "/images/products/no-image.png" : p.ImageUrl
                })
                .ToListAsync();

            return Ok(new {
                products,
                totalItems,
                totalPages,
                currentPage = page
            });
        }

        // ====================================================================================
        // HÀM MỚI: API LẤY SẢN PHẨM BÁN CHẠY (HOT PRODUCTS)
        // ====================================================================================
        [HttpGet("Hot")]
        public async Task<IActionResult> GetHotProducts()
        {
            // Tạm thời lấy 3 sản phẩm mới nhất làm sản phẩm hot
            var products = await _context.Products
                .OrderByDescending(p => p.Id)
                .Take(3)
                .Select(p => new {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.StockQuantity,
                    ImageUrl = string.IsNullOrEmpty(p.ImageUrl) ? "/images/products/no-image.png" : p.ImageUrl
                })
                .ToListAsync();
            return Ok(products);
        }

        // ====================================================================================
        // HÀM MỚI: SẢN PHẨM NỔI BẬT (FEATURED PRODUCTS)
        // ====================================================================================
        [HttpGet("Featured")]
        public async Task<IActionResult> GetFeaturedProducts()
        {
            var products = await _context.Products
                .OrderByDescending(p => p.Price) // Lấy các sản phẩm đắt giá nhất làm nổi bật
                .Take(3)
                .Select(p => new {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.StockQuantity,
                    ImageUrl = string.IsNullOrEmpty(p.ImageUrl) ? "/images/products/no-image.png" : p.ImageUrl
                })
                .ToListAsync();
            return Ok(products);
        }

        // ====================================================================================
        // HÀM MỚI: SẢN PHẨM MỚI NHẬP VỀ (NEW ARRIVALS)
        // ====================================================================================
        [HttpGet("New")]
        public async Task<IActionResult> GetNewProducts()
        {
            var products = await _context.Products
                .OrderByDescending(p => p.Id)
                .Skip(3) // Bỏ qua 3 sản phẩm đầu tiên (đã lọt vào Hot)
                .Take(3)
                .Select(p => new {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.StockQuantity,
                    ImageUrl = string.IsNullOrEmpty(p.ImageUrl) ? "/images/products/no-image.png" : p.ImageUrl
                })
                .ToListAsync();
            return Ok(products);
        }

        // ====================================================================================
        // HÀM 3: API XEM CHI TIẾT SẢN PHẨM
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

            // Đảm bảo dữ liệu chi tiết cũng có ảnh mặc định nếu cần
            if (string.IsNullOrEmpty(product.ImageUrl))
            {
                product.ImageUrl = "/images/products/no-image.png";
            }

            return Ok(product);
        }
    }
}