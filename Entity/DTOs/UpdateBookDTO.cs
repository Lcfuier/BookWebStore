using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class UpdateBookDTO
    {
        public BookDTO BookDto { get; set; } = new();
        public IEnumerable<Category> Categories { get; set; }  = new List<Category>();
        public IEnumerable<Author> author { get; set; } = new List<Author>();
        public IEnumerable<Publisher> publisher { get; set; } = new List<Publisher>();
    }
}
