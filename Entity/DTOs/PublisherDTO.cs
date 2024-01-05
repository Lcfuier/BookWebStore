using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class PublisherDTO
    {
        public Guid PublisherId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên của nhà xuất bản.")]
        [StringLength(80, ErrorMessage = "Tên của nhà xuất bản phải ngắn hơn 80 kí tự.")]
        public string Name { get; set; } = null!;
    }
}
