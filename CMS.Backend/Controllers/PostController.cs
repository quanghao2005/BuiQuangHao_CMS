//Họ Tên :Bùi Quang Hào
//MSSV : 2123110043
//version : 2.1 (Đã sửa lỗi gọi nhầm bảng Products thành Posts)

using Microsoft.AspNetCore.Mvc;
using CMS.Data;
using CMS.Data.Entities;
using System.Linq;

namespace CMS.Backend.Controllers
{
    public class PostController : Controller
    {
        // Khai báo bối cảnh dữ liệu toàn cục cho Controller
        private readonly ApplicationDbContext _context;

        // "Tiêm" ApplicationDbContext vào hàm khởi tạo (Constructor Injection)
        public PostController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Hàm Index: Lấy danh sách bài viết THẬT từ SQL Server hiển thị ra giao diện
        public IActionResult Index()
        {
            // CHUẨN: Lấy dữ liệu THẬT từ đúng bảng Posts
            var posts = _context.Posts.ToList();

            // Gửi danh sách dữ liệu thật sang View
            return View(posts);
        }

        // 2. Hàm Details: Hiển thị chi tiết một bài viết cụ thể dựa vào Id lấy từ SQL Server
        public IActionResult Details(int id)
        {
            // CHUẨN: Tìm đúng bài viết trong bảng Posts bằng Id
            var post = _context.Posts.Find(id);

            // Nếu không tìm thấy bài viết này trong cơ sở dữ liệu, trả về trang lỗi 404
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }
    }
}