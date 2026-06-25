import axiosClient from '../api/axiosClient';

const orderService = {
    checkout: (data) => {
        return axiosClient.post('/Orders/Checkout', data);
    },
    getHistory: (customerId) => {
        return axiosClient.get(`/Orders/History/${customerId}`);
    }
};

export default orderService;
