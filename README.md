# HaoCMS - Hệ Thống Quản Lý Nội Dung & Bán Hàng (Phần Backend)

**Sinh viên thực hiện:** Bùi Quang Hào  
**MSSV:** 2123110043  
**Học phần:** Thực hành Phát triển Ứng dụng Web 

---

## 🚀 TIẾN ĐỘ HOÀN THÀNH: BUỔI 4 & BUỔI 5

Trong giai đoạn này, dự án đã tập trung hoàn thiện module cốt lõi về **Bảo mật (Authentication)**, **Phân quyền người dùng (Authorization)** và **Đồng bộ hóa giao diện quản trị**.

### 🔐 Buổi 4: Cơ Chế Đăng Nhập Hệ Thống (Authentication)
* **Xây dựng Form Đăng nhập (`AccountController`):** Tiếp nhận thông tin `Username` và `Password` từ người dùng gửi lên.
* **Xác thực dữ liệu (Database Match):** Đối soát thông tin tài khoản trực tiếp với dữ liệu lưu trữ trong bảng `Users` thuộc SQL Server thông qua `ApplicationDbContext`.
* **Cấu hình Phiên làm việc (Cookie-based Authentication):**
  * Sử dụng giải pháp `HttpContext.SignInAsync` để mã hóa thông tin và ghi file Cookie bảo mật (`.AspNetCore.Cookies`) trực tiếp lên trình duyệt của người dùng.
  * Thiết lập cơ chế tự động chuyển hướng người dùng đã xác thực vào trang quản trị trung tâm (`Dashboard`).

### 🛡️ Buổi 5: Cơ Chế Phân Quyền & Quản Lý Vùng Cấm (Authorization)
* **Khóa bảo vệ diện rộng (`[Authorize]`):** Áp dụng bộ lọc an ninh lên toàn bộ các Controller chức năng (`Category`, `Post`, `Product`, `Customer`, `Order`) nhằm ngăn chặn tuyệt đối các truy cập ẩn danh (người lạ gõ bừa URL).
* **Phân quyền theo Vai trò (Role-Based Authorization):**
  * Khóa chặt `UserController.cs` (Trang quản lý tài khoản hệ thống) bằng thẻ `[Authorize(Roles = "Administrator")]` nhằm chỉ cho phép tài khoản tối cao truy cập.
* **Xử lý giao diện động (Dynamic UI Layout):**
  * Tích hợp mã Razor kiểm tra Cookie để hiển thị **Họ và tên**, **Vai trò (Role)** của người đang đăng nhập trên thanh Sidebar.
  * Sử dụng câu lệnh điều kiện `@if (User.IsInRole("Administrator"))` để **ẩn hoàn toàn** menu Quản lý thành viên khi tài khoản mang quyền `Editor` (nhân viên) đăng nhập.
  * Tích hợp thành công nút **Đăng xuất (Logout)** giúp xóa sạch Cookie trên trình duyệt một cách an toàn.
* **Xử lý trang từ chối truy cập (Access Denied - 403):** * Cấu hình hàm `AccessDenied` trong `AccountController` và thiết kế giao diện View thông báo lỗi lịch sự khi tài khoản không đủ quyền hạn cố tình truy cập trái phép qua URL.

---

## 🛠️ CÔNG NGHỆ SỬ DỤNG
* **Backend Framework:** .NET Core 8.0 / ASP.NET Core MVC
* **Database & ORM:** SQL Server Management Studio (SSMS) & Entity Framework Core
* **UI/UX Framework:** Bootstrap 5 & Bootstrap Icons

---

## 🗂️ HƯỚNG DẪN KIỂM TRA (TEST CASES)
1. **Test Login:** Sử dụng tài khoản `admin` (Mật khẩu: `123`) hoặc `nhanvien001` (Mật khẩu: `456`) để kiểm tra luồng đăng nhập thành công.
2. **Test Ẩn Menu:** Đăng nhập bằng `nhanvien001` (Editor) và kiểm tra mục "Tài khoản hệ thống" trên Sidebar đã biến mất.
3. **Test Chặn URL (403):** Khi đang là Editor, gõ ép đường dẫn `https://localhost:7271/User` để kiểm tra trang báo lỗi 403 hoạt động.
