/*
 * Sinh viên: Bùi Quang Hào
 * MSSV: 2123110043
 */
using CMS.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// =================================================================
// 1. KHU VỰC ĐĂNG KÝ DỊCH VỤ (SERVICES CONTAINER)
// =================================================================

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();

// --- CẤU HÌNH CORS CHO REACTJS ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            builder.SetIsOriginAllowed(origin => true) // Cho phép tất cả các cổng (3000, 3001, 3002...)
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials(); // Hỗ trợ gửi cookie/token nếu cần
        });
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.AccessDeniedPath = "/Account/AccessDenied";
        });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// =================================================================
// 2. KHU VỰC CẤU HÌNH MIDDLEWARE (REQUEST PIPELINE)
// =================================================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ThaiCMS Web API v1");
        c.RoutePrefix = "swagger";
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

// --- KÍCH HOẠT CORS ĐÚNG VỊ TRÍ ---
app.UseCors("AllowReactApp");

// --- BUỔI 5: KÍCH HOẠT XÁC THỰC VÀ PHÂN QUYỀN COOKIE ---
app.UseAuthentication(); // Bước A: Xác nhận "Anh là ai?"
app.UseAuthorization();  // Bước B: Xác nhận "Anh được làm gì?"

// =================================================================
// 3. KHU VỰC ĐỊNH TUYẾN PHÂN LUỒNG (ROUTING MAP)
// =================================================================

app.MapControllers();

// Phân luồng B (Buổi 1): Giữ lại bản đồ đường đi mặc định cho trang quản trị Web MVC cũ
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();