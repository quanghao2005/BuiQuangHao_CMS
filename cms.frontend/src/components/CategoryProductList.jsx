import React, { useState, useEffect } from 'react';
import categoryProductService from '../services/categoryProductService';

const CategoryProductList = () => {
    const [categoryProducts, setCategoryProducts] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchCategoryProducts = async () => {
            try {
                setLoading(true);
                const data = await categoryProductService.getAllCategoryProducts();
                // Đảm bảo dữ liệu là mảng trước khi set vào state
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
        return <div className="text-center my-4"><div className="spinner-border text-primary" role="status"></div></div>;
    }

    return (
        <div className="card shadow-sm border-0">
            <div className="card-header bg-white py-3">
                <h5 className="card-title text-uppercase font-weight-bold mb-0">
                    <i className="fa-solid fa-list text-primary me-2"></i> Danh mục SP
                </h5>
            </div>
            <div className="list-group list-group-flush">
                {categoryProducts.length > 0 ? (
                    categoryProducts.map((item) => (
                        <button
                            key={item.id}
                            type="button"
                            className="list-group-item list-group-item-action d-flex justify-content-between align-items-center py-3"
                        >
                            {item.name}
                            <i className="fa-solid fa-angle-right text-muted"></i>
                        </button>
                    ))
                ) : (
                    <div className="p-3 text-center text-muted">Không có danh mục nào.</div>
                )}
            </div>
        </div>
    );
};

export default CategoryProductList;