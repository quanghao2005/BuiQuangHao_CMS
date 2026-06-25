import React, { useState, useEffect } from 'react';
import { useParams, Link, useNavigate } from 'react-router-dom';
import productService from '../services/productService';
import { useCart } from '../context/CartContext';

const ProductDetail = () => {
    // Lấy ID sản phẩm từ thanh URL
    const { productId } = useParams();

    const [product, setProduct] = useState(null);
    const [loading, setLoading] = useState(true);
    const [quantity, setQuantity] = useState(1); // State quản lý số lượng mua
    const { addToCart } = useCart();
    const navigate = useNavigate();

    useEffect(() => {
        const fetchProductDetail = async () => {
            try {
                setLoading(true);
                const data = await productService.getProductById(productId);
                setProduct(data);
            } catch (error) {
                console.error("Lỗi khi tải chi tiết sản phẩm:", error);
            } finally {
                setLoading(false);
            }
        };

        if (productId) {
            fetchProductDetail();
        }
    }, [productId]);

    // Hàm xử lý tăng giảm số lượng
    const handleQuantityChange = (type) => {
        if (type === 'decrease' && quantity > 1) setQuantity(quantity - 1);
        if (type === 'increase') setQuantity(quantity + 1);
    };

    if (loading) {
        return (
            <div className="text-center py-5 bg-white shadow-sm border rounded">
                <div className="spinner-border text-primary" role="status"></div>
                <p className="mt-2 text-muted">Đang tải thông tin sản phẩm...</p>
            </div>
        );
    }

    if (!product) {
        return (
            <div className="text-center py-5 bg-white shadow-sm border rounded text-muted">
                <i className="fa-solid fa-box-open fs-1 mb-3 text-warning d-block"></i>
                Sản phẩm không tồn tại hoặc đã ngừng kinh doanh.
                <br />
                <Link to="/products" className="btn btn-outline-primary mt-3">Xem sản phẩm khác</Link>
            </div>
        );
    }

    return (
        <div className="bg-white p-4 shadow-sm border rounded">
            {/* Nút quay lại */}
            <Link to="/products" className="text-decoration-none text-secondary mb-4 d-inline-block hover-primary">
                <i className="fa-solid fa-arrow-left me-2"></i> Tiếp tục mua sắm
            </Link>

            <div className="row">
                {/* CỘT TRÁI: ẢNH SẢN PHẨM */}
                <div className="col-md-5 mb-4">
                    <div className="border rounded p-2 text-center" style={{ backgroundColor: '#f8f9fa' }}>
                        <img
                            src={product.imageUrl ? `https://localhost:7271${product.imageUrl}` : '/images/default-product.jpg'}
                            alt={product.name}
                            className="img-fluid rounded"
                            style={{ maxHeight: '400px', objectFit: 'contain' }}
                            onError={(e) => { e.target.src = '/images/default-product.jpg'; }}
                        />
                    </div>
                </div>

                {/* CỘT PHẢI: THÔNG TIN VÀ ĐẶT HÀNG */}
                <div className="col-md-7">
                    <h2 className="fw-bold text-dark mb-2">{product.name || 'Điện thoại iPhone 15 Pro Max'}</h2>

                    <div className="d-flex align-items-center mb-3">
                        <span className="badge bg-success me-2">Còn hàng</span>
                        <span className="text-muted small">
                            <i className="fa-solid fa-tag me-1"></i> Danh mục: {product.categoryName || 'Sản phẩm mới'}
                        </span>
                    </div>

                    <h3 className="text-danger fw-bold mb-4">
                        {(product.price || 29990000).toLocaleString('vi-VN')} VNĐ
                    </h3>

                    <p className="text-secondary mb-4" style={{ lineHeight: '1.6', fontSize: '15px' }}>
                        {product.description || 'Sản phẩm chính hãng, hiệu năng vượt trội với vi xử lý thế hệ mới, đáp ứng hoàn hảo mọi nhu cầu kết nối và giải trí của bạn. Hỗ trợ kiến trúc hệ thống xử lý tốc độ cao.'}
                    </p>

                    <hr className="mb-4" />

                    {/* Bộ chọn số lượng */}
                    <div className="d-flex align-items-center mb-4">
                        <span className="me-3 fw-bold text-dark">Số lượng:</span>
                        <div className="input-group" style={{ width: '130px' }}>
                            <button className="btn btn-outline-secondary" type="button" onClick={() => handleQuantityChange('decrease')}>-</button>
                            <input type="text" className="form-control text-center bg-white" value={quantity} readOnly />
                            <button className="btn btn-outline-secondary" type="button" onClick={() => handleQuantityChange('increase')}>+</button>
                        </div>
                    </div>

                    {/* Các nút hành động */}
                    <div className="d-flex gap-3">
                        <button 
                            className="btn btn-primary btn-lg flex-grow-1 fw-bold"
                            onClick={() => addToCart(product, quantity)}
                        >
                            <i className="fa-solid fa-cart-plus me-2"></i> THÊM VÀO GIỎ
                        </button>
                        <button 
                            className="btn btn-danger btn-lg flex-grow-1 fw-bold"
                            onClick={() => {
                                addToCart(product, quantity);
                                navigate('/cart');
                            }}
                        >
                            MUA NGAY
                        </button>
                    </div>

                    {/* Cam kết bảo hành */}
                    <div className="mt-4 pt-3 border-top text-muted small">
                        <div className="mb-2"><i className="fa-solid fa-shield-check me-2 text-success"></i> Bảo hành chính hãng 12 tháng</div>
                        <div className="mb-2"><i className="fa-solid fa-rotate-left me-2 text-primary"></i> Đổi trả miễn phí trong 7 ngày</div>
                        <div><i className="fa-solid fa-truck-fast me-2 text-info"></i> Giao hàng siêu tốc 2h</div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default ProductDetail;