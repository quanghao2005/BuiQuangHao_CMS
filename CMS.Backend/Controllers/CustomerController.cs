// Họ Tên: Bùi Quang Hào
// MSSV: 2123110043
using Microsoft.AspNetCore.Mvc;
using CMS.Data;
using CMS.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CMS.Backend.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // 1. HIỂN THỊ DANH SÁCH KHÁCH HÀNG
        // ==========================================
        public IActionResult Index()
        {
            var data = _context.Customers.ToList();
            return View(data);
        }

        // ==========================================
        // 2. THÊM MỚI KHÁCH HÀNG (GET)
        // ==========================================
        [HttpGet]
        public IActionResult Create() => View();

        // ==========================================
        // 3. XỬ LÝ LƯU KHÁCH HÀNG MỚI (POST)
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Customer model)
        {
            // 🛠️ Loại bỏ Id tự tăng ra khỏi bộ kiểm tra dữ liệu đầu vào tự động
            ModelState.Remove("Id");

            if (ModelState.IsValid)
            {
                try
                {
                    model.Id = 0; // Đảm bảo Id bằng 0 để SQL Server tự động tăng IDENTITY

                    _context.Customers.Add(model);
                    _context.SaveChanges();

                    // Lưu thành công, chuyển hướng ngay về trang danh sách
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    // Trích xuất lỗi trực tiếp từ hệ quản trị CSDL SQL Server (Ví dụ: Trùng lặp Email)
                    var sqlError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    ModelState.AddModelError("", "Lỗi Database: " + sqlError);
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi hệ thống: " + ex.Message);
                }
            }
            else
            {
                // Nếu ModelState bị False ngầm, gom chi tiết lỗi hiển thị lên vùng thông báo chữ đỏ
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                foreach (var error in errors)
                {
                    ModelState.AddModelError("", "Lỗi nhập liệu: " + error);
                }
            }

            // Trả lại giao diện form cùng dữ liệu cũ và thông báo lỗi cụ thể
            return View(model);
        }

        // ==========================================
        // 4. SỬA THÔNG TIN KHÁCH HÀNG (GET)
        // ==========================================
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null) return NotFound();
            return View(customer);
        }

        // ==========================================
        // 5. CẬP NHẬT DỮ LIỆU SỬA (POST)
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Customer model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Customers.Update(model);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    var sqlError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    ModelState.AddModelError("", "Lỗi SQL khi sửa: " + sqlError);
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi sửa: " + ex.Message);
                }
            }
            else
            {
                // Gom lỗi kiểm tra biểu mẫu khi sửa dữ liệu
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                foreach (var error in errors)
                {
                    ModelState.AddModelError("", "Lỗi sửa nhập liệu: " + error);
                }
            }
            return View(model);
        }

        // ==========================================
        // 6. XÓA KHÁCH HÀNG (POST)
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            try
            {
                var customer = _context.Customers.Find(id);
                if (customer != null)
                {
                    _context.Customers.Remove(customer);
                    _context.SaveChanges();
                }
            }
            catch (DbUpdateException ex)
            {
                TempData["ErrorMessage"] = "Không thể xóa khách hàng này do có ràng buộc dữ liệu liên quan!";
            }
            return RedirectToAction("Index");
        }
    }
}