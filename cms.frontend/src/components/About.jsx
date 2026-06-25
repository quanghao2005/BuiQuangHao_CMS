import React from 'react';
import { Link } from 'react-router-dom';

const About = () => (
    <div className="bg-white p-5 shadow-sm border rounded text-center">
        <h3 className="text-primary fw-bold mb-4">Về HaoCMS.DigitalGuard</h3>
        <p className="text-muted text-start" style={{ lineHeight: '1.8' }}>
            HaoCMS.DigitalGuard ra đời với sứ mệnh bảo vệ đôi mắt của những người làm việc cường độ cao với màn hình kỹ thuật số (Dân IT, Coder, Game thủ, Designer). Chúng tôi cung cấp các giải pháp tròng kính chống ánh sáng xanh chất lượng cao, thiết kế gọng kính siêu nhẹ, mang lại sự thoải mái tối đa cho 8-12 tiếng làm việc mỗi ngày.
        </p>
        <Link to="/" className="btn btn-primary mt-3 px-4">← Trở về Cửa hàng</Link>
    </div>
);

export default About;