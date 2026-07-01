import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import customerService from '../services/customerService';

const Profile = () => {
    const navigate = useNavigate();
    const [formData, setFormData] = useState({
        fullName: '',
        phone: '',
        address: '',
        newPassword: '',
        confirmPassword: ''
    });
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');
    const [loading, setLoading] = useState(false);
    const [customerInfo, setCustomerInfo] = useState(null);

    useEffect(() => {
        // Lấy thông tin từ LocalStorage khi load trang
        const savedCustomer = JSON.parse(localStorage.getItem('customer'));
        if (!savedCustomer) {
            navigate('/login');
            return;
        }
        setCustomerInfo(savedCustomer);
        setFormData({
            ...formData,
            fullName: savedCustomer.fullName || savedCustomer.FullName || '',
            phone: savedCustomer.phone || savedCustomer.Phone || '',
            address: savedCustomer.address || savedCustomer.Address || ''
        });
    }, [navigate]);

    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const handleUpdate = async (e) => {
        e.preventDefault();
        setError('');
        setSuccess('');

        if (!formData.fullName) {
            setError('Họ tên không được để trống.');
            return;
        }

        if (formData.newPassword && formData.newPassword !== formData.confirmPassword) {
            setError('Mật khẩu xác nhận không khớp.');
            return;
        }

        try {
            setLoading(true);
            const response = await customerService.updateProfile(customerInfo.id || customerInfo.Id, {
                fullName: formData.fullName,
                phone: formData.phone,
                address: formData.address,
                newPassword: formData.newPassword
            });
            
            const data = response.data ? response.data : response;
            
            // Cập nhật lại thông tin mới vào LocalStorage
            localStorage.setItem('customer', JSON.stringify(data.customer));
            setCustomerInfo(data.customer);
            
            // Xóa rỗng ô nhập mật khẩu sau khi cập nhật thành công
            setFormData({ ...formData, newPassword: '', confirmPassword: '' });
            
            setSuccess('Cập nhật thông tin thành công!');
            
            // Load lại trang sau 2 giây để Header cập nhật tên mới nếu có đổi
            setTimeout(() => {
                window.location.reload();
            }, 1500);

        } catch (err) {
            setError(err.response?.data?.message || 'Cập nhật thất bại. Vui lòng thử lại!');
        } finally {
            setLoading(false);
        }
    };

    if (!customerInfo) return null;

    return (
        <div className="container mt-4 mb-5 fade-in-up">
            <div className="row justify-content-center">
                {/* Cột trái: Thông tin tổng quan (Avatar) */}
                <div className="col-lg-4 mb-4">
                    <div className="card border-0 shadow-sm rounded-4 text-center p-4 h-100">
                        <div className="avatar bg-gradient-primary text-white rounded-circle d-flex align-items-center justify-content-center mx-auto mb-3 shadow" 
                             style={{width: '120px', height: '120px', fontSize: '50px', background: 'linear-gradient(135deg, #0d6efd, #0dcaf0)'}}>
                            <i className="fa-solid fa-user-astronaut"></i>
                        </div>
                        <h4 className="fw-bold text-dark mb-1">{customerInfo.fullName || customerInfo.FullName}</h4>
                        <p className="text-muted small mb-3">{customerInfo.email || customerInfo.Email}</p>
                        
                        <div className="d-flex justify-content-center gap-2 mb-4">
                            <span className="badge bg-success bg-opacity-10 text-success rounded-pill px-3 py-2 border border-success"><i className="fa-solid fa-check-circle me-1"></i> Thành viên</span>
                        </div>

                        <div className="d-grid mb-4">
                            <button className="btn btn-outline-primary rounded-pill fw-bold shadow-sm" onClick={() => window.location.href = '/order-history'}>
                                <i className="fa-solid fa-clock-rotate-left me-2"></i> Lịch Sử Mua Hàng
                            </button>
                        </div>

                        <hr className="text-muted opacity-25" />
                        <div className="text-start mt-3">
                            <p className="text-muted small mb-2"><i className="fa-solid fa-phone me-2 text-primary"></i> SĐT: <strong className="text-dark">{customerInfo.phone || customerInfo.Phone || 'Chưa cập nhật'}</strong></p>
                            <p className="text-muted small mb-0"><i className="fa-solid fa-location-dot me-2 text-primary"></i> Địa chỉ: <strong className="text-dark">{customerInfo.address || customerInfo.Address || 'Chưa cập nhật'}</strong></p>
                        </div>
                    </div>
                </div>

                {/* Cột phải: Form cập nhật thông tin */}
                <div className="col-lg-8">
                    <div className="card border-0 shadow-sm rounded-4 p-4 p-md-5">
                        <h4 className="fw-bold mb-4 border-bottom pb-3"><i className="fa-solid fa-pen-to-square me-2 text-primary"></i> Cập Nhật Hồ Sơ</h4>
                        
                        {error && <div className="alert alert-danger p-3 small rounded-3 border-0 border-start border-danger border-4 shadow-sm"><i className="fa-solid fa-circle-exclamation me-2"></i>{error}</div>}
                        {success && <div className="alert alert-success p-3 small rounded-3 border-0 border-start border-success border-4 shadow-sm"><i className="fa-solid fa-circle-check me-2"></i>{success}</div>}
                        
                        <form onSubmit={handleUpdate}>
                            <div className="mb-4">
                                <label className="form-label fw-bold text-secondary small">Họ và Tên (*)</label>
                                <input 
                                    type="text" 
                                    name="fullName"
                                    className="form-control form-control-lg bg-light border-0 shadow-none rounded-3" 
                                    value={formData.fullName}
                                    onChange={handleChange}
                                />
                            </div>

                            <div className="row mb-4">
                                <div className="col-md-6 mb-4 mb-md-0">
                                    <label className="form-label fw-bold text-secondary small">Số Điện Thoại</label>
                                    <input 
                                        type="text" 
                                        name="phone"
                                        className="form-control form-control-lg bg-light border-0 shadow-none rounded-3" 
                                        value={formData.phone}
                                        onChange={handleChange}
                                    />
                                </div>
                                <div className="col-md-6">
                                    <label className="form-label fw-bold text-secondary small">Địa Chỉ Giao Hàng</label>
                                    <input 
                                        type="text" 
                                        name="address"
                                        className="form-control form-control-lg bg-light border-0 shadow-none rounded-3" 
                                        value={formData.address}
                                        onChange={handleChange}
                                    />
                                </div>
                            </div>

                            <div className="p-4 bg-light rounded-4 mb-4 mt-5 border">
                                <h6 className="fw-bold text-dark mb-3"><i className="fa-solid fa-shield-halved me-2 text-warning"></i> Đổi Mật Khẩu (Bỏ trống nếu không đổi)</h6>
                                <div className="row">
                                    <div className="col-md-6 mb-3 mb-md-0">
                                        <input 
                                            type="password" 
                                            name="newPassword"
                                            className="form-control" 
                                            placeholder="Nhập mật khẩu mới" 
                                            value={formData.newPassword}
                                            onChange={handleChange}
                                        />
                                    </div>
                                    <div className="col-md-6">
                                        <input 
                                            type="password" 
                                            name="confirmPassword"
                                            className="form-control" 
                                            placeholder="Xác nhận mật khẩu mới" 
                                            value={formData.confirmPassword}
                                            onChange={handleChange}
                                        />
                                    </div>
                                </div>
                            </div>
                            
                            <div className="text-end mt-4">
                                <button type="submit" className="btn btn-primary btn-lg rounded-pill px-5 fw-bold shadow-sm" disabled={loading}>
                                    {loading ? (<span><i className="fa-solid fa-spinner fa-spin me-2"></i> ĐANG LƯU...</span>) : (<span><i className="fa-solid fa-floppy-disk me-2"></i> LƯU THAY ĐỔI</span>)}
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Profile;
