using Entity.DTOs;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Query
{
    public class CheckCategory
    {
        public static bool IsSelected (BookDTO book,Category category)
        {
            if (book.BookId == Guid.Empty)
            {
                return false;
            }
            return book.CategoryIds.Contains(category.CategoryId);
        }
    }
}
