//Họ Tên :Bùi Quang Hào
//MSSV : 2123110043
//version : 1.0 (Chức năng Xem danh sách thành viên)

using Microsoft.AspNetCore.Mvc;
using CMS.Data; // Để hệ thống tìm thấy ApplicationDbContext
using CMS.Data.Entities; // Để nhận diện lớp dữ liệu User
using System.Linq;

namespace CMS.Backend.Controllers
{
    public class UserController : Controller
    {
        // Khai báo bối cảnh kết nối cơ sở dữ liệu
        private readonly ApplicationDbContext _context;

        // Tiêm ApplicationDbContext thông qua Constructor
        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Action Index: Lấy toàn bộ danh sách thành viên từ SQL Server
        public IActionResult Index()
        {
            // Lấy dữ liệu thật từ bảng Users
            var users = _context.Users.ToList();

            // Truyền danh sách thành viên sang View để hiển thị lên bảng
            return View(users);
        }
    }
}