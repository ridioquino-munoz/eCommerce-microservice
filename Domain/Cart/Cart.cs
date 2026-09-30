using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Cart
{
    public class Cart
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public Guid UserID { get; set; }
        public List<CartItem> Items { get; set; } = new();
        public DateTime DateCreated { get; set; }

    }
}
