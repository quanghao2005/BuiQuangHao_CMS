/*
 * Họ Tên : Bùi Quang Hào
 * MSSV : 2123110043
 */
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMS.Data;
using CMS.Data.Entities;
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
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _env;

        public OrdersController(ApplicationDbContext context, Microsoft.AspNetCore.Hosting.IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpPost("Checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequestDTO input)
        {
            if (input == null || input.OrderDetails == null || input.OrderDetails.Count == 0)
            {
                return BadRequest(new { message = "Giỏ hàng rỗng hoặc dữ liệu không hợp lệ!" });
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // 1. Tìm hoặc tạo mới Customer
                    var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == input.CustomerEmail || c.Phone == input.CustomerPhone);
                    if (customer == null)
                    {
                        customer = new Customer
                        {
                            FullName = input.CustomerName,
                            Email = input.CustomerEmail ?? "guest@example.com",
                            Phone = input.CustomerPhone,
                            Address = input.CustomerAddress,
                            Password = "GuestPassword123" // Mặc định cho guest
                        };
                        _context.Customers.Add(customer);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        // Nếu tìm thấy khách hàng cũ (trùng số điện thoại/email), 
                        // cập nhật lại Họ Tên và Địa chỉ mới nhất theo Form Đặt Hàng hiện tại.
                        customer.FullName = input.CustomerName;
                        customer.Address = input.CustomerAddress;
                        _context.Customers.Update(customer);
                    }

                    // 2. Tạo Order
                    var newOrder = new Order
                    {
                        OrderDate = DateTime.Now,
                        CustomerId = customer.Id,
                        Status = 0,
                        Notes = input.Note
                    };

                    _context.Orders.Add(newOrder);
                    await _context.SaveChangesAsync();

                    // 3. Xử lý OrderDetails và trừ Stock
                    string productRowsHtml = "";
                    var inlineImages = new Dictionary<string, string>();

                    foreach (var item in input.OrderDetails)
                    {
                        var product = await _context.Products.FindAsync(item.ProductId);
                        if (product == null)
                        {
                            return BadRequest(new { message = $"Sản phẩm ID {item.ProductId} không tồn tại!" });
                        }

                        if (product.StockQuantity < item.Quantity)
                        {
                            return BadRequest(new { message = $"Sản phẩm {product.Name} không đủ tồn kho!" });
                        }

                        var orderDetail = new OrderDetail
                        {
                            OrderId = newOrder.Id,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            UnitPrice = product.Price
                        };

                        product.StockQuantity -= item.Quantity;
                        _context.OrderDetails.Add(orderDetail);

                        // Xử lý hình ảnh nhúng (Embedded Image - CID)
                        string cid = $"img_{product.Id}";
                        string imgUrl = "";

                        if (!string.IsNullOrEmpty(product.ImageUrl))
                        {
                            string physicalPath = System.IO.Path.Combine(_env.WebRootPath, product.ImageUrl.TrimStart('/'));
                            if (System.IO.File.Exists(physicalPath))
                            {
                                if (!inlineImages.ContainsKey(cid))
                                {
                                    inlineImages.Add(cid, physicalPath);
                                }
                                imgUrl = $"cid:{cid}";
                            }
                        }

                        if (string.IsNullOrEmpty(imgUrl))
                        {
                            imgUrl = "https://via.placeholder.com/60";
                        }

                        productRowsHtml += $@"
                            <tr>
                                <td style='padding: 12px 5px; border-bottom: 1px solid #eee;'>
                                    <img src='{imgUrl}' alt='{product.Name}' style='width: 60px; height: 60px; object-fit: cover; border-radius: 6px; border: 1px solid #eee;' />
                                </td>
                                <td style='padding: 12px 5px; border-bottom: 1px solid #eee;'>
                                    <strong style='color: #333; font-size: 15px;'>{product.Name}</strong>
                                </td>
                                <td style='padding: 12px 5px; border-bottom: 1px solid #eee; text-align: center; color: #555;'>
                                    x{item.Quantity}
                                </td>
                                <td style='padding: 12px 5px; border-bottom: 1px solid #eee; text-align: right; color: #e11d48; font-weight: bold;'>
                                    {(item.Price * item.Quantity).ToString("N0")} ₫
                                </td>
                            </tr>";
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    // Gửi email xác nhận với Template HTML chuyên nghiệp
                    string emailBody = $@"
                        <div style='font-family: ""Segoe UI"", Tahoma, Geneva, Verdana, sans-serif; max-width: 600px; margin: 0 auto; border: 1px solid #e5e7eb; border-radius: 10px; overflow: hidden; box-shadow: 0 4px 6px rgba(0,0,0,0.05);'>
                            <div style='background: linear-gradient(135deg, #4f46e5 0%, #7c3aed 100%); padding: 25px; text-align: center; color: white;'>
                                <h2 style='margin: 0; font-size: 24px; letter-spacing: 1px;'>XÁC NHẬN ĐƠN HÀNG</h2>
                                <p style='margin: 8px 0 0 0; opacity: 0.9; font-size: 14px;'>Cảm ơn bạn đã mua sắm tại HaoCMS Store!</p>
                            </div>
                            <div style='padding: 30px; background-color: #ffffff;'>
                                <p style='font-size: 16px; color: #374151;'>Xin chào <strong style='color: #111827;'>{customer.FullName}</strong>,</p>
                                <p style='font-size: 15px; color: #4b5563; line-height: 1.5;'>Đơn hàng <strong style='color: #4f46e5;'>#{newOrder.Id}</strong> của bạn đã được tiếp nhận và đang trong quá trình xử lý.</p>
                                
                                <h4 style='border-bottom: 2px solid #f3f4f6; padding-bottom: 10px; margin-top: 30px; color: #111827; font-size: 16px;'>🛍️ CHI TIẾT ĐƠN HÀNG</h4>
                                <table style='width: 100%; border-collapse: collapse; margin-bottom: 20px;'>
                                    {productRowsHtml}
                                </table>
                                
                                <div style='text-align: right; margin-top: 15px; padding-top: 15px; border-top: 1px dashed #d1d5db;'>
                                    <span style='font-size: 16px; color: #4b5563;'>Tổng cộng: </span>
                                    <strong style='color: #e11d48; font-size: 24px;'>{input.OrderDetails.Sum(o => o.Quantity * o.Price).ToString("N0")} ₫</strong>
                                </div>
                                
                                <h4 style='border-bottom: 2px solid #f3f4f6; padding-bottom: 10px; margin-top: 35px; color: #111827; font-size: 16px;'>📍 THÔNG TIN GIAO HÀNG</h4>
                                <table style='width: 100%; font-size: 15px; color: #4b5563;'>
                                    <tr><td style='padding: 4px 0; width: 100px;'><strong>Người nhận:</strong></td><td>{customer.FullName}</td></tr>
                                    <tr><td style='padding: 4px 0;'><strong>Điện thoại:</strong></td><td>{customer.Phone}</td></tr>
                                    <tr><td style='padding: 4px 0;'><strong>Địa chỉ:</strong></td><td>{customer.Address}</td></tr>
                                </table>
                            </div>
                            <div style='background-color: #f9fafb; padding: 20px; text-align: center; font-size: 13px; color: #6b7280; border-top: 1px solid #e5e7eb;'>
                                &copy; 2026 HaoCMS Store. Mọi thắc mắc vui lòng liên hệ hotline: 1800-1234.
                            </div>
                        </div>";
                    await CMS.Backend.Helpers.EmailHelper.SendEmailAsync(customer.Email, "Xác nhận đơn hàng HaoCMS", emailBody, inlineImages);

                    return StatusCode(201, new { message = "Đặt hàng thành công!" });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, new { message = "Lỗi tạo đơn hàng", detail = ex.Message });
                }
            }
        }

        [HttpGet("History/{customerId}")]
        public async Task<IActionResult> GetHistory(int customerId)
        {
            try
            {
                var orders = await _context.Orders
                    .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                    .Where(o => o.CustomerId == customerId)
                    .OrderByDescending(o => o.OrderDate)
                    .Select(o => new {
                        Id = o.Id,
                        OrderDate = o.OrderDate,
                        Status = o.Status,
                        Notes = o.Notes,
                        TotalAmount = o.OrderDetails.Sum(od => od.Quantity * od.UnitPrice),
                        Details = o.OrderDetails.Select(od => new {
                            ProductId = od.ProductId,
                            ProductName = od.Product.Name,
                            ProductImage = od.Product.ImageUrl,
                            Quantity = od.Quantity,
                            UnitPrice = od.UnitPrice
                        }).ToList()
                    })
                    .ToListAsync();

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi truy xuất lịch sử đơn hàng", detail = ex.Message });
            }
        }
    }

    public class CheckoutRequestDTO
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
        public string Note { get; set; }
        public List<CheckoutItemDTO> OrderDetails { get; set; }
    }

    public class CheckoutItemDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}