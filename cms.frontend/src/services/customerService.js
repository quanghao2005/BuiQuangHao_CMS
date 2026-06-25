import axiosClient from '../api/axiosClient';

const customerService = {
    login: (data) => {
        return axiosClient.post('/Customers/Login', data);
    },
    register: (data) => {
        return axiosClient.post('/Customers/Register', data);
    },
    forgotPassword: (data) => {
        return axiosClient.post('/Customers/ForgotPassword', data);
    },
    updateProfile: (id, data) => {
        return axiosClient.put(`/Customers/UpdateProfile/${id}`, data);
    }
};

export default customerService;
