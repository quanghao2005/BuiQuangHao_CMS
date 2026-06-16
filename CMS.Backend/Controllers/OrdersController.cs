/*
 * Họ Tên : Bùi Quang Hào
 * MSSV : 2123110043
 */
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMS.Data;
using CMS.Data.Entities; // Đã bổ sung: Kết nối tới thư mục chứa các lớp thực thể Order, OrderDetail
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace CMS.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// API: Tiếp nhận đơn đặt hàng kèm chi tiết giỏ hàng từ FrontEnd gửi lên
        /// Đường dẫn: POST https://localhost:xxxx/api/Orders
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderInputDTO input)
        {
            // 1. Kiểm tra kịch bản lỗi bảo vệ: Nếu dữ liệu truyền lên trống rỗng hoặc không có sản phẩm nào
            if (input == null || input.CartItems == null || input.CartItems.Count == 0)
            {
                return BadRequest(new { message = "Dữ liệu đơn hàng hoặc giỏ hàng không hợp lệ!" });
            }

            // Sử dụng cơ chế Transaction để đảm bảo: Nếu lưu chi tiết đơn hàng lỗi thì tự động hủy luôn đơn hàng cha
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // --- BƯỚC A: KHỞI TẠO VÀ LƯU THÔNG TIN ĐƠN HÀNG TỔNG QUÁT ---
                    var newOrder = new Order
                    {
                        OrderDate = DateTime.Now,    // Tự động lấy ngày giờ thực tế máy tính lúc mua
                        CustomerId = input.CustomerId,
                        Status = 0,                  // 0: Mặc định đơn hàng mới ở trạng thái "Chờ duyệt"
                        Notes = input.Notes
                    };

                    _context.Orders.Add(newOrder);
                    await _context.SaveChangesAsync(); // Ép hệ thống sinh ra mã ID Đơn hàng (newOrder.Id) tự động tăng

                    // --- BƯỚC B: VÒNG LẶP DUYỆT GIỎ HÀNG ĐỂ LƯU CHI TIẾT ĐƠN HÀNG ---
                    foreach (var item in input.CartItems)
                    {
                        // Truy vấn nhanh từ DB để lấy giá bán thực tế của sản phẩm tại thời điểm mua
                        var product = await _context.Products.FindAsync(item.ProductId);
                        if (product == null)
                        {
                            return BadRequest(new { message = $"Sản phẩm có ID {item.ProductId} không tồn tại trên hệ thống!" });
                        }

                        // Kiểm tra số lượng tồn kho (Bảo vệ hệ thống)
                        if (product.StockQuantity < item.Quantity)
                        {
                            return BadRequest(new { message = $"Sản phẩm {product.Name} không đủ số lượng trong kho!" });
                        }

                        // Khởi tạo thực thể Chi tiết đơn hàng (OrderDetail)
                        var orderDetail = new OrderDetail
                        {
                            OrderId = newOrder.Id, // Gắn mã ID đơn hàng vừa đẻ ra ở Bước A vào đây
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            UnitPrice = product.Price // Lấy giá gốc của sản phẩm nạp vào hóa đơn
                        };

                        // Trừ bớt số lượng tồn kho của sản phẩm
                        product.StockQuantity -= item.Quantity;

                        _context.OrderDetails.Add(orderDetail);
                    }

                    // Chốt lưu toàn bộ danh sách chi tiết đơn hàng và cập nhật lại kho sản phẩm xuống SQL Server
                    await _context.SaveChangesAsync();

                    // Xác nhận hoàn tất toàn bộ tiến trình giao dịch an toàn
                    await transaction.CommitAsync();

                    // Trả về mã thành công 201 Created và gửi ngược lại mã ID đơn hàng vừa tạo cho Frontend
                    return StatusCode(201, new
                    {
                        message = "Đặt hàng thành công trọn vẹn!",
                        orderId = newOrder.Id
                    });
                }
                catch (Exception ex)
                {
                    // Nếu có bất kỳ lỗi nào xảy ra, hủy bỏ toàn bộ dữ liệu rác vừa thêm vào bộ nhớ tạm
                    await transaction.RollbackAsync();
                    return StatusCode(500, new { message = "Lỗi xử lý tạo đơn hàng ngầm", detail = ex.Message });
                }
            }
        }
    }

    // ====================================================================================
    // HỆ THỐNG CÁC LỚP TRUNG GIAN DTO ĐỂ HỨNG DỮ LIỆU TỪ GIỎ HÀNG REACTJS TRUYỀN LÊN
    // ====================================================================================

    public class OrderInputDTO
    {
        public int CustomerId { get; set; }
        public string Notes { get; set; }
        public List<CartItemDTO> CartItems { get; set; } // Danh sách mảng các sản phẩm chọn mua
    }

    public class CartItemDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}