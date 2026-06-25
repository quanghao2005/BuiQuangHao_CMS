import axiosClient from '../api/axiosClient';

const blogService = {
    // Lấy danh sách bài viết (Khớp với /api/Posts trong Swagger)
    getAllPosts: () => {
        return axiosClient.get('/Posts');
    },

    // Lấy chi tiết bài viết (Khớp với /api/Posts/{id} trong Swagger)
    getPostById: (id) => {
        return axiosClient.get(`/Posts/${id}`);
    },

    // Lưu ý: Hiện tại Swagger chưa có /api/Categories
    // Nếu bạn muốn lấy danh mục, hãy tạo Controller tương ứng ở Backend
    getBlogCategories: () => {
        // Sau khi tạo Controller ở Backend và nó hiện trong Swagger, hãy thay URL vào đây
        return axiosClient.get('/Categories');
    }
};

export default blogService;