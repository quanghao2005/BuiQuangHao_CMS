import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom'; // Thêm import Link để điều hướng không reload
import blogService from '../services/blogService';

const BlogCategoryList = () => {
    // Kho lưu trữ danh sách chuyên mục bài viết lấy từ SQL Server
    const [blogCategories, setBlogCategories] = useState([]);
    // Trạng thái tối ưu trải nghiệm người dùng trong lúc đợi API phản hồi
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchBlogCategories = async () => {
            try {
                setLoading(true);
                // Gọi sang lớp Service chứa trục Axios tập trung
                const data = await blogService.getBlogCategories();

                // Cực kỳ quan trọng: Kiểm tra dữ liệu phải là mảng trước khi gán vào State
                setBlogCategories(Array.isArray(data) ? data : []);
            } catch (error) {
                console.error("Lỗi hệ thống khi gọi API chuyên mục tin tức:", error);
            } finally {
                setLoading(false); // Đóng trạng thái Loading
            }
        };

        fetchBlogCategories();
    }, []); // Mảng rỗng đảm bảo không xảy ra vòng lặp render vô hạn làm treo trình duyệt

    if (loading) {
        return (
            <div className="text-center my-3 text-muted small">
                <div className="spinner-border text-info spinner-border-sm me-2" role="status"></div>
                Đang nạp các chuyên mục...
            </div>
        );
    }

    return (
        <div className="card shadow-sm p-3 mt-4 bg-white rounded">
            <h5 className="card-title text-uppercase fw-bold text-secondary">
                <i className="fa-solid fa-tags me-2 text-info"></i> Chủ đề bài viết
            </h5>

            <div className="list-group list-group-flush mt-2">
                {blogCategories.length === 0 ? (
                    <p className="text-muted small ps-2">Chưa có chủ đề tin tức nào.</p>
                ) : (
                    blogCategories.map((cate) => (
                        <Link // Sử dụng Link thay cho thẻ <a>
                            key={cate.id}
                            to={`/blog/category/${cate.id}`} // Đổi href thành to
                            className="list-group-item list-group-item-action d-flex justify-content-between align-items-center py-2 px-1 text-dark text-decoration-none small"
                        >
                            <span><i className="fa-solid fa-hashtag me-2 text-muted"></i>{cate.name}</span>
                            <span className="badge bg-light text-dark border">Read</span>
                        </Link>
                    ))
                )}
            </div>
        </div>
    );
};

export default BlogCategoryList;