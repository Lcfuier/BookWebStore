using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class CartItemDTO
    {
        public Guid CartItemID { get; set; }

        public Guid CartId { get; set; }

        public Guid BookId { get; set; }
        public DateTime LastModifiedDate { get; set; }=DateTime.Now;
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Vui lòng nhập số nguyên lớn hơn 0 cho Số lượng.")]
        [Range(1, 1000, ErrorMessage = "Vui lòng nhập số nguyên lớn hơn 0 cho Số lượng.")]
        public int Quantity { get; set; }
    }
}
