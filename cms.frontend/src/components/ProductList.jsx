import React, { useState, useEffect } from 'react';
import { useParams, Link, useSearchParams } from 'react-router-dom';
import productService from '../services/productService';

const ProductList = () => {
    const [products, setProducts] = useState([]);
    const [loading, setLoading] = useState(true);
    const [page, setPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const { categoryId } = useParams();
    const [searchParams] = useSearchParams();

    useEffect(() => {
        const fetchProducts = async () => {
            try {
                setLoading(true);
                let response;
                const searchKeyword = searchParams.get('search') || '';
                if (categoryId) {
                    response = await productService.getProductsByCategory(categoryId, page, 9);
                } else {
                    response = await productService.getAllProducts(page, 9, searchKeyword);
                }

                const data = response.data ? response.data : response;
                if (data.products) {
                    setProducts(data.products);
                    setTotalPages(data.totalPages || 1);
                } else {
                    setProducts(Array.isArray(data) ? data : []);
                }
            } catch (error) {
                console.error("Lỗi lấy sản phẩm:", error);
                setProducts([]);
            } finally {
                setLoading(false);
            }
        };

        fetchProducts();
    }, [categoryId, page, searchParams]);

    // Lọc sản phẩm theo giá
    const minPrice = searchParams.get('min');
    const maxPrice = searchParams.get('max');

    const filteredProducts = products.filter(product => {
        const price = product.price || 0;
        if (minPrice && price < parseInt(minPrice)) return false;
        if (maxPrice && price > parseInt(maxPrice)) return false;
        return true;
    });

    if (loading) return (
        <div className="text-center py-5">
            <div className="spinner-border text-primary me-2"></div>
            Đang tải sản phẩm...
        </div>
    );

    return (
        <div className="product-list-container">
            {/* Header hiển thị kết quả */}
            <div className="d-flex justify-content-between align-items-center mb-4 pb-2 border-bottom">
                <h5 className="fw-bold text-dark m-0 text-uppercase">
                    {searchParams.get('search') ? `KẾT QUẢ TÌM KIẾM CHO: "${searchParams.get('search')}"` : (categoryId ? 'SẢN PHẨM THEO DANH MỤC' : 'TẤT CẢ SẢN PHẨM')}
                </h5>
                <span className="text-muted small badge bg-light text-dark border">
                    {filteredProducts.length} sản phẩm
                </span>
            </div>

            {filteredProducts.length === 0 ? (
                <div className="text-center py-5 text-muted bg-light rounded border">
                    <i className="fa-solid fa-box-open fs-1 mb-3 text-warning d-block"></i>
                    <p className="fs-5">Không tìm thấy sản phẩm nào phù hợp với tiêu chí của bạn</p>
                </div>
            ) : (
                <div className="row g-4"> {/* G-4 giúp tạo khoảng cách đều giữa các cột */}
                    {filteredProducts.map((product) => (
                        <div key={product.id} className="col-lg-4 col-md-6">
                            <div
                                className="card h-100 border-0 shadow-sm custom-card-hover"
                                style={{
                                    transition: 'transform 0.3s ease, box-shadow 0.3s ease',
                                    borderRadius: '12px'
                                }}
                                onMouseEnter={(e) => {
                                    e.currentTarget.style.transform = 'translateY(-8px)';
                                    e.currentTarget.style.boxShadow = '0 12px 24px rgba(0,0,0,0.12)';
                                }}
                                onMouseLeave={(e) => {
                                    e.currentTarget.style.transform = 'translateY(0)';
                                    e.currentTarget.style.boxShadow = '0 0.125rem 0.25rem rgba(0,0,0,0.075)';
                                }}
                            >
                                <div className="p-3 text-center bg-light" style={{ borderRadius: '12px 12px 0 0' }}>
                                    <img
                                        src={product.imageUrl ? `https://localhost:7271${product.imageUrl}` : '/images/default-product.jpg'}
                                        className="img-fluid"
                                        alt={product.name}
                                        style={{ height: '180px', objectFit: 'contain' }}
                                        onError={(e) => { e.target.src = '/images/default-product.jpg'; }}
                                    />
                                </div>
                                <div className="card-body d-flex flex-column p-3">
                                    <h6 className="fw-bold text-dark mb-2" style={{ height: '40px', overflow: 'hidden' }}>
                                        {product.name}
                                    </h6>
                                    <p className="text-danger fw-bold mb-3 fs-5">
                                        {product.price ? product.price.toLocaleString('vi-VN') : 0}
                                        <span className="fs-6 text-decoration-underline ms-1">đ</span>
                                    </p>

                                    <Link
                                        to={`/product/${product.id}`}
                                        className="btn btn-primary btn-sm w-100 mt-auto fw-bold py-2"
                                        style={{ borderRadius: '8px' }}
                                    >
                                        <i className="fa-solid fa-eye me-1"></i> Xem chi tiết
                                    </Link>
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            )}

            {/* Pagination */}
            {totalPages > 0 && filteredProducts.length > 0 && (
                <div className="d-flex justify-content-center mt-5">
                    <nav>
                        <ul className="pagination">
                            <li className={`page-item ${page === 1 ? 'disabled' : ''}`}>
                                <button className="page-link" onClick={() => setPage(page - 1)}>Trước</button>
                            </li>
                            {[...Array(totalPages)].map((_, index) => (
                                <li key={index} className={`page-item ${page === index + 1 ? 'active' : ''}`}>
                                    <button className="page-link" onClick={() => setPage(index + 1)}>{index + 1}</button>
                                </li>
                            ))}
                            <li className={`page-item ${page === totalPages ? 'disabled' : ''}`}>
                                <button className="page-link" onClick={() => setPage(page + 1)}>Sau</button>
                            </li>
                        </ul>
                    </nav>
                </div>
            )}
        </div>
    );
};

export default ProductList;