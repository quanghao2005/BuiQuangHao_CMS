import axiosClient from '../api/axiosClient';

const productService = {
    // Hàm gọi API lấy toàn bộ danh sách sản phẩm
    getAllProducts: (page = 1, limit = 9, search = '') => {
        const url = `/Products?page=${page}&limit=${limit}&search=${encodeURIComponent(search)}`;
        return axiosClient.get(url);
    },

    // Hàm gọi API lấy danh sách sản phẩm theo ID danh mục
    getProductsByCategory: (categoryId, page = 1, limit = 9) => {
        const url = `/Products/Category/${categoryId}?page=${page}&limit=${limit}`;
        return axiosClient.get(url);
    },
    getHotProducts: () => {
        return axiosClient.get('/Products/Hot');
    },
    getProductById: (id) => {
        return axiosClient.get(`/Products/${id}`);
    }
};

export default productService;