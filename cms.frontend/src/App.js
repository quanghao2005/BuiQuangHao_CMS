import React, { useState, useEffect } from 'react';
import { Routes, Route, useLocation, Link } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css';
import './App.css';

// Import các components
import CategoryProductList from './components/CategoryProductList';
import PriceFilter from './components/PriceFilter';
import ProductList from './components/ProductList';
import PostList from './components/PostList';
import HotProductList from './components/HotProductList';
import FeaturedProductList from './components/FeaturedProductList';
import NewProductList from './components/NewProductList';
import BannerCarousel from './components/BannerCarousel';
import SearchBar from './components/SearchBar';

import BlogCategoryList from './components/BlogCategoryList';
import PostDetail from './components/PostDetail';
import ProductDetail from './components/ProductDetail';
import categoryProductService from './services/categoryProductService';
import About from './components/About';
import Contact from './components/Contact';
import Cart from './components/Cart';
import Login from './components/Login';
import Register from './components/Register';
import ForgotPassword from './components/ForgotPassword';
import Profile from './components/Profile';
import OrderHistory from './components/OrderHistory';
import Checkout from './components/Checkout';
import { CartProvider, useCart } from './context/CartContext';

// Xóa bỏ component Header cũ vì đã gộp chung vào App

function App() {
    const location = useLocation();
    const { cartCount } = useCart();
    const [categories, setCategories] = useState([]);

    useEffect(() => {
        const fetchCategories = async () => {
            try {
                const data = await categoryProductService.getAllCategoryProducts();
                setCategories(Array.isArray(data) ? data : []);
            } catch (error) {
                console.error("Lỗi lấy danh mục:", error);
            }
        };
        fetchCategories();
    }, []);

    const isHomePage = location.pathname === '/';
    const isUtilityPage = ['/about', '/contact', '/cart', '/login', '/register', '/forgot-password', '/profile', '/order-history', '/checkout'].includes(location.pathname);
    const isBlogPage = location.pathname.startsWith('/blog') || location.pathname.startsWith('/post');
    const isProductPage = !isUtilityPage && !isBlogPage;

    const customer = JSON.parse(localStorage.getItem('customer'));

    const handleLogout = () => {
        localStorage.removeItem('customer');
        window.location.href = '/';
    };

    return (
        <div className="main-container container my-4 p-0 fade-in-up">
            {/* 1. TOP BAR */}
            <div className="bg-dark text-white py-2 px-4 d-flex justify-content-between align-items-center" style={{ fontSize: '13px' }}>
                <div className="d-none d-md-block">
                    <i className="fa-solid fa-envelope me-2 text-primary"></i> support@haocms.com 
                    <span className="mx-3 text-secondary">|</span> 
                    <i className="fa-solid fa-phone me-2 text-primary"></i> Hotline: 1800-1234
                </div>
                <div className="ms-auto">
                    {customer ? (
                        <>
                            <Link to="/profile" className="text-white text-decoration-none me-4 hover-opacity">
                                <i className="fa-solid fa-user-circle me-1 text-primary"></i> Xin chào, <b className="text-warning">{customer.fullName || customer.FullName}</b>
                            </Link>
                            <span style={{ cursor: 'pointer', transition: 'color 0.3s' }} className="text-danger fw-bold hover-opacity" onClick={handleLogout}>
                                <i className="fa-solid fa-right-from-bracket me-1"></i> Đăng xuất
                            </span>
                        </>
                    ) : (
                        <Link to="/login" className="text-white text-decoration-none hover-opacity fw-bold">
                            <i className="fa-solid fa-user me-2 text-primary"></i> Đăng nhập / Đăng ký
                        </Link>
                    )}
                </div>
            </div>
            
            {/* 2. MAIN NAVBAR */}
            <nav className="navbar navbar-expand-lg bg-white shadow-sm py-3 px-4 sticky-top" style={{ zIndex: 1000, borderRadius: '0 0 16px 16px' }}>
                <div className="container-fluid align-items-center">
                    {/* Logo */}
                    <Link to="/" className="navbar-brand brand-font fs-3 fw-bold text-gradient me-5">
                        <i className="fa-solid fa-glasses me-2"></i>HaoCMS<span className="text-dark">.Store</span>
                    </Link>
                    
                    {/* Nút hamburger cho Mobile */}
                    <button className="navbar-toggler border-0 shadow-none" type="button" data-bs-toggle="collapse" data-bs-toggle="target" aria-expanded="false" aria-label="Toggle navigation">
                        <span className="navbar-toggler-icon"></span>
                    </button>

                    {/* Menu Links */}
                    <div className="collapse navbar-collapse justify-content-center">
                        <ul className="navbar-nav gap-2 gap-lg-4">
                            <li className="nav-item">
                                <Link to="/" className="nav-link fw-bold text-dark text-uppercase hover-primary">TRANG CHỦ</Link>
                            </li>
                            <li className="nav-item">
                                <Link to="/products" className="nav-link fw-bold text-dark text-uppercase hover-primary">SẢN PHẨM</Link>
                            </li>
                            <li className="nav-item">
                                <Link to="/blog" className="nav-link fw-bold text-dark text-uppercase hover-primary">TIN TỨC</Link>
                            </li>
                            <li className="nav-item">
                                <Link to="/contact" className="nav-link fw-bold text-dark text-uppercase hover-primary">LIÊN HỆ</Link>
                            </li>
                        </ul>
                    </div>

                    {/* Search & Cart */}
                    <div className="d-flex align-items-center gap-3 mt-3 mt-lg-0 ms-lg-auto">
                        <SearchBar />

                        <Link to="/cart" className="btn btn-primary rounded-pill fw-bold position-relative px-4 shadow-sm border-0 d-flex align-items-center gap-2">
                            <i className="fa-solid fa-cart-shopping"></i> Giỏ Hàng
                            {cartCount > 0 && (
                                <span className="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger border border-white border-2" style={{ fontSize: '11px' }}>
                                    {cartCount}
                                </span>
                            )}
                        </Link>
                    </div>
                </div>
            </nav>

            {/* 3. CONTENT */}
            <div className="p-4 mt-3">
                {!isUtilityPage && (
                    <BannerCarousel />
                )}

                {isHomePage && (
                    <div className="fade-in-up delay-100">
                        <HotProductList />
                        <FeaturedProductList />
                        <NewProductList />
                    </div>
                )}

                <div className="row">
                    {/* SIDEBAR BÊN TRÁI: Hiển thị trên cả trang sản phẩm và trang tin tức */}
                    {!isUtilityPage && (
                        <div className="col-lg-3 col-md-4">
                            {(isHomePage || isProductPage) && (
                                <>
                                    <div className="mb-4">
                                        <h6 className="fw-bold border-bottom pb-2">LỌC DANH MỤC</h6>
                                        <CategoryProductList />
                                    </div>
                                    <div className="mb-4">
                                        <h6 className="fw-bold border-bottom pb-2">LỌC GIÁ TIỀN</h6>
                                        <PriceFilter />
                                    </div>
                                </>
                            )}

                            {(isHomePage || isBlogPage) && (
                                <div className="mb-4">
                                    <h6 className="fw-bold border-bottom pb-2">CHỦ ĐỀ BÀI VIẾT</h6>
                                    <BlogCategoryList />
                                </div>
                            )}
                        </div>
                    )}

                    {/* MAIN CONTENT BÊN PHẢI: Danh sách sản phẩm / Tin tức */}
                    <div className={isUtilityPage ? "col-12" : "col-lg-9 col-md-8"}>

                        <Routes>
                            <Route path="/" element={<ProductList />} />
                            <Route path="/products" element={<ProductList />} />
                            <Route path="/category/:categoryId" element={<ProductList />} />
                            <Route path="/product/:productId" element={<ProductDetail />} />
                            <Route path="/blog" element={<PostList />} />
                            <Route path="/post/:postId" element={<PostDetail />} />
                            <Route path="/about" element={<About />} />
                            <Route path="/blog/category/:categoryId" element={<PostList />} />
                            <Route path="/contact" element={<Contact />} />
                            <Route path="/cart" element={<Cart />} />
                            <Route path="/checkout" element={<Checkout />} />
                            <Route path="/login" element={<Login />} />
                            <Route path="/register" element={<Register />} />
                            <Route path="/forgot-password" element={<ForgotPassword />} />
                            <Route path="/profile" element={<Profile />} />
                            <Route path="/order-history" element={<OrderHistory />} />
                        </Routes>
                    </div>
                </div>

                {/* PHẦN TIN TỨC MỚI NHẤT (NẰM NGANG PHÍA DƯỚI) */}
                {isHomePage && (
                    <div className="row mt-5 border-top pt-4 fade-in-up delay-200">
                        <div className="col-12">
                            <h4 className="fw-bold text-dark mb-4">📰 TIN TỨC MỚI NHẤT</h4>
                            <PostList />
                        </div>
                    </div>
                )}
            </div>

            <footer className="text-center py-4 border-top text-muted" style={{ fontSize: '13px' }}>
                © 2026 HaoCMS.DigitalGuard - Nền tảng kính mắt bảo vệ thị lực kỹ thuật số
            </footer>
        </div>
    );
}

const AppWrapper = () => (
    <CartProvider>
        <App />
    </CartProvider>
);

export default AppWrapper;