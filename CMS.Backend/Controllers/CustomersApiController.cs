using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMS.Data;
using CMS.Data.Entities;
using System.Threading.Tasks;

namespace CMS.Backend.Controllers
{
    [Route("api/Customers")]
    [ApiController]
    public class CustomersApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CustomersApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] CustomerRegisterDTO input)
        {
            if (string.IsNullOrWhiteSpace(input.Email) || string.IsNullOrWhiteSpace(input.Password))
                return BadRequest(new { message = "Email và mật khẩu không được để trống!" });

            var exists = await _context.Customers.AnyAsync(c => c.Email == input.Email);
            if (exists)
                return BadRequest(new { message = "Email này đã được sử dụng!" });

            var newCustomer = new Customer
            {
                FullName = input.FullName,
                Email = input.Email,
                Phone = input.Phone,
                Address = input.Address,
                Password = BCrypt.Net.BCrypt.HashPassword(input.Password)
            };

            _context.Customers.Add(newCustomer);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đăng ký tài khoản thành công!" });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] CustomerLoginDTO input)
        {
            if (string.IsNullOrWhiteSpace(input.Email) || string.IsNullOrWhiteSpace(input.Password))
                return BadRequest(new { message = "Email và mật khẩu không được để trống!" });

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == input.Email);
            if (customer == null)
                return BadRequest(new { message = "Email hoặc mật khẩu không đúng!" });

            if (!BCrypt.Net.BCrypt.Verify(input.Password, customer.Password))
                return BadRequest(new { message = "Email hoặc mật khẩu không đúng!" });

            return Ok(new { 
                message = "Đăng nhập thành công!",
                customer = new { customer.Id, customer.FullName, customer.Email } 
            });
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO input)
        {
            if (string.IsNullOrWhiteSpace(input.Email))
                return BadRequest(new { message = "Vui lòng nhập Email!" });

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == input.Email);
            if (customer == null)
                return BadRequest(new { message = "Email này chưa được đăng ký trong hệ thống!" });

            // Generate new password
            string newPassword = "Cms@" + new Random().Next(10000, 99999);
            customer.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();

            // Gửi email cho khách hàng
            string emailBody = $"<h3>Xin chào {customer.FullName},</h3>" +
                               $"<p>Hệ thống đã đặt lại mật khẩu của bạn.</p>" +
                               $"<p>Mật khẩu mới để đăng nhập là: <b>{newPassword}</b></p>" +
                               $"<p>Vui lòng đăng nhập và đổi lại mật khẩu để đảm bảo an toàn.</p>";
            await CMS.Backend.Helpers.EmailHelper.SendEmailAsync(customer.Email, "Khôi phục mật khẩu tài khoản HaoCMS", emailBody);

            return Ok(new { message = $"Thành công! Mật khẩu mới của bạn là: {newPassword}. (Hệ thống đang chạy giả lập nên không gửi email thật)" });
        }

        [HttpPut("UpdateProfile/{id}")]
        public async Task<IActionResult> UpdateProfile(int id, [FromBody] CustomerUpdateDTO input)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return NotFound(new { message = "Không tìm thấy khách hàng!" });

            customer.FullName = input.FullName;
            customer.Phone = input.Phone;
            customer.Address = input.Address;

            // Nếu người dùng có nhập mật khẩu mới thì tiến hành đổi
            if (!string.IsNullOrWhiteSpace(input.NewPassword))
            {
                customer.Password = BCrypt.Net.BCrypt.HashPassword(input.NewPassword);
            }

            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();

            return Ok(new { 
                message = "Cập nhật thông tin thành công!",
                customer = new { customer.Id, customer.FullName, customer.Email, customer.Phone, customer.Address }
            });
        }
    }

    public class CustomerRegisterDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
    }

    public class CustomerLoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class ForgotPasswordDTO
    {
        public string Email { get; set; }
    }

    public class CustomerUpdateDTO
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string NewPassword { get; set; }
    }
}
