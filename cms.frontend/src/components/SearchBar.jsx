import React, { useState, useEffect, useRef } from 'react';
import { useNavigate } from 'react-router-dom';
import productService from '../services/productService';

const SearchBar = () => {
    const [keyword, setKeyword] = useState('');
    const [results, setResults] = useState([]);
    const [isSearching, setIsSearching] = useState(false);
    const [showDropdown, setShowDropdown] = useState(false);
    const navigate = useNavigate();
    const dropdownRef = useRef(null);

    useEffect(() => {
        const handleClickOutside = (event) => {
            if (dropdownRef.current && !dropdownRef.current.contains(event.target)) {
                setShowDropdown(false);
            }
        };
        document.addEventListener('mousedown', handleClickOutside);
        return () => document.removeEventListener('mousedown', handleClickOutside);
    }, []);

    useEffect(() => {
        const fetchResults = async () => {
            if (keyword.trim().length === 0) {
                setResults([]);
                setShowDropdown(false);
                return;
            }
            
            setIsSearching(true);
            try {
                const response = await productService.getAllProducts(1, 5, keyword);
                const data = response.data ? response.data : response;
                const products = data.products ? data.products : (Array.isArray(data) ? data : []);
                
                setResults(products);
                setShowDropdown(true);
            } catch (err) {
                console.error("Search error", err);
            } finally {
                setIsSearching(false);
            }
        };

        const timerId = setTimeout(() => {
            fetchResults();
        }, 400); // 400ms debounce

        return () => clearTimeout(timerId);
    }, [keyword]);

    const handleSubmit = (e) => {
        e.preventDefault();
        setShowDropdown(false);
        if (keyword.trim()) {
            navigate(`/products?search=${encodeURIComponent(keyword)}`);
        }
    };

    const handleSelectProduct = (productId) => {
        setShowDropdown(false);
        navigate(`/product/${productId}`);
    };

    return (
        <div className="position-relative d-none d-md-block" ref={dropdownRef}>
            <form onSubmit={handleSubmit}>
                <input 
                    type="text" 
                    className="form-control rounded-pill ps-4 pe-5 bg-light border-0 shadow-none" 
                    placeholder="Tìm kiếm kính..." 
                    style={{ width: '250px' }} 
                    value={keyword}
                    onChange={(e) => setKeyword(e.target.value)}
                    onFocus={() => { if (results.length > 0) setShowDropdown(true); }}
                    autoComplete="off"
                />
                <button type="submit" className="btn position-absolute top-50 end-0 translate-middle-y text-primary border-0 bg-transparent shadow-none rounded-circle">
                    {isSearching ? <i className="fa-solid fa-spinner fa-spin"></i> : <i className="fa-solid fa-magnifying-glass"></i>}
                </button>
            </form>

            {showDropdown && results.length > 0 && (
                <div className="position-absolute w-100 bg-white border rounded-3 shadow mt-2" style={{ maxHeight: '350px', overflowY: 'auto', zIndex: 1050 }}>
                    {results.map(product => (
                        <div 
                            key={product.id} 
                            className="d-flex align-items-center p-2 border-bottom"
                            style={{ cursor: 'pointer', transition: 'background 0.2s' }}
                            onClick={() => handleSelectProduct(product.id)}
                            onMouseEnter={(e) => e.currentTarget.style.backgroundColor = '#f8f9fa'}
                            onMouseLeave={(e) => e.currentTarget.style.backgroundColor = 'transparent'}
                        >
                            <img 
                                src={product.imageUrl ? `https://localhost:7271${product.imageUrl}` : '/images/default-product.jpg'} 
                                alt={product.name}
                                className="rounded object-fit-cover me-2 border"
                                style={{ width: '45px', height: '45px' }}
                                onError={(e) => { e.target.src = '/images/default-product.jpg'; }}
                            />
                            <div className="text-truncate">
                                <div className="fw-bold text-dark text-truncate" style={{ fontSize: '13px' }}>{product.name}</div>
                                <div className="text-danger fw-bold" style={{ fontSize: '12px' }}>{product.price ? product.price.toLocaleString('vi-VN') : 0} ₫</div>
                            </div>
                        </div>
                    ))}
                    <div 
                        className="text-center p-2 text-primary fw-bold hover-bg-light" 
                        style={{ cursor: 'pointer', fontSize: '13px', backgroundColor: '#f4f6f8' }}
                        onClick={handleSubmit}
                    >
                        Xem tất cả kết quả cho "{keyword}" <i className="fa-solid fa-arrow-right ms-1"></i>
                    </div>
                </div>
            )}
            
            {showDropdown && results.length === 0 && keyword.trim().length > 0 && !isSearching && (
                <div className="position-absolute w-100 bg-white border rounded-3 shadow mt-2 p-3 text-center small text-muted" style={{ zIndex: 1050 }}>
                    Không tìm thấy sản phẩm nào phù hợp.
                </div>
            )}
        </div>
    );
};

export default SearchBar;
