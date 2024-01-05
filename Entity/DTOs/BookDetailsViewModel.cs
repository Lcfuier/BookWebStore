using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class BookDetailsViewModel
    {
        public CartItemDTO CartItemDTO { get; set; } = new();
        public Book Book { get; set; } = new();
    }
}
