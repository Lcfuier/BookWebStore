using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Models
{
    public class Cart
    {
        public Guid CartId { get; set; }
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }
        public DateTime LastModifiedDate { get; set; } 

        public ICollection<Cart>? Carts { get; set; }
        [BindNever]
        public string customerId { get; set; }
        [ForeignKey(nameof(customerId))]
        [ValidateNever]
        public Customer Customer { get; set; }
        public ICollection<CartItem>? CartItems { get; set; }


    }
}
