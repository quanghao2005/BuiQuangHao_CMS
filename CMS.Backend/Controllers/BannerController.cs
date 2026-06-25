using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CMS.Data;
using CMS.Data.Entities;
using System.Linq;

namespace CMS.Backend.Controllers
{
    [Authorize]
    public class BannerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BannerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Xem danh sách Banner
        public IActionResult Index()
        {
            var banners = _context.Banners.OrderBy(b => b.DisplayOrder).ToList();
            return View(banners);
        }

        // 2. Giao diện Thêm Banner (GET)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // 3. Thực thi Thêm Banner (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Banner model)
        {
            if (ModelState.IsValid)
            {
                _context.Banners.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // 4. Giao diện Cập nhật (GET)
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var banner = _context.Banners.Find(id);
            if (banner == null) return NotFound();
            return View(banner);
        }

        // 5. Thực thi Cập nhật (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Banner model)
        {
            if (ModelState.IsValid)
            {
                var bannerInDb = _context.Banners.Find(model.Id);
                if (bannerInDb == null) return NotFound();

                bannerInDb.Title = model.Title;
                bannerInDb.ImageUrl = model.ImageUrl;
                bannerInDb.Link = model.Link;
                bannerInDb.DisplayOrder = model.DisplayOrder;
                bannerInDb.IsActive = model.IsActive;

                _context.Banners.Update(bannerInDb);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // 6. Thực thi Xóa (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var banner = _context.Banners.Find(id);
            if (banner != null)
            {
                _context.Banners.Remove(banner);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
