import React from 'react';

const Contact = () => (
    <div className="bg-white p-4 shadow-sm border rounded">
        <h4 className="text-dark fw-bold mb-4 border-bottom pb-2">THÔNG TIN LIÊN HỆ</h4>
        <div className="row">
            <div className="col-md-6 mb-3">
                <p><strong>📍 Địa chỉ:</strong> Quận 9, TP. Hồ Chí Minh</p>
                <p><strong>📞 Hotline:</strong> (024) 73086880</p>
                <p><strong>✉️ Email:</strong> support@haocms.com</p>
            </div>
            <div className="col-md-6">
                <textarea className="form-control mb-3" rows="3" placeholder="Nhập lời nhắn của bạn..."></textarea>
                <button className="btn btn-primary w-100">Gửi Liên Hệ</button>
            </div>
        </div>
    </div>
);

export default Contact;