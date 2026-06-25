import React from 'react';
import { Link } from 'react-router-dom';
import { useCart } from '../context/CartContext';

const Cart = () => {
    const { cartItems, removeFromCart, updateQuantity, cartTotal } = useCart();

    if (cartItems.length === 0) {
        return (
            <div className="bg-white p-5 shadow-sm border rounded text-center">
                <i className="fa-solid fa-cart-shopping fs-1 text-muted mb-3 d-block"></i>
                <h4 className="text-dark">Giỏ hàng của bạn đang trống</h4>
                <p className="text-muted">Hãy chọn cho mình một chiếc kính bảo vệ mắt nhé!</p>
                <Link to="/" className="btn btn-outline-primary mt-3">Tiếp tục mua sắm</Link>
            </div>
        );
    }

    return (
        <div className="bg-white p-4 shadow-sm border rounded">
            <h4 className="fw-bold mb-4 border-bottom pb-2">GIỎ HÀNG CỦA BẠN</h4>
            <div className="table-responsive">
                <table className="table align-middle">
                    <thead className="table-light">
                        <tr>
                            <th>Sản phẩm</th>
                            <th>Đơn giá</th>
                            <th style={{ width: '150px' }}>Số lượng</th>
                            <th>Thành tiền</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        {cartItems.map(item => (
                            <tr key={item.id}>
                                <td>
                                    <div className="d-flex align-items-center">
                                        <img 
                                            src={item.imageUrl ? `https://localhost:7271${item.imageUrl}` : '/images/default-product.jpg'} 
                                            alt={item.name} 
                                            style={{ width: '60px', height: '60px', objectFit: 'contain', marginRight: '15px' }} 
                                        />
                                        <div>
                                            <h6 className="mb-0 fw-bold">{item.name}</h6>
                                        </div>
                                    </div>
                                </td>
                                <td>{item.price.toLocaleString('vi-VN')} ₫</td>
                                <td>
                                    <div className="input-group input-group-sm">
                                        <button className="btn btn-outline-secondary" onClick={() => updateQuantity(item.id, item.quantity - 1, item.stockQuantity)}>-</button>
                                        <input type="text" className="form-control text-center" value={item.quantity} readOnly />
                                        <button className="btn btn-outline-secondary" onClick={() => updateQuantity(item.id, item.quantity + 1, item.stockQuantity)}>+</button>
                                    </div>
                                </td>
                                <td className="fw-bold text-danger">
                                    {(item.price * item.quantity).toLocaleString('vi-VN')} ₫
                                </td>
                                <td>
                                    <button className="btn btn-sm btn-outline-danger" onClick={() => removeFromCart(item.id)}>
                                        <i className="fa-solid fa-trash"></i>
                                    </button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>

            <div className="d-flex justify-content-between align-items-center mt-4 pt-3 border-top">
                <Link to="/" className="text-decoration-none text-secondary">
                    <i className="fa-solid fa-arrow-left me-2"></i> Tiếp tục mua sắm
                </Link>
                <div className="text-end">
                    <h5 className="mb-2">Tổng tiền: <span className="fw-bold text-danger fs-4">{cartTotal.toLocaleString('vi-VN')} ₫</span></h5>
                    <Link to="/checkout" className="btn btn-primary btn-lg fw-bold px-5 mt-2" style={{ borderRadius: '8px' }}>
                        THANH TOÁN
                    </Link>
                </div>
            </div>
        </div>
    );
};

export default Cart;