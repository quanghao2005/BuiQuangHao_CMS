import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import productService from '../services/productService';

const NewProductList = () => {
    const [products, setProducts] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchProducts = async () => {
            try {
                const data = await productService.getNewProducts();
                setProducts(data);
            } catch (error) {
                console.error("Lỗi lấy sản phẩm mới:", error);
            } finally {
                setLoading(false);
            }
        };

        fetchProducts();
    }, []);

    if (loading || products.length === 0) return null;

    return (
        <div className="hot-products mb-5">
            <h4 className="fw-bold text-dark mb-3">🆕 SẢN PHẨM MỚI NHẬP VỀ</h4>
            <div className="row g-4">
                {products.map(product => (
                    <div key={product.id} className="col-lg-4 col-md-6">
                        <div className="card h-100 border-0 shadow-sm" style={{ borderRadius: '12px' }}>
                            <div className="p-3 text-center bg-light" style={{ borderRadius: '12px 12px 0 0' }}>
                                <img 
                                    src={product.imageUrl ? `https://localhost:7271${product.imageUrl}` : '/images/default-product.jpg'}
                                    className="img-fluid" 
                                    alt={product.name} 
                                    style={{ height: '180px', objectFit: 'contain' }}
                                />
                            </div>
                            <div className="card-body p-3">
                                <h6 className="fw-bold mb-2 text-truncate">{product.name}</h6>
                                <p className="text-danger fw-bold fs-5 mb-0">
                                    {product.price ? product.price.toLocaleString('vi-VN') : 0} ₫
                                </p>
                                <Link to={`/product/${product.id}`} className="btn btn-outline-danger btn-sm w-100 mt-3">
                                    Mua ngay
                                </Link>
                            </div>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default NewProductList;
