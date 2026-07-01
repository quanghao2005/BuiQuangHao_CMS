import axiosClient from '../api/axiosClient';

const blogService = {
    // Lấy danh sách bài viết 
    getAllPosts: () => {
        return axiosClient.get('/Posts');
    },
    getPostsByCategory: (categoryId) => {
        // Lưu ý: Đảm bảo Backend C# của bạn có API bắt Route này
        return axiosClient.get(`/Posts/Category/${categoryId}`);
    },
    // Lấy chi tiết bài viết 
    getPostById: (id) => {
        return axiosClient.get(`/Posts/${id}`);
    },

    // Đã đổi tên hàm thành getBlogCategories để khớp 100% với BlogCategoryList.jsx
    getBlogCategories: () => {
        // Lưu ý: Khi nào Backend có API này, đảm bảo Route trên C# là /api/Categories
        return axiosClient.get('/Categories');
    }
};

export default blogService;