using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Models
{
    public class Author
    {
        [Key] 
        public Guid AuthorId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
        [Required]
        public string PhoneNumber { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public ICollection<Book>? Books { get; set; }
    }
}
