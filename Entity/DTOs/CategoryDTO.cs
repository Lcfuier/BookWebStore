using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class CategoryDTO
    {
        public Guid CategoryId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập thể loại.")]
        [StringLength(80, ErrorMessage = "Thể Loại phải ngắn hơn 80 kí tự.")]
        public string Name { get; set; } = null!;
    }
}
