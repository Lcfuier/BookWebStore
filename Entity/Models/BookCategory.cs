using System.ComponentModel.DataAnnotations;

namespace Entity.Models
{
    public class BookCategory
    {
        [Key]
        [Required]
        public Guid BookId { get; set; }
        [Key]
        [Required]
        public Guid CategoryId { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public Book? Book { get; set; }
        public Category? Category { get; set; }

    }
}
