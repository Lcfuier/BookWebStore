using Entity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class OrderDTO
    {
        public Guid OrderId { get; set; }

        public string CustomerId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên người nhận.")]
        [StringLength(128)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
        [StringLength(15)]
        [Unicode(false)]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ.")]
        [StringLength(50)]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập tên phường.")]
        [StringLength(50)]
        public string Ward { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập tên quận.")]
        [StringLength(30)]
        public string District { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập tên thành phố.")]
        [StringLength(30)]
        public string City { get; set; } = null!;
        [Column(TypeName = "datetime")]
        [Required(ErrorMessage = "Vui lòng nhập Ngày đặt hàng")]
        public DateTime OrderDate { get; set; }
        [Column(TypeName = "datetime")]
        [Required(ErrorMessage = "Vui lòng nhập Ngày giao hàng")]
        public DateTime ShippingDate { get; set; }
        [StringLength(128)]
        [Unicode(false)]
        public string? TrackingNumber { get; set; }
        [Unicode(false)]
        [Column(TypeName = "money")]
        public decimal TotalAmount { get; set; }
        public string? PaymentIntentId { get; set; }
        [Unicode(false)]
        public string? SessionId { get; set; }

        [StringLength(128)]
        [Unicode(false)]
        public string? Carrier { get; set; }

        [StringLength(20)]
        [Unicode(false)]
        public string? OrderStatus { get; set; }

        [StringLength(20)]
        [Unicode(false)]
        public string? PaymentStatus { get; set; }

        [StringLength(256)]
        [Unicode(false)]
        public string? TransactionId { get; set; }
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public Customer? Customer { get; set; }
    }
}
