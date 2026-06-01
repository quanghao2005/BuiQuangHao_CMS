/*
 * Sinh viên: Bùi Quang Hào
 * MSSV: 2123110043
 */
using CMS.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// =================================================================
// 1. KHU VỰC ĐĂNG KÝ DỊCH VỤ (SERVICES CONTAINER) - Trước builder.Build()
// =================================================================

// --- BUỔI 2: ĐĂNG KÝ DBCONTEXT VÀO HỆ THỐNG ---
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- BUỔI 4 & 6: GIỮ QUYỀN BIÊN DỊCH VIEW MVC VÀ NHẬN DIỆN API ---
builder.Services.AddControllersWithViews();

// --- BUỔI 6: KHAI BÁO CHÍNH SÁCH CORS (BẬT ĐÈN XANH CHO REACTJS) ---
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        // Cho phép mọi nguồn cấp (Origin), mọi phương thức gọi (GET, POST...), và mọi thông tin đi kèm (Header)
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
}); // Đã gộp và xóa bỏ đoạn khai báo Cors trùng lặp phía dưới để làm sạch code

// --- BUỔI 5: ĐĂNG KÝ DỊCH VỤ XÁC THỰC COOKIE ---
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.AccessDeniedPath = "/Account/AccessDenied";
        });

// --- BUỔI 6: ĐĂNG KÝ BỘ SINH TÀI LIỆU SWAGGER ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build(); // Chốt đăng ký dịch vụ, chuyển sang cấu hình đường ống xử lý


// =================================================================
// 2. KHU VỰC CẤU HÌNH MIDDLEWARE (REQUEST PIPELINE) - Sau builder.Build()
// =================================================================

// --- BUỔI 6: KÍCH HOẠT VÀ TÙY BIẾN GIAO DIỆN THỬ NGHIỆM SWAGGER ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ThaiCMS Web API v1");
        c.RoutePrefix = "swagger"; // Đường dẫn mặc định truy cập: https://localhost:xxxx/swagger
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting(); // Bắt đầu nhận diện phân luồng tuyến đường

// --- [VỊ TRÍ ĐẶT CORS CHUẨN]: Phải nằm ngay giữa UseRouting và UseAuthentication ---
app.UseCors("AllowAll");

// --- BUỔI 5: KÍCH HOẠT XÁC THỰC VÀ PHÂN QUYỀN COOKIE ---
app.UseAuthentication(); // Bước A: Xác nhận "Anh là ai?"
app.UseAuthorization();  // Bước B: Xác nhận "Anh được làm gì?"


// =================================================================
// 3. KHU VỰC ĐỊNH TUYẾN PHÂN LUỒNG (ROUTING MAP)
// =================================================================

// Phân luồng A (Buổi 6): Ánh xạ hệ thống Endpoint dành riêng cho Web API [Route("api/[controller]")]
app.MapControllers();

// Phân luồng B (Buổi 1): Giữ lại bản đồ đường đi mặc định cho trang quản trị Web MVC cũ
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();