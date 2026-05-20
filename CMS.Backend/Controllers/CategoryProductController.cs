//Họ Tên :Bùi Quang Hào
//MSSV : 2123110043
using Microsoft.AspNetCore.Mvc;
using CMS.Data;
using System.Linq;

namespace CMS.Backend.Controllers
{
    public class CategoryProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoryProductController(ApplicationDbContext context) { _context = context; }

        public IActionResult Index()
        {
            var catProducts = _context.CategoriesProducts.ToList();
            return View(catProducts);
        }
    }
}