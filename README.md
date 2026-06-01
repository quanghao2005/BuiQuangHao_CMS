# 📂 BÁO CÁO TIẾN ĐỘ THỰC HÀNH - BUỔI 6 (WEB API & CORS CONFIGURATION)

* **Sinh viên:** Bùi Quang Hào
* **MSSV:** 2123110043
* **Dự án:** Hệ thống quản lý website cửa hàng điện thoại (Phone Store CMS)
* **Kiến trúc:** Cấu trúc lai đồng hành (Hybrid Architecture - ASP.NET Core MVC Admin & Web API Backend)

---

## 🛠️ CÁC CÔNG VIỆC ĐÃ HOÀN THÀNH TRONG BUỔI 6

### 1. Xây dựng cổng dữ liệu Web API cho Bài viết (`PostsController`)
* Triển khai hoàn tất 3 luồng đọc dữ liệu (GET Methods) bất đồng bộ (`async/await`) chuẩn RESTful:
  * **`GET /api/posts`**: Lấy toàn bộ danh sách bài viết tin tức (Áp dụng `.Select()` gọt tỉa băng thông và nạp chồng `.Include()` lấy tên chuyên mục).
  * **`GET /api/posts/category/{categoryId}`**: Lọc danh sách bài viết linh hoạt theo mã định danh chuyên mục.
  * **`GET /api/posts/{id}`**: Truy xuất toàn vẹn 100% nội dung chi tiết bài viết (bao gồm cả mã HTML của CKEditor) dựa trên khóa chính ID.

### 2. Triển khai API nâng cao hệ thống Cửa hàng & Giỏ hàng (`Products` & `Orders`)
* **`ProductsController.cs`**: Xây dựng API lấy danh sách sản phẩm, lọc theo danh mục (`/api/products/categoryproduct/{id}`) và xem chi tiết sản phẩm.
* **`OrdersController.cs`**: Xây dựng API tiếp nhận đơn đặt hàng phức hợp từ Frontend gửi lên (`POST /api/orders`).
  * Tích hợp cơ chế giao dịch **`Database Transaction`** bảo vệ an toàn dữ liệu.
  * Xử lý lưu đồng bộ đồng thời thông tin hóa đơn tổng quát (`Orders`) và chi tiết giỏ hàng (`OrderDetails`) trong cùng một phiên bấm nút.
  * Tự động tính toán logic trừ bớt số lượng tồn kho (`StockQuantity`) của sản phẩm sau khi đặt mua thành công.

### 3. Tối ưu cấu hình hệ thống & Đồng bộ Cơ sở dữ liệu
* **Cấu hình chính sách bảo mật CORS**: Cài đặt thành công Middleware `app.UseCors("AllowAll")` nằm chuẩn xác giữa bộ phân luồng `UseRouting` và xác thực `UseAuthentication` trong `Program.cs`, mở đường cho ứng dụng Frontend ReactJS kết nối rút dữ liệu hợp pháp.
* **Đồng bộ hóa thuộc tính Entity**: Tái cấu hình trường `ImageUrl` dạng cho phép nhận giá trị rỗng (`string?`) để tránh xung đột bộ nhớ ngầm.
* **Cập nhật Database thành công**: Khởi chạy thành công công cụ Migration (`Add-Migration AddImageUrlToProduct`) giúp ép thêm cột hình ảnh xuống bảng `dbo.Products` trong SQL Server mà không làm mất dữ liệu cũ.

### 4. Kiểm thử chất lượng qua Postman & Kiểm soát mã nguồn với Git
* Sử dụng **Postman** giả lập thành công gói tin dữ liệu JSON (đầy đủ thông tin khách hàng và mảng sản phẩm chọn mua `cartItems`) để kích hoạt đầu vào API `POST`, kiểm thử thành công trạng thái chuẩn **`Status: 201 Created`**.
* Khắc phục triệt để lỗi chặn chứng chỉ SSL ảo cục bộ và lỗi sai phương thức định danh truyền tải (`405 Method Not Allowed`) trên phần mềm kiểm thử.
* Đóng gói toàn bộ mã nguồn sạch 100% (không lỗi biên dịch, không dính nhãn hệ thống ngầm) và thực hiện **Push thành công lên nhánh biệt lập `Buoi6` trên GitHub**.
