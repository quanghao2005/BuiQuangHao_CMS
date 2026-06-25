/*
 * Họ Tên : Bùi Quang Hào
 * MSSV : 2123110043
 */
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMS.Data;
using CMS.Data.Entities;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;

namespace CMS.Backend.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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
            ViewBag.Categories = _context.CategoryProducts.ToList();
            return View();
        }

        // 3. POST: Thực thi Lưu Sản Phẩm (Đã xử lý Upload ảnh)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product model, IFormFile? imageFile)
        {
            ModelState.Remove("Id");

            if (ModelState.IsValid)
            {
                try
                {
                    // Xử lý lưu ảnh
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                        string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");
                        if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

                        using (var fileStream = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create))
                        {
                            imageFile.CopyTo(fileStream);
                        }
                        model.ImageUrl = "/images/products/" + fileName;
                    }

                    _context.Products.Add(model);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi lưu SQL: " + ex.Message);
                }
            }
            ViewBag.Categories = _context.CategoryProducts.ToList();
            return View(model);
        }

        // 4. GET: Form Sửa Sản Phẩm
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return NotFound();

            ViewBag.Categories = _context.CategoryProducts.ToList();
            return View(product);
        }

        // 5. POST: Thực thi Cập Nhật Sản Phẩm (Đã xử lý Upload ảnh)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product model, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Xử lý ảnh nếu người dùng chọn ảnh mới
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                        string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");
                        if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

                        using (var fileStream = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create))
                        {
                            imageFile.CopyTo(fileStream);
                        }
                        model.ImageUrl = "/images/products/" + fileName;
                    }
                    // Nếu imageFile null, model.ImageUrl vẫn giữ giá trị cũ (được gửi từ thẻ hidden trong View)

                    _context.Products.Update(model);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi cập nhật SQL: " + ex.Message);
                }
            }
            ViewBag.Categories = _context.CategoryProducts.ToList();
            return View(model);
        }

        // 6. POST: Xóa
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