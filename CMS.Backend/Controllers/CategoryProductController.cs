using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore; // BẮT BUỘC THÊM DÒNG NÀY ĐỂ DÙNG INCLUDE
using CMS.Data;
using CMS.Data.Entities;
using System.Linq;

namespace CMS.Backend.Controllers
{
    [Authorize]
    public class CategoryProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. TRANG DANH SÁCH LIÊN KẾT (GET)
        public IActionResult Index()
        {
            // Nếu thực thể của bạn cấu hình Virtual Navigation, ta load dữ liệu danh sách ra
            var data = _context.CategoryProducts.ToList();
            return View(data);
        }

        // 2. FORM TẠO LIÊN KẾT MỚI (GET)
        [HttpGet]
        public IActionResult Create()
        {
            // Nạp danh sách của cả 2 bảng gốc vào ViewBag để làm Dropdown chọn
            ViewBag.Categories = _context.Categories.ToList(); // Hoặc bảng danh mục SP của bạn
            ViewBag.Products = _context.Products.ToList();
            return View();
        }

        // 3. THỰC THI LƯU LIÊN KẾT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryProduct model)
        {
            ModelState.Remove("Id");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.CategoryProducts.Add(model);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi kết nối dữ liệu: " + ex.Message);
                }
            }

            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Products = _context.Products.ToList();
            return View(model);
        }

        // 4. FORM SỬA LIÊN KẾT (GET)
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var item = _context.CategoryProducts.Find(id);
            if (item == null) return NotFound();

            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Products = _context.Products.ToList();
            return View(item);
        }

        // 5. THỰC THI CẬP NHẬT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryProduct model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.CategoryProducts.Update(model);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi cập nhật: " + ex.Message);
                }
            }

            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Products = _context.Products.ToList();
            return View(model);
        }

        // 6. THỰC THI XÓA LIÊN KẾT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var item = _context.CategoryProducts.Find(id);
            if (item != null)
            {
                _context.CategoryProducts.Remove(item);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}