// Họ Tên: Bùi Quang Hào
// MSSV: 2123110043
using Microsoft.AspNetCore.Mvc;
using CMS.Data;
using CMS.Data.Entities;
using System.Linq;

namespace CMS.Backend.Controllers
{
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
            return View();
        }

        // 3. POST: Thực thi Lưu Sản Phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product model)
        {
            try
            {
                _context.Products.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("", "Lỗi lưu SQL: " + ex.Message);
                return View(model);
            }
        }

        // 4. GET: Form Sửa Sản Phẩm
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return NotFound();
            return View(product);
        }

        // 5. POST: Thực thi Cập Nhật Sản Phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product model)
        {
            try
            {
                _context.Products.Update(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("", "Lỗi cập nhật SQL: " + ex.Message);
                return View(model);
            }
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