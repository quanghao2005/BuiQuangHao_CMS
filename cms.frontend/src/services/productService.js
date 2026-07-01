import axiosClient from '../api/axiosClient';

const productService = {
    // Hàm gọi API lấy toàn bộ danh sách sản phẩm
    getAllProducts: (page = 1, limit = 9, search = '', minPrice = '', maxPrice = '') => {
        let url = `/Products?page=${page}&limit=${limit}&search=${encodeURIComponent(search)}`;
        if (minPrice) url += `&minPrice=${minPrice}`;
        if (maxPrice) url += `&maxPrice=${maxPrice}`;
        return axiosClient.get(url);
    },

    // Hàm gọi API lấy danh sách sản phẩm theo ID danh mục
    getProductsByCategory: (categoryId, page = 1, limit = 9, minPrice = '', maxPrice = '') => {
        let url = `/Products/Category/${categoryId}?page=${page}&limit=${limit}`;
        if (minPrice) url += `&minPrice=${minPrice}`;
        if (maxPrice) url += `&maxPrice=${maxPrice}`;
        return axiosClient.get(url);
    },
    getHotProducts: () => {
        return axiosClient.get('/Products/Hot');
    },
    getFeaturedProducts: () => {
        return axiosClient.get('/Products/Featured');
    },
    getNewProducts: () => {
        return axiosClient.get('/Products/New');
    },
    getProductById: (id) => {
        return axiosClient.get(`/Products/${id}`);
    }
};

export default productService;