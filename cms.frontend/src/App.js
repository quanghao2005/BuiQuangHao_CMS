import React, { useState, useEffect } from 'react';
import { Routes, Route, useLocation, Link } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css';
import './App.css';
import bannerImg from './assets/images/banner.jpg';

// Import các components
import CategoryProductList from './components/CategoryProductList';
import PriceFilter from './components/PriceFilter';
import ProductList from './components/ProductList';
import PostList from './components/PostList';
import HotProductList from './components/HotProductList';

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
import Checkout from './components/Checkout';
import { CartProvider, useCart } from './context/CartContext';

// Tách phần Header ra component riêng để dùng useCart (vì useCart phải nằm trong CartProvider)
const Header = () => {
    const { cartCount } = useCart();
    const [searchKeyword, setSearchKeyword] = useState('');
    
    const handleSearch = (e) => {
        e.preventDefault();
        if (searchKeyword.trim()) {
            window.location.href = `/products?search=${encodeURIComponent(searchKeyword)}`;
        }
    };

    return (
        <header className="glass-header d-flex align-items-center justify-content-between p-4 flex-wrap gap-3">
            <Link to="/" className="brand-font fs-3 fw-bold text-gradient text-decoration-none">
                <i className="fa-solid fa-glasses me-2"></i>HaoCMS.DigitalGuard
            </Link>
            
            <form onSubmit={handleSearch} className="d-flex flex-grow-1 mx-lg-5" style={{ maxWidth: '500px' }}>
                <input 
                    type="text" 
                    className="form-control me-2" 
                    placeholder="Tìm kiếm sản phẩm..." 
                    value={searchKeyword}
                    onChange={(e) => setSearchKeyword(e.target.value)}
                />
                <button type="submit" className="btn btn-primary"><i className="fa-solid fa-magnifying-glass"></i></button>
            </form>

            <Link to="/cart" className="btn btn-light border fw-bold position-relative">
                🛒 GIỎ HÀNG
                {cartCount > 0 && (
                    <span className="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger">
                        {cartCount}
                    </span>
                )}
            </Link>
        </header>
    );
};

function App() {
    const location = useLocation();
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
    const isUtilityPage = ['/about', '/contact', '/cart', '/login', '/register', '/forgot-password', '/profile', '/checkout'].includes(location.pathname);
    const isBlogPage = location.pathname.startsWith('/blog') || location.pathname.startsWith('/post');
    const isProductPage = !isUtilityPage && !isBlogPage;

    const customer = JSON.parse(localStorage.getItem('customer'));

    const handleLogout = () => {
        localStorage.removeItem('customer');
        window.location.href = '/';
    };

    return (
        <div className="main-container container my-4 p-0 fade-in-up">
            {/* 1. HEADER & TOP BAR */}
            <div className="bg-dark text-white py-1 px-3 d-flex justify-content-end align-items-center" style={{ fontSize: '12px' }}>
                <Link to="/" className="text-white text-decoration-none me-4"><i className="fa-solid fa-house me-1"></i> TRANG CHỦ</Link>
                {customer ? (
                    <>
                        <Link to="/profile" className="text-white text-decoration-none me-3"><i className="fa-solid fa-user me-1"></i> Xin chào, <b>{customer.fullName || customer.FullName}</b></Link>
                        <span style={{ cursor: 'pointer', color: '#ff6b6b' }} onClick={handleLogout}><i className="fa-solid fa-right-from-bracket me-1"></i> Đăng xuất</span>
                    </>
                ) : (
                    <Link to="/login" className="text-white text-decoration-none"><i className="fa-solid fa-user me-1"></i> Đăng nhập</Link>
                )}
            </div>
            
            <Header />

            {/* 2. THANH NAVBAR ĐIỀU HƯỚNG MỚI */}
            <nav className="navbar navbar-expand-lg navbar-light bg-white shadow-sm py-3 px-4" style={{ borderRadius: '0 0 16px 16px' }}>
                <div className="container-fluid">
                    <ul className="navbar-nav d-flex flex-row w-100 justify-content-center">
                        <li className="nav-item mx-3">
                            <Link to="/" className="nav-link fw-bold text-dark text-uppercase">TRANG CHỦ</Link>
                        </li>
                        <li className="nav-item mx-3">
                            <Link to="/products" className="nav-link fw-bold text-dark text-uppercase">SẢN PHẨM</Link>
                        </li>
                        <li className="nav-item mx-3">
                            <Link to="/blog" className="nav-link fw-bold text-dark text-uppercase">TIN TỨC</Link>
                        </li>
                        <li className="nav-item mx-3">
                            <Link to="/contact" className="nav-link fw-bold text-dark text-uppercase">LIÊN HỆ</Link>
                        </li>
                    </ul>
                </div>
            </nav>

            {/* 3. CONTENT */}
            <div className="p-4 mt-3">
                {!isUtilityPage && (
                    <div className="fade-in-up delay-100">
                        <div className="mb-5 position-relative overflow-hidden" style={{ borderRadius: '24px', boxShadow: 'var(--shadow-md)' }}>
                            <img src={bannerImg} alt="Banner" className="img-fluid w-100" style={{ maxHeight: '400px', objectFit: 'cover' }} />
                            <div className="position-absolute top-0 start-0 w-100 h-100" style={{ background: 'linear-gradient(to right, rgba(0,0,0,0.6), transparent)' }}></div>
                            <div className="position-absolute top-50 start-0 translate-middle-y text-white p-5">
                                <h1 className="brand-font fw-bold mb-3 display-4">Kính mắt <span className="text-gradient" style={{background: 'linear-gradient(135deg, #60a5fa, #c084fc)', WebkitBackgroundClip: 'text'}}>Công Nghệ</span></h1>
                                <p className="fs-5 mb-4" style={{maxWidth: '400px'}}>Bảo vệ thị lực toàn diện trước ánh sáng xanh. Thiết kế thời thượng, chất lượng vượt trội.</p>
                                <Link to="/products" className="btn btn-primary btn-lg px-4 rounded-pill">Khám phá ngay <i className="fa-solid fa-arrow-right ms-2"></i></Link>
                            </div>
                        </div>
                    </div>
                )}

                {isHomePage && (
                    <div className="fade-in-up delay-100">
                        <HotProductList />
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