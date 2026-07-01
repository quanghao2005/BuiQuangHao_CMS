import React, { useState, useEffect } from 'react';
import { useSearchParams, useNavigate, useLocation } from 'react-router-dom';

const PriceFilter = () => {
    const [searchParams] = useSearchParams();
    const navigate = useNavigate();
    const location = useLocation();

    // Lấy giá trị hiện tại trên thanh URL
    const currentMin = searchParams.get('minPrice');
    const currentMax = searchParams.get('maxPrice');

    // Hàm cập nhật URL khi người dùng bấm chọn giá
    const handleFilter = (min, max) => {
        const params = new URLSearchParams(searchParams);

        if (min !== null && min > 0) params.set('minPrice', min);
        else params.delete('minPrice');

        if (max !== null && max < 50000000) params.set('maxPrice', max);
        else params.delete('maxPrice');

        // Điều hướng nhưng giữ nguyên trang hiện tại (Trang chủ hoặc trang Danh mục)
        navigate(`${location.pathname}?${params.toString()}`);
    };

    // State cục bộ cho thanh kéo
    const [sliderMin, setSliderMin] = useState(currentMin ? parseInt(currentMin) : 0);
    const [sliderMax, setSliderMax] = useState(currentMax ? parseInt(currentMax) : 50000000);

    // Đồng bộ thanh kéo khi URL bị thay đổi (ví dụ bấm radio)
    useEffect(() => {
        setSliderMin(currentMin ? parseInt(currentMin) : 0);
        setSliderMax(currentMax ? parseInt(currentMax) : 50000000);
    }, [currentMin, currentMax]);

    const handleSliderApply = () => {
        handleFilter(sliderMin, sliderMax);
    };

    // Kiểm tra xem Radio nào đang được Active
    const isActive = (min, max) => {
        return currentMin === (min ? min.toString() : null) &&
            currentMax === (max ? max.toString() : null);
    };

    // Các mốc giá tiền
    const priceRanges = [
        { label: 'Tất cả mức giá', min: null, max: null },
        { label: 'Dưới 5.000.000đ', min: 0, max: 5000000 },
        { label: 'Từ 5 - 10 triệu', min: 5000000, max: 10000000 },
        { label: 'Từ 10 - 15 triệu', min: 10000000, max: 15000000 },
        { label: 'Từ 15 - 30 triệu', min: 15000000, max: 30000000 },
        { label: 'Trên 30 triệu', min: 30000000, max: null }
    ];

    return (
        <div className="card shadow-sm p-3 bg-white rounded border mt-4">
            <h6 className="card-title text-uppercase fw-bold text-dark mb-3 border-bottom pb-2">
                <i className="fa-solid fa-filter me-2 text-primary"></i> Lọc Theo Giá Tiền
            </h6>

            <div className="d-flex flex-column gap-2">
                {priceRanges.map((range, index) => (
                    <div className="form-check" key={index}>
                        <input
                            className="form-check-input"
                            type="radio"
                            name="priceFilter"
                            id={`price-${index}`}
                            checked={isActive(range.min, range.max)}
                            onChange={() => handleFilter(range.min, range.max)}
                            style={{ cursor: 'pointer' }}
                        />
                        <label
                            className="form-check-label text-secondary small"
                            htmlFor={`price-${index}`}
                            style={{ cursor: 'pointer' }}
                        >
                            {range.label}
                        </label>
                    </div>
                ))}
            </div>

            {/* THANH KÉO (RANGE SLIDER) */}
            <div className="mt-4 pt-3 border-top">
                <h6 className="fw-bold text-dark mb-3" style={{ fontSize: '13px' }}>
                    <i className="fa-solid fa-sliders me-2 text-primary"></i> KÉO CHỌN MỨC GIÁ
                </h6>
                
                <label className="form-label small text-muted mb-1 d-flex justify-content-between">
                    <span>Giá từ:</span> 
                    <b className="text-danger">{sliderMin.toLocaleString('vi-VN')}đ</b>
                </label>
                <input 
                    type="range" 
                    className="form-range" 
                    min="0" 
                    max="50000000" 
                    step="500000" 
                    value={sliderMin} 
                    onChange={(e) => {
                        let val = Number(e.target.value);
                        if(val > sliderMax) val = sliderMax;
                        setSliderMin(val);
                    }}
                    onMouseUp={handleSliderApply}
                    onTouchEnd={handleSliderApply}
                />

                <label className="form-label small text-muted mb-1 mt-2 d-flex justify-content-between">
                    <span>Đến:</span> 
                    <b className="text-danger">{sliderMax.toLocaleString('vi-VN')}đ</b>
                </label>
                <input 
                    type="range" 
                    className="form-range" 
                    min="0" 
                    max="50000000" 
                    step="500000" 
                    value={sliderMax} 
                    onChange={(e) => {
                        let val = Number(e.target.value);
                        if(val < sliderMin) val = sliderMin;
                        setSliderMax(val);
                    }}
                    onMouseUp={handleSliderApply}
                    onTouchEnd={handleSliderApply}
                />
            </div>
        </div>
    );
};

export default PriceFilter;