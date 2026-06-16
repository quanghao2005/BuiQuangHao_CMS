// Họ Tên: Bùi Quang Hào
// MSSV: 2123110043
using Microsoft.AspNetCore.Mvc;
using CMS.Data;
using CMS.Data.Entities;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace CMS.Backend.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Hiển thị danh sách sản phẩm
        public IActionResult Index()
        {
            var data = _context.Products.ToList();
            return View(data);
        }

        // 2. GET: Form Thêm Sản Phẩm
        [HttpGet]
        public IActionResult Create()
        {
            // Lấy danh sách danh mục truyền sang View để làm ô chọn Dropdown
            ViewBag.Categories = _context.CategoryProducts.ToList();
            return View();
        }

        // 3. POST: Thực thi Lưu Sản Phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product model)
        {
            ModelState.Remove("Id"); // Bỏ qua kiểm tra Id tự tăng của Entity

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Products.Add(model);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (System.Exception ex)
                {
                    // Lấy thông tin lỗi sâu nhất từ SQL Server (InnerException) nếu có
                    var errorMsg = ex.InnerException?.Message ?? ex.Message;
                    ModelState.AddModelError("", "Lỗi lưu SQL: " + errorMsg);
                }
            }

            // Nếu dữ liệu không hợp lệ hoặc lỗi SQL, nạp lại danh mục trước khi trả về View
            ViewBag.Categories = _context.CategoryProducts.ToList();
            return View(model);
        }

        // 4. GET: Form Sửa Sản Phẩm
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return NotFound();

            // Nạp danh sách danh mục để form Sửa cũng chọn lại được danh mục
            ViewBag.Categories = _context.CategoryProducts.ToList();
            return View(product);
        }

        // 5. POST: Thực thi Cập Nhật Sản Phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Products.Update(model);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (System.Exception ex)
                {
                    var errorMsg = ex.InnerException?.Message ?? ex.Message;
                    ModelState.AddModelError("", "Lỗi cập nhật SQL: " + errorMsg);
                }
            }

            // Nạp lại danh sách danh mục nếu xảy ra lỗi validate
            ViewBag.Categories = _context.CategoryProducts.ToList();
            return View(model);
        }

        // 6. POST: Thực thi Xóa Sản Phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}