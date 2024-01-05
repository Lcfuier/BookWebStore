using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class BookCategoryDTO
    {
        public Category category {  get; set; } 
        public IEnumerable<Book> books { get; set;} 
    }
}
