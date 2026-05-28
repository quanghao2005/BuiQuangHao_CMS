/*
 * Sinh viên: Bùi Quang Hào
 * MSSV: 2123110043
 */
using Microsoft.EntityFrameworkCore;
using CMS.Data;

var builder = WebApplication.CreateBuilder(args);

// --- BƯỚC QUAN TRỌNG: ĐĂNG KÝ DBCONTEXT VÀO HỆ THỐNG ---
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();

// --- BỔ SUNG BUỔI 5: ĐĂNG KÝ DỊCH VỤ XÁC THỰC COOKIE ---
builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// --- BỔ SUNG BUỔI 5: KÍCH HOẠT XÁC THỰC COOKIE ---
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();