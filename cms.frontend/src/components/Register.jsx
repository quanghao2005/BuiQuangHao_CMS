import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import customerService from '../services/customerService';

const Register = () => {
    const [formData, setFormData] = useState({
        fullName: '',
        email: '',
        phone: '',
        address: '',
        password: '',
        confirmPassword: ''
    });
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');
    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();

    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const handleRegister = async (e) => {
        e.preventDefault();
        setError('');
        setSuccess('');

        if (!formData.fullName || !formData.email || !formData.password) {
            setError('Vui lòng nhập đầy đủ Họ tên, Email và Mật khẩu.');
            return;
        }

        if (formData.password !== formData.confirmPassword) {
            setError('Mật khẩu xác nhận không khớp.');
            return;
        }

        try {
            setLoading(true);
            const response = await customerService.register({
                fullName: formData.fullName,
                email: formData.email,
                phone: formData.phone,
                address: formData.address,
                password: formData.password
            });
            
            setSuccess('Đăng ký tài khoản thành công! Tự động chuyển đến trang Đăng nhập...');
            setTimeout(() => {
                navigate('/login');
            }, 2000);
        } catch (err) {
            setError(err.response?.data?.message || 'Đăng ký thất bại. Vui lòng thử lại!');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="bg-white p-5 shadow-sm border rounded" style={{ maxWidth: '500px', margin: '0 auto', marginTop: '30px' }}>
            <h3 className="text-center fw-bold text-dark mb-4">Đăng Ký Tài Khoản</h3>
            
            {error && <div className="alert alert-danger p-2 small">{error}</div>}
            {success && <div className="alert alert-success p-2 small">{success}</div>}
            
            <form onSubmit={handleRegister}>
                <input 
                    type="text" 
                    name="fullName"
                    className="form-control mb-3" 
                    placeholder="Họ và tên (*)" 
                    value={formData.fullName}
                    onChange={handleChange}
                />
                <input 
                    type="email" 
                    name="email"
                    className="form-control mb-3" 
                    placeholder="Email (*)" 
                    value={formData.email}
                    onChange={handleChange}
                />
                <input 
                    type="text" 
                    name="phone"
                    className="form-control mb-3" 
                    placeholder="Số điện thoại" 
                    value={formData.phone}
                    onChange={handleChange}
                />
                <input 
                    type="text" 
                    name="address"
                    className="form-control mb-3" 
                    placeholder="Địa chỉ" 
                    value={formData.address}
                    onChange={handleChange}
                />
                <div className="row mb-4">
                    <div className="col-6">
                        <input 
                            type="password" 
                            name="password"
                            className="form-control" 
                            placeholder="Mật khẩu (*)" 
                            value={formData.password}
                            onChange={handleChange}
                        />
                    </div>
                    <div className="col-6">
                        <input 
                            type="password" 
                            name="confirmPassword"
                            className="form-control" 
                            placeholder="Xác nhận MK (*)" 
                            value={formData.confirmPassword}
                            onChange={handleChange}
                        />
                    </div>
                </div>
                
                <button type="submit" className="btn btn-primary w-100 fw-bold" disabled={loading}>
                    {loading ? 'ĐANG XỬ LÝ...' : 'TẠO TÀI KHOẢN'}
                </button>
            </form>
            
            <div className="text-center mt-3 text-muted small">
                Đã có tài khoản? <Link to="/login" className="text-primary fw-bold text-decoration-none">Đăng nhập ngay</Link>
            </div>
        </div>
    );
};

export default Register;
