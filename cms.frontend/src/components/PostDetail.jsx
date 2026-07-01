import React, { useState, useEffect } from 'react';
import { useParams, Link } from 'react-router-dom'; // 1. Import thêm Link
import axios from 'axios';

const PostDetail = () => {
    // 2. Đổi 'id' thành 'postId' để khớp chính xác với khai báo trong App.js
    const { postId } = useParams();

    const [post, setPost] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchPost = async () => {
            try {
                setLoading(true);
                // Truyền biến postId vào URL
                const res = await axios.get(`https://localhost:7271/api/posts/${postId}`);
                setPost(res.data);
            } catch (err) {
                console.error("Lỗi khi tải chi tiết bài viết:", err);
                setError("Không thể tải bài viết này.");
            } finally {
                setLoading(false);
            }
        };

        // Chỉ gọi API nếu postId tồn tại
        if (postId) {
            fetchPost();
        }
    }, [postId]); // Đưa postId vào mảng dependency

    if (loading) return <div className="text-center my-5 py-5"><div className="spinner-border text-primary"></div><p className="mt-2">Đang tải nội dung...</p></div>;
    if (error) return <div className="text-center my-5 text-danger"><h3>{error}</h3></div>;
    if (!post) return <div className="text-center my-5 text-muted">Không tìm thấy bài viết.</div>;

    return (
        <div className="container mt-5" style={{ maxWidth: '800px' }}>
            {/* Tiêu đề bài viết */}
            <h1 className="fw-bold text-dark mb-3">{post.title}</h1>

            {/* Thông tin ngày tháng */}
            <p className="text-muted small">
                <i className="fa-regular fa-calendar-days me-2"></i>
                {new Date(post.createdDate).toLocaleDateString('vi-VN')}
            </p>

            <hr />

            {/* Ảnh bài viết */}
            <img
                src={`https://localhost:7271${post.imageUrl}`}
                className="img-fluid rounded mb-4 shadow-sm w-100"
                alt={post.title}
                onError={(e) => { e.target.src = '/images/default-blog.jpg'; }}
            />

            {/* Nội dung bài viết */}
            <div className="content py-3">
                <div
                    className="post-detail-content text-dark"
                    style={{ fontSize: '15px', lineHeight: '1.8' }}
                    dangerouslySetInnerHTML={{ __html: post.content }}
                />
            </div>

            {/* Nút quay lại */}
            <div className="mt-5 mb-5">
                {/* 3. Thay thẻ <a> bằng thẻ <Link> để chống reload trang */}
                <Link to="/" className="btn btn-outline-secondary">
                    ← Quay lại trang chủ
                </Link>
            </div>
        </div>
    );
};

export default PostDetail;