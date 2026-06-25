👤 THÔNG TIN SINH VIÊN THỰC HIỆN
Họ và tên: Bùi Quang Hào
Mã số sinh viên (MSSV): 2123110043
Lớp: CCQ2311B
Năm thực hiện: 2026
# 🎨 HaoCMS Store - Frontend UI/UX
Giao diện người dùng cho cửa hàng kính mắt HaoCMS. Được xây dựng dựa trên ReactJS kết hợp Bootstrap để mang lại trải nghiệm thương mại điện tử hiện đại và thân thiện.

## 🌟 NHỮNG NÂNG CẤP GIAO DIỆN & TÍNH NĂNG (GẦN ĐÂY)
Trong các bản cập nhật mới nhất, hệ thống đã được trang bị hàng loạt tính năng cao cấp của các sàn TMĐT chuyên nghiệp:

### 1. Trải nghiệm tìm kiếm thông minh (Live Search)
- Xây dựng component `SearchBar.jsx` hoàn toàn mới.
- Khách hàng chỉ cần gõ từ khóa, hệ thống sẽ **gợi ý thả xuống (Dropdown)** ngay lập tức với hình ảnh và giá tiền sản phẩm mà không cần tải lại trang.

### 2. Quy trình Mua Hàng & Thanh Toán (Checkout) cực mượt
- Nút **"Mua Ngay"** tự động chuyển hướng khách hàng bay thẳng vào trang Thanh toán để chốt đơn nhanh nhất.
- Bổ sung **Hình ảnh sản phẩm** trực quan ngay tại trang Checkout.
- **Auto-fill (Tự động điền):** Tự động nhận diện và điền sẵn Họ tên, SĐT, Địa chỉ của khách hàng vào Form đặt hàng nếu đã đăng nhập.
- **Bảo mật:** Bắt buộc khách hàng phải đăng nhập mới được thêm vào giỏ hàng hoặc truy cập Checkout. Khách lạ sẽ bị đẩy về trang Login.

### 3. Trang Hồ Sơ (Profile) mang phong cách Card Dashboard
- Thiết kế lại trang thông tin người dùng theo phong cách giao diện Dashboard cực kỳ "nịnh mắt".
- Tách biệt rõ ràng khu vực Thông tin chung (Bên trái) và khu vực Cập nhật dữ liệu, Đổi mật khẩu (Bên phải).

### 4. Lịch Sử Mua Hàng (Order History)
- Khách hàng có thể tự theo dõi lại toàn bộ các đơn hàng mình đã từng mua.
- Hiển thị đầy đủ tổng tiền, trạng thái giao hàng và chi tiết các sản phẩm bên trong đơn.

### 5. Tinh chỉnh Layout & Nội dung
- **Thanh Điều Hướng (Top Navbar):** Được làm gọn gàng, dính chặt trên đỉnh (Sticky Top) và mượt mà hơn.
- **Phân trang sản phẩm:** Chốt cố định lưới (grid) 9 sản phẩm/trang để giao diện luôn vuông vức 3x3. Thanh phân trang luôn hiển thị để điều hướng tốt hơn.
- **Tin tức & Blog:** Trích xuất tự động và làm sạch đoạn mã HTML để biến thành các mô tả ngắn (excerpt) tuyệt đẹp trong danh sách bài viết.

## 🛠️ HƯỚNG DẪN KHỞI CHẠY (NPM)
```bash
# Cài đặt thư viện node_modules
npm install

# Khởi chạy máy chủ React
npm start
```
