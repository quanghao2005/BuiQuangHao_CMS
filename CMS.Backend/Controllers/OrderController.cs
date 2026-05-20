//Họ Tên :Bùi Quang Hào
//MSSV : 2123110043
using Microsoft.AspNetCore.Mvc;
//Họ Tên :Bùi Quang Hào
//MSSV : 2123110043
using CMS.Data;
using System.Linq;

namespace CMS.Backend.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        public OrderController(ApplicationDbContext context) { _context = context; }

        public IActionResult Index()
        {
            var orders = _context.Orders.ToList();
            return View(orders);
        }
    }
}