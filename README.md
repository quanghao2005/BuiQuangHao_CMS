**👤 THÔNG TIN SINH VIÊN THỰC HIỆN**  
**Họ và tên:** Bùi Quang Hào  
**Mã số sinh viên (MSSV):** 2123110043  
**Lớp:** CCQ2311B  
**Năm thực hiện:** 2026  

📂 BÁO CÁO TIẾN ĐỘ THỰC HÀNH - BUỔI 8 (SYNC & DATA BINDING)
🛠 CÁC CÔNG VIỆC ĐÃ HOÀN THÀNH TRONG BUỔI 8
1. Đồng bộ hóa kết nối API & Cấu hình môi trường

Axios Configuration: Chuẩn hóa axiosClient.js với baseURL linh hoạt, xử lý triệt để lỗi ERR_CONNECTION_REFUSED bằng việc đồng bộ hóa cổng kết nối giữa Backend (đang chạy cổng 7271) và Frontend.

CORS Policy: Tối ưu hóa cấu hình Middleware AddCors trong Program.cs, đảm bảo cấp quyền truy cập an toàn cho ReactJS client (http://localhost:3000).

2. Hoàn thiện Logic hiển thị dữ liệu (Data Binding)

Component Mapping: Đồng bộ hóa tên Component và lệnh import trong App.js, đảm bảo hệ thống không bị lỗi "Module not found" khi biên dịch.

Interface Design: Hoàn thiện bố cục (Layout) với React Bootstrap, phân tách rõ ràng giữa cột danh mục (Category) và cột nội dung chi tiết (Post/Product).

3. Kiểm soát dữ liệu & Kiểm thử

API Validation: Sử dụng trình duyệt (F12 Console) để kiểm tra các luồng Request/Response. Khắc phục lỗi 404 Not Found bằng cách đối chiếu chính xác Endpoint thực tế trên Swagger với URL trong blogService.js.

Deployment Readiness: Hệ thống hiện đã hiển thị dữ liệu động (Dynamic Data) đổ từ Database SQL Server ra giao diện thành công, sẵn sàng cho các chức năng mở rộng tiếp theo.
