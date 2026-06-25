**👤 THÔNG TIN SINH VIÊN THỰC HIỆN**  
**Họ và tên:** Bùi Quang Hào  
**Mã số sinh viên (MSSV):** 2123110043  
**Lớp:** CCQ2311B  
**Năm thực hiện:** 2026  

---

# 🕶️ Đồ án Buổi 9 - Cửa hàng Kính Mắt HaoCMS Store
Hệ thống cửa hàng kinh doanh Kính Mắt thời trang và bảo vệ thị lực kỹ thuật số.

## 🚀 MÔ HÌNH VÀ CẤU TRÚC DỰ ÁN
Dự án được phân chia theo cấu trúc **3 Phân Tầng (3-Tier Architecture)** chuẩn mực:
- **CMS.Data:** Tầng thao tác cơ sở dữ liệu (SQL Server) bằng công nghệ Entity Framework Core.
- **CMS.Backend:** Tầng cung cấp dịch vụ Web API bằng C# (ASP.NET Core Web API) và giao diện Admin quản trị.
- **CMS.Frontend:** Tầng tương tác người dùng bằng thư viện ReactJS kết hợp Bootstrap 5.

---

## 🌟 CÁC CHỨC NĂNG NỔI BẬT ĐÃ HOÀN THIỆN TRONG BUỔI 9

### 1. Phía Backend (C# Web API)
- **API Sản phẩm:** Tích hợp bộ lọc phân trang (Pagination) chuẩn xác và chức năng tìm kiếm thông minh đa luồng.
- **API Đơn hàng:** Tự động **trừ số lượng tồn kho** (Reserve Stock) ngay tại thời điểm khách hàng đặt lệnh Checkout thành công nhằm tránh tình trạng cháy hàng ảo.
- **API Lịch sử & Tin tức:** Cung cấp luồng dữ liệu lịch sử mua hàng riêng biệt cho từng tài khoản và bóc tách nội dung HTML chuẩn xác để hiển thị tóm tắt tin tức.

### 2. Phía Frontend (ReactJS)
- **Tìm kiếm tức thì (Live Search):** Xây dựng Component tìm kiếm mới với khả năng gọi API ngầm (Debounce) và xổ danh sách sản phẩm (kèm hình ảnh, giá tiền) ngay lập tức khi người dùng đang gõ từ khóa.
- **Bảo mật & Auto-fill Giỏ hàng:** Chặn khách vãng lai (buộc đăng nhập) trước khi thanh toán. Hệ thống tự động nhận diện tài khoản và điền sẵn thông tin Họ tên, Số điện thoại, Địa chỉ vào đơn đặt hàng.
- **Nâng cấp UX/UI:**
  - Giao diện Trang Hồ sơ cá nhân (Profile) được nâng cấp thành dạng Card Dashboard hiện đại.
  - Chức năng "Mua Ngay" đưa khách hàng bay thẳng tới trang Thanh toán (bỏ qua bước xem giỏ hàng).
  - Tinh chỉnh Navbar cố định (Sticky Top) và cố định lưới sản phẩm vuông vức (9 sản phẩm/trang).

---

## 🛠️ HƯỚNG DẪN CÀI ĐẶT VÀ CHẠY DỰ ÁN (CHẤM ĐIỂM)

### BƯỚC 1: Khởi chạy Backend (ASP.NET Core)
1. Mở file Solution `CMS.Backend.sln` bằng **Visual Studio**.
2. Đảm bảo cấu hình chuỗi kết nối (Connection String) trong `appsettings.json` đã đúng với SQL Server của bạn.
3. Chạy lệnh `Update-Database` trong Package Manager Console (nếu chưa có DB).
4. Bấm **F5** để chạy dự án. Hệ thống sẽ mở ra cửa sổ Swagger hiển thị danh sách các API. *(Lưu ý: Luôn để Backend chạy ngầm).*

### BƯỚC 2: Khởi chạy Frontend (ReactJS)
1. Mở Terminal / Command Prompt và di chuyển vào thư mục frontend:
   ```bash
   cd cms.frontend
