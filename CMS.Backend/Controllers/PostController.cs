// Họ Tên: Bùi Quang Hào
// MSSV: 2123110043
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMS.Data;
using CMS.Data.Entities;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;

namespace CMS.Backend.Controllers
{
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Hiển thị danh sách bài viết
        public IActionResult Index()
        {
            var data = _context.Posts.Include(p => p.Category).ToList();
            return View(data);
        }

        // 2. GET: Form Thêm Bài Viết
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        // 3. POST: Thực thi Lưu Bài Viết + Upload Ảnh Từ Máy Tính
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Post model, IFormFile? ImageFile)
        {
            try
            {
                // Kiểm tra và xử lý nếu người dùng upload ảnh từ máy tính
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var fileName = System.Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                    // Đảm bảo thư mục uploads tồn tại
                    var uploadDir = Path.GetDirectoryName(filePath);
                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        ImageFile.CopyTo(stream);
                    }

                    model.ImageUrl = "/uploads/" + fileName;
                }

                _context.Posts.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (System.Exception ex)
            {
                ViewBag.Categories = _context.Categories.ToList();
                ModelState.AddModelError("", "Lỗi khi thêm bài viết: " + ex.Message);
                return View(model);
            }
        }

        // 4. GET: Form Sửa Bài Viết
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var post = _context.Posts.Find(id);
            if (post == null) return NotFound();

            ViewBag.Categories = _context.Categories.ToList();
            return View(post);
        }

        // 5. POST: Thực thi Cập Nhật Bài Viết + Upload Ảnh Thay Thế
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Post model, IFormFile? ImageFile)
        {
            try
            {
                // Tìm bài viết gốc trong DB để tránh mất dữ liệu ảnh cũ nếu không chọn file mới
                var existingPost = _context.Posts.AsNoTracking().FirstOrDefault(p => p.Id == model.Id);
                if (existingPost == null) return NotFound();

                if (ImageFile != null && ImageFile.Length > 0)
                {
                    // Tạo tên ảnh duy nhất
                    var fileName = System.Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                    // Đảm bảo thư mục uploads tồn tại
                    var uploadDir = Path.GetDirectoryName(filePath);
                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        ImageFile.CopyTo(stream);
                    }

                    // Gán đường dẫn ảnh mới
                    model.ImageUrl = "/uploads/" + fileName;

                    // Tùy chọn xóa file ảnh cũ trong thư mục nếu có để đỡ rác máy (Không bắt buộc)
                    if (!string.IsNullOrEmpty(existingPost.ImageUrl))
                    {
                        var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingPost.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }
                }
                else
                {
                    // Nếu không chọn file ảnh mới, giữ lại đường dẫn ảnh cũ từ DB
                    model.ImageUrl = existingPost.ImageUrl;
                }

                _context.Posts.Update(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (System.Exception ex)
            {
                ViewBag.Categories = _context.Categories.ToList();
                ModelState.AddModelError("", "Lỗi khi cập nhật bài viết: " + ex.Message);
                return View(model);
            }
        }

        // 5.2. CHỨC NĂNG XEM CHI TIẾT (DETAILS)
        public IActionResult Details(int id)
        {
            var post = _context.Posts
                               .Include(p => p.Category)
                               .FirstOrDefault(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // 6. POST: Thực thi Xóa Bài Viết
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var post = _context.Posts.Find(id);
            if (post != null)
            {
                // Xóa file ảnh trong thư mục vật lý trước khi xóa data trong DB
                if (!string.IsNullOrEmpty(post.ImageUrl))
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", post.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                _context.Posts.Remove(post);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}