import React from 'react';
import { useSearchParams, useNavigate, useLocation } from 'react-router-dom';

const PriceFilter = () => {
    const [searchParams] = useSearchParams();
    const navigate = useNavigate();
    const location = useLocation();

    // Lấy giá trị hiện tại trên thanh URL
    const currentMin = searchParams.get('min');
    const currentMax = searchParams.get('max');

    // Hàm cập nhật URL khi người dùng bấm chọn giá
    const handleFilter = (min, max) => {
        const params = new URLSearchParams(searchParams);

        if (min !== null) params.set('min', min);
        else params.delete('min');

        if (max !== null) params.set('max', max);
        else params.delete('max');

        // Điều hướng nhưng giữ nguyên trang hiện tại (Trang chủ hoặc trang Danh mục)
        navigate(`${location.pathname}?${params.toString()}`);
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
        { label: 'Trên 10 triệu', min: 10000000, max: null }
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
        </div>
    );
};

export default PriceFilter;