import React, { useState, useEffect } from 'react';
import productService from '../services/productService';

const ProductList = () => {
    const [products, setProducts] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchProducts = async () => {
            try {
                setLoading(true);
                const data = await productService.getAllProducts();
                // Đảm bảo set mảng, mặc định là [] nếu data không hợp lệ
                setProducts(Array.isArray(data) ? data : []);
            } catch (error) {
                console.error("Lỗi khi tải danh sách sản phẩm:", error);
            } finally {
                setLoading(false);
            }
        };

        fetchProducts();
    }, []);

    if (loading) {
        return <div className="text-center my-4">Đang tải danh sách sản phẩm...</div>;
    }

    return (
        <div className="row">
            {products.length === 0 ? (
                <div className="col-12"><p className="text-muted">Chưa có sản phẩm nào.</p></div>
            ) : (
                products.map((item) => (
                    <div className="col-md-6 mb-4" key={item.id}>
                        <div className="card h-100 shadow-sm border-0">
                            {/* Hiển thị hình ảnh sản phẩm */}
                            {item.imageUrl && (
                                <img src={item.imageUrl} className="card-img-top" alt={item.name} style={{ height: '200px', objectFit: 'cover' }} />
                            )}
                            <div className="card-body">
                                <h5 className="card-title text-dark">{item.name}</h5>
                                <p className="card-text text-danger font-weight-bold">
                                    {new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(item.price)}
                                </p>
                                <p className="card-text small text-muted">
                                    Tồn kho: {item.stockQuantity || item.stock} sản phẩm
                                </p>
                            </div>
                            <div className="card-footer bg-transparent border-0 pb-3">
                                <button className="btn btn-outline-primary w-100 btn-sm">
                                    <i className="fa-solid fa-eye me-1"></i> Xem chi tiết
                                </button>
                            </div>
                        </div>
                    </div>
                ))
            )}
        </div>
    );
};

export default ProductList;