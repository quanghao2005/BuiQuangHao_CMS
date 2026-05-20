//Họ Tên :Bùi Quang Hào
//MSSV : 2123110043
//version : 1.1

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // Thêm thư viện này để dùng thẻ [Key]
using System.ComponentModel.DataAnnotations.Schema; // Thêm thư viện này để dùng thẻ [DatabaseGenerated]
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Entities
{
    public class Category
    {
        [Key] // Xác định Id là Khóa chính bắt buộc
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Ép SQL Server tự động tăng số (1, 2, 3...) từ gốc
        public int Id { get; set; }

        public string Name { get; set; } // Tên danh mục (vd: Tin Giáo Dục)
        public string Description { get; set; }

        // Quan hệ: Một danh mục có nhiều bài viết
        public virtual ICollection<Post> Posts { get; set; }
    }
}