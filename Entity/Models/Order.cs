using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entity.Models
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime OrderDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime ShippingDate { get; set; }

        [StringLength(128)]
        public string? Name { get; set; }

        [StringLength(15)]
        [Unicode(false)]
        public string PhoneNumber { get; set; } = null!;

        [Column(TypeName = "money")]
        public decimal TotalAmount { get; set; }

        [StringLength(128)]
        [Unicode(false)]
        public string? TrackingNumber { get; set; }
        [Unicode(false)]
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

        [StringLength(128)]
        public string? Address { get; set; }

        [StringLength(50)]
        public string? Ward { get; set; }

        [StringLength(30)]
        public string? District { get; set; }

        [StringLength(30)]
        public string? City { get; set; }

        public ICollection<OrderDetail>? OrderDetails { get; set; }

        [BindNever]
        public string customerId { get; set; }
        [ForeignKey(nameof(customerId))]
        [ValidateNever]
        public Customer? Customer { get; set; }

    }
}
