//Họ Tên :Bùi Quang Hào
//MSSV : 2123110043
using CMS.Backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CMS.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CMS.Backend.Controllers
{
    [Authorize] // Bắt buộc đăng nhập mới xem được Dashboard
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Thống kê số liệu cho Dashboard
            ViewBag.TotalProducts = _context.Products.Count();
            ViewBag.TotalOrders = _context.Orders.Count();
            ViewBag.TotalCustomers = _context.Customers.Count();
            
            // Tính tổng doanh thu
            ViewBag.TotalRevenue = _context.OrderDetails.Sum(od => (decimal?)od.Quantity * od.UnitPrice) ?? 0;

            // Lấy 5 đơn hàng mới nhất
            ViewBag.RecentOrders = _context.Orders
                .Include(o => o.Customer)
                .OrderByDescending(o => o.OrderDate)
                .Take(5)
                .ToList();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
