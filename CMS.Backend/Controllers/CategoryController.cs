//Họ Tên : Bùi Quang Hào
//MSSV : 2123110043
//version : 2.0 (Bổ sung quy trình 2 bước: Nhập và Lưu dữ liệu vào SQL Server)

using CMS.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using CMS.Data;
using System.Linq;

namespace CMS.Backend.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        // "Tiêm" kết nối vào Controller
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Hàm Index: Hiển thị danh sách danh mục gốc của bạn
        public IActionResult Index()
        {
            // Lấy dữ liệu THẬT từ bảng Categories trong SQL
            var data = _context.Categories.ToList();
            return View(data);
        }

        // =========================================================================
        // // 1. Hàm GET: Dùng để hiển thị giao diện Form cho bạn nhập liệu
        // =========================================================================
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // =========================================================================
        // // 2. Hàm POST: Đã thêm bẫy lỗi try-catch để ép hiển thị lỗi SQL ra màn hình
        // =========================================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category model)
        {
            try
            {
                // Bỏ qua ModelState.IsValid để ép hệ thống thực hiện lệnh lưu thẳng
                _context.Categories.Add(model);
                _context.SaveChanges();

                // Lưu thành công sẽ nhảy về trang danh sách ngay lập tức
                return RedirectToAction("Index");
            }
            catch (System.Exception ex)
            {
                // Nếu SQL bị kẹt (Lỗi Identity, lỗi kết nối, v.v.), dòng này sẽ gửi lỗi ra giao diện
                ModelState.AddModelError("", "Lỗi hệ thống database: " + ex.InnerException?.Message ?? ex.Message);
                return View(model);
            }
        }
        // =========================================================================
        // // 3. CHỨC NĂNG SỬA (EDIT) - BƯỚC 1: GET - Lấy dữ liệu cũ hiển thị lên Form
        // =========================================================================
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Tìm danh mục trong SQL Server theo đúng ID truyền vào
            var category = _context.Categories.Find(id);

            if (category == null)
            {
                return NotFound(); // Trả về trang 404 nếu không tìm thấy danh mục
            }

            return View(category);
        }

        // =========================================================================
        // // 3. CHỨC NĂNG SỬA (EDIT) - BƯỚC 2: POST - Đón dữ liệu mới và cập nhật SQL
        // =========================================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category model)
        {
            try
            {
                // Ra lệnh cho Entity Framework cập nhật dòng dữ liệu này
                _context.Categories.Update(model);
                _context.SaveChanges();

                return RedirectToAction("Index"); // Thành công thì quay về trang danh sách
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("", "Lỗi cập nhật SQL: " + ex.Message);
                return View(model);
            }
        }

        // =========================================================================
        // // 4. CHỨC NĂNG XÓA (DELETE) - POST - Thực hiện xóa thẳng dữ liệu bằng ID
        // =========================================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            try
            {
                // Tìm đúng danh mục cần xóa trong DB
                var category = _context.Categories.Find(id);

                if (category != null)
                {
                    _context.Categories.Remove(category); // Xóa khỏi bộ nhớ tạm
                    _context.SaveChanges(); // Ép SQL Server thực thi xóa vĩnh viễn
                }

                return RedirectToAction("Index");
            }
            catch (System.Exception ex)
            {
                // Nếu danh mục này đang có Bài viết (Post) tham chiếu tới, SQL sẽ chặn không cho xóa để bảo vệ dữ liệu
                TempData["ErrorMessage"] = "Không thể xóa danh mục này vì đang có bài viết thuộc danh mục! Lỗi: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}