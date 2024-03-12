using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Models
{
    public class Category
    {
        [Key]
        public Guid CategoryId { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        public DateTime LastModifiedDate { get; set; }
        [ForeignKey("CategoryId")]
        [InverseProperty("Categories")]
        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
