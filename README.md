👤 THÔNG TIN SINH VIÊN THỰC HIỆN
Họ và tên: Bùi Quang Hào
Mã số sinh viên (MSSV): 2123110043
Lớp: CCQ2311B
Năm thực hiện: 2026

# 🕶️ HaoCMS - DigitalGuard Store
Hệ thống cửa hàng kinh doanh Kính Mắt thời trang và bảo vệ thị lực.

## 🚀 CẤU TRÚC DỰ ÁN 3 TẦNG (3-TIER ARCHITECTURE)
Dự án được xây dựng chuẩn mực theo cấu trúc 3 phân tầng độc lập:
- **CMS.Data:** Tầng thao tác và giao tiếp trực tiếp với cơ sở dữ liệu SQL Server (Entity Framework Core).
- **CMS.Backend:** Tầng xử lý nghiệp vụ, giao tiếp API (ASP.NET Core Web API / MVC).
- **CMS.Frontend:** Tầng hiển thị giao diện người dùng và trải nghiệm khách hàng (ReactJS).

## 🛠️ HƯỚNG DẪN CÀI ĐẶT VÀ CHẠY DỰ ÁN

### 1. Khởi động Backend (API & Admin)
1. Mở file Solution `CMS.Backend.sln` bằng **Visual Studio**.
2. Chọn project khởi chạy là `CMS.Backend`.
3. Bấm nút **Run (F5)** hoặc biểu tượng Tam giác màu xanh lá cây.
4. Hệ thống sẽ tự động build và mở trang web Swagger API hoặc giao diện Admin. Đảm bảo Backend luôn chạy ngầm để Frontend có thể lấy dữ liệu.

### 2. Khởi động Frontend (ReactJS)
1. Mở Terminal / Command Prompt và di chuyển vào thư mục frontend:
   ```bash
   cd cms.frontend
