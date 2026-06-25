import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import customerService from '../services/customerService';

const Login = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();

    const handleLogin = async (e) => {
        e.preventDefault();
        setError('');
        
        if (!email || !password) {
            setError('Vui lòng nhập email và mật khẩu');
            return;
        }

        try {
            setLoading(true);
            const response = await customerService.login({ email, password });
            const data = response.data ? response.data : response;
            
            // Lưu thông tin user vào localStorage
            localStorage.setItem('customer', JSON.stringify(data.customer));
            
            // Chuyển hướng về trang chủ và tải lại trang để header nhận diện
            window.location.href = '/';
        } catch (err) {
            setError(err.response?.data?.message || 'Đăng nhập thất bại. Vui lòng thử lại!');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="bg-white p-5 shadow-sm border rounded" style={{ maxWidth: '400px', margin: '0 auto', marginTop: '50px' }}>
            <h3 className="text-center fw-bold text-dark mb-4">Đăng Nhập</h3>
            {error && <div className="alert alert-danger p-2 small">{error}</div>}
            
            <form onSubmit={handleLogin}>
                <input 
                    type="email" 
                    className="form-control mb-3" 
                    placeholder="Email của bạn" 
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                />
                <input 
                    type="password" 
                    className="form-control mb-4" 
                    placeholder="Mật khẩu" 
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                />
                <div className="text-end mb-4 mt-n2">
                    <Link to="/forgot-password" className="text-primary small text-decoration-none">Quên mật khẩu?</Link>
                </div>
                <button type="submit" className="btn btn-primary w-100 fw-bold" disabled={loading}>
                    {loading ? 'ĐANG ĐĂNG NHẬP...' : 'ĐĂNG NHẬP'}
                </button>
            </form>
            
            <div className="text-center mt-3 text-muted small">
                Chưa có tài khoản? <Link to="/register" className="text-primary fw-bold text-decoration-none">Đăng ký ngay</Link>
            </div>
        </div>
    );
};

export default Login;