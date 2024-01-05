using System.ComponentModel.DataAnnotations;

namespace Entity.Models
{
    public class Discount
    {
        [Key]
        public Guid DiscountId { get; set; }
        [Required]
        
        public decimal DiscountPercent { get; set; }
        [Required]
        public DateTime LastModifiedDate { get; set; }


        public ICollection<Book>? Books { get; set; }
    }
}
