using System.ComponentModel.DataAnnotations;

namespace Entity.Models
{
    public class Publisher
    {
        [Key]
        public Guid PublisherId { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime LastModifiedDate { get; set; } 
        public ICollection<Book>? Books { get; set; }

    }
}
