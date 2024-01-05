using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class CartDTO
    {
        public Guid CartId { get; set; }

        public decimal Amount { get; set; }
        public DateTime LastModifiedDate { get; set; }=DateTime.Now;
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
