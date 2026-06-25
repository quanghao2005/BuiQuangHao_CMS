import React, { useState, useEffect } from 'react';
import { Link, useParams } from 'react-router-dom'; // Thêm useParams
import blogService from '../services/blogService';

const PostList = () => {
    const [posts, setPosts] = useState([]);
    const [loading, setLoading] = useState(true);

    // Lấy categoryId từ URL
    const { categoryId } = useParams();

    useEffect(() => {
        const fetchPosts = async () => {
            try {
                setLoading(true);
                let data;

                // Gọi API dựa trên việc có categoryId hay không
                if (categoryId) {
                    data = await blogService.getPostsByCategory(categoryId);
                } else {
                    data = await blogService.getAllPosts();
                }

                setPosts(Array.isArray(data) ? data : []);
            } catch (error) {
                console.error("Quá trình kết nối API bài viết thất bại:", error);
            } finally {
                setLoading(false);
            }
        };
        fetchPosts();
    }, [categoryId]); // Theo dõi sự thay đổi của categoryId

    if (loading) {
        return (
            <div className="text-center py-5">
                <div className="spinner-border text-primary" role="status"></div>
                <p className="mt-2 text-muted">Đang tải kiến thức bảo vệ thị lực...</p>
            </div>
        );
    }

    return (
        <div className="bg-white p-4 shadow-sm border rounded">
            <h4 className="text-danger fw-bold border-bottom pb-2 mb-4 text-uppercase">
                {categoryId ? 'BÀI VIẾT THEO CHỦ ĐỀ' : 'KIẾN THỨC & BẢO VỆ THỊ LỰC'}
            </h4>

            {posts.length === 0 ? (
                <div className="text-center py-5 text-muted">
                    <i className="fa-regular fa-folder-open fs-1 mb-3 d-block"></i>
                    Hiện tại chưa có bài viết nào trong chủ đề này.
                </div>
            ) : (
                posts.map((item) => (
                    <Link
                        key={item.id}
                        to={`/post/${item.id}`}
                        className="d-flex align-items-center py-3 border-bottom text-decoration-none"
                        style={{ transition: 'background-color 0.3s', display: 'flex' }}
                        onMouseEnter={(e) => e.currentTarget.style.backgroundColor = '#f8f9fa'}
                        onMouseLeave={(e) => e.currentTarget.style.backgroundColor = 'transparent'}
                    >
                        <div style={{ width: '120px', height: '80px', flexShrink: 0 }}>
                            <img
                                src={item.imageUrl ? `https://localhost:7271${item.imageUrl}` : '/images/default-blog.jpg'}
                                className="w-100 h-100 rounded object-fit-cover shadow-sm"
                                alt={item.title}
                                onError={(e) => { e.target.src = '/images/default-blog.jpg'; }}
                            />
                        </div>

                        <div className="ms-3 flex-grow-1">
                            <h6 className="fw-bold mb-1 text-dark" style={{ lineHeight: '1.4' }}>{item.title}</h6>
                            <p className="small text-muted mb-2 text-truncate" style={{ maxWidth: '400px' }}>
                                {item.shortDescription || (item.content ? item.content.replace(/<[^>]+>/g, '') : 'Đang cập nhật nội dung...')}
                            </p>
                            <div className="d-flex align-items-center">
                                <span className="badge bg-danger me-2">Kiến thức</span>
                                <small className="text-muted">
                                    <i className="fa-regular fa-calendar-alt me-1"></i>
                                    {item.createdDate ? new Date(item.createdDate).toLocaleDateString('vi-VN') : 'Đang cập nhật'}
                                </small>
                            </div>
                        </div>

                        <div className="ms-3">
                            <i className="fa-solid fa-chevron-right text-muted"></i>
                        </div>
                    </Link>
                ))
            )}
        </div>
    );
};

export default PostList;