using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Models
{
    public class CartItem
    {
        [Key]
        public Guid CartItemID { get; set; }
        [Required]
        [Range(1, 1000)]
        public int Quantity { get; set; }
        [Column(TypeName = "Money")]
        public Decimal  Price { get; set; }
        public DateTime LastModifiedDate { get; set; } 

        public Guid BookId { get; set; }
        public Book? Book { get; set; }

        public Guid CartId { get; set; }
        public Cart? Cart { get; set; }
    }
}
