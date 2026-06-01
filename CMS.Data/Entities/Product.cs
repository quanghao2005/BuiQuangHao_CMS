using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Entities
{
    [Table("Products")]
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(250)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Giá sản phẩm không được để trống")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public string? Description { get; set; }

        // --- ĐÃ BỔ SUNG: Khai báo ImageUrl cho phép nhận giá trị rỗng (string?) ---
        // Thuộc tính này bắt buộc phải có để gọt tỉa dữ liệu JSON trong ProductsController
        public string? ImageUrl { get; set; }

        [Required]
        public int CategoryProductId { get; set; } // Mã liên kết khóa ngoại

        [ForeignKey("CategoryProductId")]
        public virtual CategoryProduct? CategoryProduct { get; set; } // Đối tượng liên kết điều hướng
    }
}