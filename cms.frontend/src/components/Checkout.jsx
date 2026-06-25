import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useCart } from '../context/CartContext';
import axiosClient from '../api/axiosClient';

const Checkout = () => {
    const { cartItems, cartTotal, clearCart } = useCart();
    const navigate = useNavigate();
    
    const [formData, setFormData] = useState({
        fullName: '',
        email: '',
        phone: '',
        address: '',
        note: ''
    });
    
    const [loading, setLoading] = useState(false);

    if (cartItems.length === 0) {
        return (
            <div className="text-center p-5 bg-white shadow-sm border rounded">
                <h4>Giỏ hàng của bạn đang trống!</h4>
                <button className="btn btn-primary mt-3" onClick={() => navigate('/')}>Quay lại mua sắm</button>
            </div>
        );
    }

    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        
        // Kiểm tra validation cơ bản
        if (!formData.fullName || !formData.phone || !formData.address) {
            alert('Vui lòng nhập đầy đủ thông tin bắt buộc: Họ tên, Số điện thoại và Địa chỉ!');
            return;
        }

        try {
            setLoading(true);
            const payload = {
                customerName: formData.fullName,
                customerEmail: formData.email,
                customerPhone: formData.phone,
                customerAddress: formData.address,
                note: formData.note,
                totalAmount: cartTotal,
                orderDetails: cartItems.map(item => ({
                    productId: item.id,
                    quantity: item.quantity,
                    price: item.price
                }))
            };

            await axiosClient.post('/Orders/Checkout', payload);
            alert('Đặt hàng thành công! Vui lòng kiểm tra email của bạn.');
            clearCart();
            navigate('/');
        } catch (error) {
            console.error("Lỗi đặt hàng:", error);
            alert('Đã xảy ra lỗi khi đặt hàng. Vui lòng thử lại sau.');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="bg-white p-4 shadow-sm border rounded">
            <h4 className="fw-bold mb-4 border-bottom pb-2">THÔNG TIN THANH TOÁN</h4>
            <div className="row">
                <div className="col-md-7">
                    <form onSubmit={handleSubmit}>
                        <div className="mb-3">
                            <label className="form-label fw-bold">Họ và tên *</label>
                            <input type="text" className="form-control" name="fullName" value={formData.fullName} onChange={handleChange} required />
                        </div>
                        <div className="mb-3">
                            <label className="form-label fw-bold">Số điện thoại *</label>
                            <input type="tel" className="form-control" name="phone" value={formData.phone} onChange={handleChange} required />
                        </div>
                        <div className="mb-3">
                            <label className="form-label fw-bold">Email</label>
                            <input type="email" className="form-control" name="email" value={formData.email} onChange={handleChange} />
                        </div>
                        <div className="mb-3">
                            <label className="form-label fw-bold">Địa chỉ nhận hàng *</label>
                            <textarea className="form-control" name="address" rows="3" value={formData.address} onChange={handleChange} required></textarea>
                        </div>
                        <div className="mb-4">
                            <label className="form-label fw-bold">Ghi chú thêm</label>
                            <textarea className="form-control" name="note" rows="2" value={formData.note} onChange={handleChange}></textarea>
                        </div>
                        <button type="submit" className="btn btn-danger btn-lg w-100 fw-bold" disabled={loading}>
                            {loading ? 'ĐANG XỬ LÝ...' : 'XÁC NHẬN ĐẶT HÀNG'}
                        </button>
                    </form>
                </div>
                
                <div className="col-md-5 mt-4 mt-md-0">
                    <div className="card border-0 bg-light rounded">
                        <div className="card-body">
                            <h5 className="fw-bold mb-3 border-bottom pb-2">ĐƠN HÀNG CỦA BẠN</h5>
                            {cartItems.map((item, index) => (
                                <div key={index} className="d-flex justify-content-between mb-2 small">
                                    <span className="text-truncate" style={{maxWidth: '60%'}}>
                                        {item.quantity} x {item.name}
                                    </span>
                                    <span className="fw-bold">{(item.price * item.quantity).toLocaleString('vi-VN')} ₫</span>
                                </div>
                            ))}
                            <hr />
                            <div className="d-flex justify-content-between">
                                <span className="fw-bold">Tổng thanh toán:</span>
                                <span className="fw-bold text-danger fs-5">{cartTotal.toLocaleString('vi-VN')} ₫</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Checkout;
