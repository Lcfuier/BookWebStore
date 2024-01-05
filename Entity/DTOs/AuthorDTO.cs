using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class AuthorDTO
    {
        public Guid AuthorId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên của tác giả.")]
        [StringLength(60, ErrorMessage = "Tên của tác giả phải ngắn hơn 60 kí tự.")]
        public string FirstName { get; set; } 

        [Required(ErrorMessage = "Vui lòng nhập họ của tác giả.")]
        [StringLength(60, ErrorMessage = "Họ của tác giả phải ngắn hơn 60 kí tự.")]
        public string LastName { get; set; }

        public string? PhoneNumber { get; set; }
    }
}
