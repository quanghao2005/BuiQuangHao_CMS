import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import axiosClient from '../api/axiosClient';

const BannerCarousel = () => {
    const [banners, setBanners] = useState([]);
    const [currentIndex, setCurrentIndex] = useState(0);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchBanners = async () => {
            try {
                // Sử dụng axiosClient thay vì fetch để tận dụng cấu hình base url
                const response = await axiosClient.get('/Banners');
                setBanners(response || []);
            } catch (error) {
                console.error("Lỗi lấy banner:", error);
            } finally {
                setLoading(false);
            }
        };

        fetchBanners();
    }, []);

    // Tự động chuyển banner sau mỗi 4 giây
    useEffect(() => {
        if (banners.length <= 1) return;
        
        const intervalId = setInterval(() => {
            setCurrentIndex((prevIndex) => (prevIndex + 1) % banners.length);
        }, 4000);

        return () => clearInterval(intervalId);
    }, [banners.length]);

    if (loading) return <div className="text-center my-4"><div className="spinner-border text-primary"></div></div>;
    if (banners.length === 0) return null;

    const nextSlide = () => setCurrentIndex((prevIndex) => (prevIndex + 1) % banners.length);
    const prevSlide = () => setCurrentIndex((prevIndex) => (prevIndex === 0 ? banners.length - 1 : prevIndex - 1));

    return (
        <div className="fade-in-up delay-100 mb-5">
            <div 
                className="position-relative overflow-hidden" 
                style={{ borderRadius: '24px', boxShadow: 'var(--shadow-md)', height: '450px', background: '#000' }}
            >
                {/* Vòng lặp các Banners để tạo hiệu ứng mờ dần (Cross-fade) */}
                {banners.map((banner, index) => (
                    <div 
                        key={banner.id}
                        className="position-absolute top-0 start-0 w-100 h-100"
                        style={{
                            opacity: index === currentIndex ? 1 : 0,
                            transition: 'opacity 0.8s ease-in-out',
                            zIndex: index === currentIndex ? 1 : 0
                        }}
                    >
                        {banner.link ? (
                            <a href={banner.link} target={banner.link.startsWith('http') ? '_blank' : '_self'} rel="noreferrer" className="d-block w-100 h-100">
                                <img src={banner.imageUrl} alt={banner.title} className="w-100 h-100" style={{ objectFit: 'cover' }} />
                            </a>
                        ) : (
                            <img src={banner.imageUrl} alt={banner.title} className="w-100 h-100" style={{ objectFit: 'cover' }} />
                        )}
                    </div>
                ))}
                
                {/* Nút Chuyển Trái/Phải (Chỉ hiện khi có > 1 ảnh) */}
                {banners.length > 1 && (
                    <>
                        <button 
                            onClick={prevSlide}
                            className="btn btn-light position-absolute top-50 start-0 translate-middle-y ms-3 rounded-circle shadow"
                            style={{ width: '45px', height: '45px', opacity: '0.8', zIndex: 10 }}
                        >
                            <i className="fa-solid fa-chevron-left"></i>
                        </button>
                        <button 
                            onClick={nextSlide}
                            className="btn btn-light position-absolute top-50 end-0 translate-middle-y me-3 rounded-circle shadow"
                            style={{ width: '45px', height: '45px', opacity: '0.8', zIndex: 10 }}
                        >
                            <i className="fa-solid fa-chevron-right"></i>
                        </button>
                    </>
                )}

                {/* Thanh chấm (dots) điều hướng */}
                {banners.length > 1 && (
                    <div className="position-absolute bottom-0 start-50 translate-middle-x mb-3 d-flex gap-2" style={{ zIndex: 10 }}>
                        {banners.map((_, index) => (
                            <button 
                                key={index} 
                                onClick={() => setCurrentIndex(index)}
                                className={`border-0 rounded-pill ${index === currentIndex ? 'bg-primary' : 'bg-light'}`}
                                style={{ 
                                    width: index === currentIndex ? '30px' : '10px', 
                                    height: '10px', 
                                    opacity: index === currentIndex ? 1 : 0.6, 
                                    transition: 'all 0.3s ease' 
                                }}
                                aria-label={`Go to slide ${index + 1}`}
                            />
                        ))}
                    </div>
                )}
            </div>
        </div>
    );
};

export default BannerCarousel;
