using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class OrderIndexViewModel
    {
        public OrderDTO OrderDto { get; set; } = new();
        public CartDTO CartDto { get; set; } = new();
        public CartItemDTO CartItemDto { get; set; } = new();
    }
}
