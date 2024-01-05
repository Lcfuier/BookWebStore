using Entity.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class BookDTO
    {
        public Guid BookId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tiêu đề.")]
        [StringLength(80, ErrorMessage = "Tiêu đề phải ngắn hơn 80 kí tự.")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập mô tả.")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập isbn13.")]
        [StringLength(13, ErrorMessage = "Vui lòng nhập Isbn13 hợp lệ.")]
        public string Isbn13 { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập hàng tồn kho.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Vui lòng nhập số nguyên lớn hơn 0 cho Hàng tồn kho.")]
        public int Inventory { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Vui lòng nhập số nguyên lớn hơn 0 cho Giá.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giảm giá.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$",
            ErrorMessage = "Vui lòng nhập số nguyên lớn hơn 0 cho Giảm giá.")]
        [Range(0.00, 1.0, ErrorMessage = "Nhập Giảm giá từ 0.00 đến 1.")]
        public decimal DiscountPercent { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số trang.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Vui lòng nhập số nguyên lớn hơn 0 cho Số trang.")]
        public int NumberOfPage { get; set; }

        [DateRange(ErrorMessage = "Ngày xuất bản phải từ ngày 1/1/1000 đến hiện tại.")]
        public DateTime PublicationDate { get; set; }

        public string? ImageUrl { get; set; }

        public Guid AuthorId { get; set; }

        public Guid PublisherId { get; set; }

        // for multiple checkbox
        [Required(ErrorMessage = "Vui lòng chọn thể loại.")]
        public Guid[] CategoryIds { get; set; } = null!;

        // those two are for display
        [Required(ErrorMessage = "Vui lòng nhập tác giả.")]
        public string AuthorName { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập nhà xuất bản.")]
        public string PublisherName { get; set; } = null!;
    }
}
