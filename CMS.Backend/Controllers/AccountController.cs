using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using CMS.Data; // Kết nối tới lớp dữ liệu chứa ApplicationDbContext

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;

    // "Tiêm" kết nối Database vào Controller để sử dụng
    public AccountController(ApplicationDbContext context)
    {
        _context = context;
    }

    // ==========================================
    // --- 1. GIAO DIỆN ĐĂNG NHẬP (GET) ---
    // ==========================================
    [HttpGet]
    public IActionResult Login()
    {
        // Nếu người dùng đã đăng nhập rồi thì tự động chuyển vào trang chủ Admin
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    // ==========================================
    // --- 2. XỬ LÝ LOGIC ĐĂNG NHẬP (POST) ---
    // ==========================================
    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        // Đối soát tài khoản và mật khẩu trực tiếp trong Database
        var user = _context.Users.FirstOrDefault(u => u.Username == username && u.PasswordHash == password);

        if (user != null)
        {
            // Thiết lập danh tính (Claims) chứa thông tin của thành viên
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username), // Lưu tên đăng nhập
                new Claim(ClaimTypes.Role, user.Role),     // Lưu vai trò: Administrator/Editor để phân quyền
                new Claim("FullName", user.FullName)      // Lưu Họ và tên đầy đủ để hiển thị ngoài giao diện
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Ghi file Cookie bảo mật vào trình duyệt để duy trì phiên làm việc
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            // Đăng nhập thành công -> tự động chuyển hướng về trang chủ Admin
            return RedirectToAction("Index", "Home");
        }

        // Nếu thông tin tài khoản hoặc mật khẩu bị sai, hiển thị thông báo lỗi
        ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không chính xác!";
        return View();
    }

    // ==========================================
    // --- 3. HÀM XỬ LÝ ĐĂNG XUẤT (LOGOUT) ---
    // ==========================================
    public async Task<IActionResult> Logout()
    {
        // Ra lệnh xóa file Cookie bảo mật khỏi trình duyệt
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        // Chuyển hướng người dùng quay trở lại trang Đăng nhập
        return RedirectToAction("Login");
    }

    // ==========================================
    // --- 4. TRANG TỪ CHỐI TRUY CẬP (ACCESS DENIED) ---
    // ==========================================
    [HttpGet]
    public IActionResult AccessDenied()
    {
        // Trả về đúng View thông báo lỗi 403 đã thiết kế cho Editor/User
        return View();
    }
}