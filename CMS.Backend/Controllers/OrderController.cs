using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CMS.Data;
using CMS.Data.Entities;
using System.Linq;

namespace CMS.Backend.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. DANH SÁCH ĐƠN HÀNG
        public IActionResult Index()
        {
            var orders = _context.Orders.Include(o => o.Customer).OrderByDescending(o => o.Id).ToList();
            return View(orders);
        }

        // 2. CHI TIẾT ĐƠN HÀNG
        public IActionResult Details(int id)
        {
            var order = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                .FirstOrDefault(o => o.Id == id);

            if (order == null) return NotFound();
            return View(order);
        }

        // 3. GIAO DIỆN SỬA ĐƠN HÀNG (GET)
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null) return NotFound();

            // Nạp danh sách khách hàng để chọn lại nếu cần
            ViewBag.Customers = _context.Customers.ToList();
            return View(order);
        }

        // 4. THỰC THI CẬP NHẬT ĐƠN HÀNG (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Order model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var orderInDb = _context.Orders.Find(model.Id);
                    if (orderInDb == null) return NotFound();

                    // Cập nhật các trường thông tin theo thực thể thực tế của Hào
                    orderInDb.Status = model.Status;
                    orderInDb.Notes = model.Notes;
                    orderInDb.CustomerId = model.CustomerId;
                    orderInDb.OrderDate = model.OrderDate;

                    _context.Orders.Update(orderInDb);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi cập nhật SQL: " + ex.Message);
                }
            }

            ViewBag.Customers = _context.Customers.ToList();
            return View(model);
        }

        // 5. THỰC THI XÓA ĐƠN HÀNG VÀ CHI TIẾT ĐƠN HÀNG (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var order = _context.Orders.Find(id);
            if (order != null)
            {
                try
                {
                    // Xóa toàn bộ liên kết con trong bảng OrderDetails trước để tránh lỗi ràng buộc khóa ngoại SQL
                    var details = _context.OrderDetails.Where(od => od.OrderId == id).ToList();
                    if (details.Any())
                    {
                        _context.OrderDetails.RemoveRange(details);
                    }

                    // Xóa đơn hàng chính
                    _context.Orders.Remove(order);
                    _context.SaveChanges();
                }
                catch (System.Exception ex)
                {
                    TempData["Error"] = "Không thể xóa đơn hàng do lỗi hệ thống: " + ex.Message;
                }
            }
            return RedirectToAction("Index");
        }

        // HÀM CẬP NHẬT TRẠNG THÁI NHANH Ở TRANG CHI TIẾT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int id, int status)
        {
            var order = _context.Orders.Find(id);
            if (order != null)
            {
                order.Status = status;
                _context.Orders.Update(order);
                _context.SaveChanges();
            }
            return RedirectToAction("Details", new { id = id });
        }
    }
}