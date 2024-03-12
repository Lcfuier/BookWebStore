using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Models
{
    [Table("Book")]
    public class Book
    {
        [Key]
        public Guid BookId { get; set; }
        [MaxLength(80)]
        public string Title { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "varchar(13)")]
        public string Isbn13 { get; set; }
        public int Inventory { get; set; }
        [Column(TypeName ="Money")]
        public decimal Price { get; set; }
        [Required]
        public decimal DiscountPercent { get; set; }
        [NotMapped]
        public decimal PriceDiscount => Price-Price*DiscountPercent;

        public int NumberOfPage { get; set; }
        public DateTime PublicationDate { get; set; }
        public string ImageURL { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public Guid authorId { get; set; }
        public Author? Author { get; set; }

        public Guid publisherID { get; set; }
        public Publisher? Publisher { get; set; }



        public ICollection<OrderDetail>? OrderDetails { get; set; }
        [ForeignKey("BookId")]
        [InverseProperty("Books")]
        public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<CartItem>? CartItems { get; set; }


    }
}
