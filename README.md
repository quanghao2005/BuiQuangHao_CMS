📂 BÁO CÁO TIẾN ĐỘ THỰC HÀNH - BUỔI 7 (WEB API INTEGRATION)
Sinh viên: Bùi Quang Hào

MSSV: 2123110043

Dự án: Hệ thống quản lý website cửa hàng điện thoại (Phone Store CMS)

Kiến trúc: Phát triển đồng bộ theo mô hình phân tầng (Layered Architecture - ReactJS Frontend & ASP.NET Core Web API Backend)

🛠 CÁC CÔNG VIỆC ĐÃ HOÀN THÀNH TRONG BUỔI 7
1. Xây dựng cổng dữ liệu Web API cho Chuyên mục tin tức (CategoriesController)

Triển khai Controller mới theo chuẩn API Controller (ControllerBase, [ApiController]) để hỗ trợ truy vấn dữ liệu dạng JSON.

Cấu hình Route chuẩn hóa: /api/Categories giúp Frontend dễ dàng kết nối và truy xuất danh mục bài viết.

Xử lý bất đồng bộ (async/await) trong việc truy vấn Database giúp tăng hiệu năng xử lý cho hệ thống CMS.

2. Tích hợp dịch vụ API vào Frontend (ReactJS)

Service Layer: Hoàn thiện blogService.js với các phương thức gọi API (getAllPosts, getPostById, getBlogCategories) sử dụng axiosClient.

Component Development: Xây dựng BlogCategoryList.jsx để render danh sách chuyên mục tin tức từ Database lên giao diện.

3. Khắc phục sự cố kỹ thuật

Git Version Control: Xử lý thành công xung đột (Merge Conflict) trong file Program.cs khi đồng bộ nhánh Buoi7 trên GitHub.

Code Quality: Khắc phục triệt để cảnh báo Unexpected Unicode BOM trong các file .jsx bằng việc chuẩn hóa Encoding (UTF-8 without signature).
