using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Models
{
    public class OrderDetail
    {
        [Key] 
        public Guid OrderDetailId { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "Money")]
        public Decimal Price { get; set; }
        public DateTime LastModifiedDate { get; set; } 

        public Guid BookId { get; set; } 
        public Book? Book { get; set; }

        public Guid OrderId { get; set; }
        public Order? Order { get; set; }
    }
}
