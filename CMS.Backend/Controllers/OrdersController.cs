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

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
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
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    // Gửi email xác nhận
                    string emailBody = $"<h3>Cảm ơn {customer.FullName} đã đặt hàng!</h3>" +
                                       $"<p>Mã đơn hàng của bạn là: <b>{newOrder.Id}</b></p>" +
                                       $"<p>Tổng tiền: <b>{input.OrderDetails.Sum(o => o.Quantity * o.Price).ToString("N0")} ₫</b></p>" +
                                       $"<p>Chúng tôi sẽ giao hàng đến: {customer.Address}</p>";
                    await CMS.Backend.Helpers.EmailHelper.SendEmailAsync(customer.Email, "Xác nhận đơn hàng HaoCMS", emailBody);

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