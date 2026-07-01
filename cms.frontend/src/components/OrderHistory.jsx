import React, { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import orderService from '../services/orderService';

const OrderHistory = () => {
    const navigate = useNavigate();
    const [orders, setOrders] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');

    useEffect(() => {
        const customer = JSON.parse(localStorage.getItem('customer'));
        if (!customer) {
            navigate('/login');
            return;
        }

        const fetchHistory = async () => {
            try {
                const response = await orderService.getHistory(customer.id || customer.Id);
                // Xử lý dữ liệu trả về từ axios interceptor
                setOrders(response.data || response || []);
            } catch (err) {
                console.error("Lỗi lấy lịch sử đơn hàng:", err);
                setError('Không thể tải lịch sử đơn hàng lúc này.');
            } finally {
                setLoading(false);
            }
        };

        fetchHistory();
    }, [navigate]);

    const getStatusBadge = (status) => {
        switch(status) {
            case 0: return <span className="badge bg-warning text-dark px-3 py-2 rounded-pill"><i className="fa-solid fa-hourglass-half me-1"></i> Chờ duyệt</span>;
            case 1: return <span className="badge bg-primary px-3 py-2 rounded-pill"><i className="fa-solid fa-truck-fast me-1"></i> Đang giao</span>;
            case 2: return <span className="badge bg-success px-3 py-2 rounded-pill"><i className="fa-solid fa-check-circle me-1"></i> Đã hoàn thành</span>;
            default: return <span className="badge bg-secondary px-3 py-2 rounded-pill">Không xác định</span>;
        }
    };

    if (loading) {
        return <div className="text-center my-5"><div className="spinner-border text-primary" role="status"></div></div>;
    }

    return (
        <div className="container mt-4 mb-5 fade-in-up">
            <div className="d-flex justify-content-between align-items-center mb-4">
                <h3 className="fw-bold text-dark"><i className="fa-solid fa-clock-rotate-left me-2 text-primary"></i> Lịch Sử Mua Hàng</h3>
                <Link to="/profile" className="btn btn-outline-secondary rounded-pill px-4 fw-bold">
                    <i className="fa-solid fa-arrow-left me-2"></i> Quay lại Hồ sơ
                </Link>
            </div>

            {error && <div className="alert alert-danger">{error}</div>}

            {orders.length === 0 ? (
                <div className="text-center bg-white p-5 rounded-4 shadow-sm">
                    <i className="fa-solid fa-box-open text-muted mb-3" style={{ fontSize: '60px' }}></i>
                    <h5 className="fw-bold">Bạn chưa có đơn hàng nào</h5>
                    <p className="text-muted">Hãy mua sắm để lấp đầy lịch sử của bạn nhé!</p>
                    <Link to="/products" className="btn btn-primary rounded-pill px-4 mt-2 fw-bold">Mua sắm ngay</Link>
                </div>
            ) : (
                <div className="row">
                    {orders.map(order => (
                        <div key={order.id} className="col-12 mb-4">
                            <div className="card border-0 shadow-sm rounded-4">
                                <div className="card-header bg-white border-bottom-0 pt-4 pb-0 px-4 d-flex justify-content-between align-items-center">
                                    <div>
                                        <span className="fw-bold fs-5 text-dark">Đơn hàng #{order.id}</span>
                                        <p className="text-muted small mb-0"><i className="fa-regular fa-calendar me-1"></i> Đặt lúc: {new Date(order.orderDate).toLocaleString('vi-VN')}</p>
                                    </div>
                                    <div>
                                        {getStatusBadge(order.status)}
                                    </div>
                                </div>
                                <div className="card-body px-4">
                                    <hr className="text-muted opacity-25 mt-0 mb-3" />
                                    {order.details.map((detail, index) => (
                                        <div key={index} className="d-flex align-items-center mb-3">
                                            <img 
                                                src={detail.productImage ? `https://localhost:7271${detail.productImage}` : '/images/no-image.png'} 
                                                alt={detail.productName} 
                                                className="rounded-3 border object-fit-cover"
                                                style={{ width: '80px', height: '80px' }}
                                                onError={(e) => { e.target.src = '/images/no-image.png'; }}
                                            />
                                            <div className="ms-3 flex-grow-1">
                                                <h6 className="fw-bold mb-1 text-dark">{detail.productName}</h6>
                                                <div className="text-muted small">Số lượng: <b>{detail.quantity}</b> x {detail.unitPrice.toLocaleString('vi-VN')} ₫</div>
                                            </div>
                                            <div className="fw-bold text-danger">
                                                {(detail.quantity * detail.unitPrice).toLocaleString('vi-VN')} ₫
                                            </div>
                                        </div>
                                    ))}
                                </div>
                                <div className="card-footer bg-light border-top-0 rounded-bottom-4 px-4 py-3 d-flex justify-content-between align-items-center">
                                    {order.notes ? (
                                        <span className="text-muted small"><i className="fa-solid fa-note-sticky me-1"></i> Ghi chú: {order.notes}</span>
                                    ) : <span></span>}
                                    <h5 className="fw-bold text-danger mb-0">
                                        Tổng tiền: {order.totalAmount.toLocaleString('vi-VN')} ₫
                                    </h5>
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
};

export default OrderHistory;
