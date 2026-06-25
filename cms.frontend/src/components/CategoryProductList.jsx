import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom'; // Thêm import này
import categoryProductService from '../services/categoryProductService';

const CategoryProductList = () => {
    const [categoryProducts, setCategoryProducts] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchCategoryProducts = async () => {
            try {
                setLoading(true);
                const data = await categoryProductService.getAllCategoryProducts();
                setCategoryProducts(Array.isArray(data) ? data : []);
            } catch (error) {
                console.error("Lỗi khi tải danh mục sản phẩm:", error);
            } finally {
                setLoading(false);
            }
        };

        fetchCategoryProducts();
    }, []);

    if (loading) {
        return (
            <div className="text-center py-3">
                <div className="spinner-border text-primary spinner-border-sm" role="status"></div>
                <span className="ms-2 text-muted small">Đang nạp danh mục...</span>
            </div>
        );
    }

    return (
        <div className="card shadow-sm p-3 bg-white rounded border">
            <h6 className="card-title text-uppercase fw-bold text-dark mb-3 border-bottom pb-2">
                <i className="fa-solid fa-layer-group me-2 text-primary"></i> Lọc Theo Danh Mục Kính
            </h6>

            <div className="d-flex flex-wrap gap-2">
                {categoryProducts.length > 0 ? (
                    categoryProducts.map((item) => (
                        <Link // Đổi thẻ <a> thành <Link>
                            key={item.id}
                            to={`/category/${item.id}`} // Đổi href thành to
                            className="btn btn-outline-secondary btn-sm rounded-pill px-3 py-1"
                            style={{ fontSize: '13px', transition: 'all 0.2s' }}
                        >
                            {item.name}
                        </Link>
                    ))
                ) : (
                    <div className="py-2 text-muted small">Không có danh mục nào.</div>
                )}
            </div>
        </div>
    );
};

export default CategoryProductList;