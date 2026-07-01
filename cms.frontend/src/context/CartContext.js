import React, { createContext, useState, useEffect, useContext } from 'react';

const CartContext = createContext();

export const useCart = () => useContext(CartContext);

export const CartProvider = ({ children }) => {
    const [cartItems, setCartItems] = useState(() => {
        const localData = localStorage.getItem('cartItems');
        return localData ? JSON.parse(localData) : [];
    });

    useEffect(() => {
        localStorage.setItem('cartItems', JSON.stringify(cartItems));
    }, [cartItems]);

    const addToCart = (product, quantity = 1) => {
        const customer = localStorage.getItem('customer');
        if (!customer) {
            alert('Bạn cần đăng nhập để có thể mua hàng!');
            window.location.href = '/login';
            return;
        }

        if (!product || product.stockQuantity < quantity) {
            alert('Số lượng sản phẩm trong kho không đủ!');
            return;
        }

        setCartItems(prev => {
            const existing = prev.find(item => item.id === product.id);
            if (existing) {
                if (existing.quantity + quantity > product.stockQuantity) {
                    alert('Số lượng vượt quá số hàng có sẵn trong kho!');
                    return prev;
                }
                return prev.map(item =>
                    item.id === product.id ? { ...item, quantity: item.quantity + quantity } : item
                );
            }
            return [...prev, { ...product, quantity }];
        });
        alert('Đã thêm sản phẩm vào giỏ hàng!');
    };

    const removeFromCart = (id) => {
        setCartItems(prev => prev.filter(item => item.id !== id));
    };

    const updateQuantity = (id, newQuantity, stockQuantity) => {
        if (newQuantity < 1) return;
        if (newQuantity > stockQuantity) {
            alert('Số lượng vượt quá số hàng có sẵn trong kho!');
            return;
        }
        setCartItems(prev => prev.map(item => 
            item.id === id ? { ...item, quantity: newQuantity } : item
        ));
    };

    const clearCart = () => setCartItems([]);

    const cartCount = cartItems.reduce((acc, item) => acc + item.quantity, 0);
    const cartTotal = cartItems.reduce((acc, item) => acc + (item.price * item.quantity), 0);

    return (
        <CartContext.Provider value={{ cartItems, addToCart, removeFromCart, updateQuantity, clearCart, cartCount, cartTotal }}>
            {children}
        </CartContext.Provider>
    );
};
