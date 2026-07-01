import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import customerService from '../services/customerService';

const ForgotPassword = () => {
    const [email, setEmail] = useState('');
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');
    const [loading, setLoading] = useState(false);

    const handleForgot = async (e) => {
        e.preventDefault();
        setError('');
        setSuccess('');

        if (!email) {
            setError('Vui lòng nhập Email của bạn!');
            return;
        }

        try {
            setLoading(true);
            const response = await customerService.forgotPassword({ email });
            const data = response.data ? response.data : response;
            setSuccess(data.message || 'Mật khẩu mới đã được gửi vào Email của bạn!');
        } catch (err) {
            setError(err.response?.data?.message || 'Có lỗi xảy ra. Vui lòng thử lại!');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="bg-white p-5 shadow-sm border rounded" style={{ maxWidth: '400px', margin: '0 auto', marginTop: '50px' }}>
            <h3 className="text-center fw-bold text-dark mb-4">Quên Mật Khẩu</h3>
            <p className="text-muted small text-center mb-4">Vui lòng nhập Email bạn đã dùng để đăng ký. Chúng tôi sẽ gửi mật khẩu mới cho bạn.</p>

            {error && <div className="alert alert-danger p-2 small">{error}</div>}
            {success && <div className="alert alert-success p-2 small">{success}</div>}
            
            <form onSubmit={handleForgot}>
                <input 
                    type="email" 
                    className="form-control mb-4" 
                    placeholder="Email của bạn" 
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                />
                <button type="submit" className="btn btn-warning w-100 fw-bold text-dark" disabled={loading}>
                    {loading ? 'ĐANG XỬ LÝ...' : 'KHÔI PHỤC MẬT KHẨU'}
                </button>
            </form>
            
            <div className="text-center mt-3 text-muted small">
                <Link to="/login" className="text-primary text-decoration-none fw-bold">Quay lại Đăng nhập</Link>
            </div>
        </div>
    );
};

export default ForgotPassword;
