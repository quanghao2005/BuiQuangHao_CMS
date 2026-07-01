// Họ Tên: Bùi Quang Hào
// MSSV: 2123110043
using Microsoft.AspNetCore.Mvc;
using CMS.Data;
using CMS.Data.Entities;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace CMS.Backend.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context) { _context = context; }

        public IActionResult Index() => View(_context.Users.ToList());

        [HttpGet] public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User model)
        {
            ModelState.Remove("Id"); // Bỏ kiểm tra Id tự tăng

            if (string.IsNullOrEmpty(model.Role)) model.Role = "Editor";

            if (ModelState.IsValid)
            {
                try
                {
                    // Mã hóa mật khẩu trước khi lưu
                    if (!string.IsNullOrEmpty(model.PasswordHash) && !model.PasswordHash.StartsWith("$2"))
                    {
                        model.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.PasswordHash);
                    }

                    _context.Users.Add(model);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi lưu dữ liệu: " + ex.Message);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = _context.Users.Find(id);
            return user == null ? NotFound() : View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(User model)
        {
            // Cần lấy lại đối tượng cũ để tránh mất mát dữ liệu không mong muốn
            var userInDb = _context.Users.Find(model.Id);
            if (userInDb == null) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    userInDb.FullName = model.FullName;
                    
                    // Tiến hành mã hóa nếu mật khẩu gửi lên chưa được mã hóa BCrypt
                    if (!string.IsNullOrEmpty(model.PasswordHash))
                    {
                        if (!model.PasswordHash.StartsWith("$2"))
                        {
                            userInDb.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.PasswordHash);
                        }
                        else
                        {
                            userInDb.PasswordHash = model.PasswordHash;
                        }
                    }

                    userInDb.Role = model.Role;

                    _context.Users.Update(userInDb);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi cập nhật: " + ex.Message);
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}