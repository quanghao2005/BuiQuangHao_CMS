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
        var user = _context.Users.FirstOrDefault(u => u.Username == username);

        if (user != null)
        {
            // So sánh mật khẩu thô hoặc dùng BCrypt (để tương thích ngược với dữ liệu cũ)
            bool isPasswordMatch = false;
            if (user.PasswordHash == password) isPasswordMatch = true;
            else
            {
                try { isPasswordMatch = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash); }
                catch { isPasswordMatch = false; }
            }

            if (isPasswordMatch)
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
        } // <-- Đóng ngoặc cho if (user != null)

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

    // ==========================================
    // --- 5. TÍNH NĂNG QUÊN MẬT KHẨU (FORGOT PASSWORD) ---
    // ==========================================
    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(string username, string email)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username == username);
        if (user != null)
        {
            string newPassword = "NewPassword123!";
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            // Gửi email
            string emailBody = $"<h3>Xin chào {user.FullName},</h3><p>Mật khẩu mới của bạn là: <b>{newPassword}</b></p><p>Vui lòng đăng nhập và đổi mật khẩu sớm nhất có thể.</p>";
            await CMS.Backend.Helpers.EmailHelper.SendEmailAsync(email, "Reset mật khẩu hệ thống HaoCMS", emailBody);

            ViewBag.Message = "Mật khẩu mới đã được khởi tạo và gửi vào email của bạn. (Xem log EmailHelper nếu không nhận được thật)";
        }
        else
        {
            ViewBag.Error = "Tên đăng nhập hoặc Email không khớp với hệ thống!";
        }
        return View();
    }
}