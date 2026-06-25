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
        <div className="bg-white p-5 shadow-sm border rounded" style={{ maxWidth: '600px', margin: '0 auto', marginTop: '30px' }}>
            <h3 className="text-center fw-bold text-dark mb-4">Hồ Sơ Của Tôi</h3>
            
            <div className="alert alert-info small mb-4">
                <strong>Email đăng nhập:</strong> {customerInfo.email || customerInfo.Email} <br/>
                <i>(Email không thể thay đổi để đảm bảo bảo mật tài khoản)</i>
            </div>

            {error && <div className="alert alert-danger p-2 small">{error}</div>}
            {success && <div className="alert alert-success p-2 small">{success}</div>}
            
            <form onSubmit={handleUpdate}>
                <div className="mb-3">
                    <label className="form-label fw-bold small">Họ và Tên (*)</label>
                    <input 
                        type="text" 
                        name="fullName"
                        className="form-control" 
                        value={formData.fullName}
                        onChange={handleChange}
                    />
                </div>

                <div className="row mb-3">
                    <div className="col-md-6">
                        <label className="form-label fw-bold small">Số Điện Thoại</label>
                        <input 
                            type="text" 
                            name="phone"
                            className="form-control" 
                            value={formData.phone}
                            onChange={handleChange}
                        />
                    </div>
                    <div className="col-md-6">
                        <label className="form-label fw-bold small">Địa Chỉ Giao Hàng</label>
                        <input 
                            type="text" 
                            name="address"
                            className="form-control" 
                            value={formData.address}
                            onChange={handleChange}
                        />
                    </div>
                </div>

                <hr className="my-4" />
                <h5 className="fw-bold fs-6 mb-3">Đổi Mật Khẩu (Bỏ trống nếu không đổi)</h5>

                <div className="row mb-4">
                    <div className="col-md-6">
                        <input 
                            type="password" 
                            name="newPassword"
                            className="form-control" 
                            placeholder="Mật khẩu mới" 
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
                
                <button type="submit" className="btn btn-success w-100 fw-bold" disabled={loading}>
                    {loading ? 'ĐANG LƯU...' : 'LƯU THAY ĐỔI'}
                </button>
            </form>
        </div>
    );
};

export default Profile;
