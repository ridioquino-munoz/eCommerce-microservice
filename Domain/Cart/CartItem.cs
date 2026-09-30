using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Cart
{
    public class CartItem
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public Guid ProductID { get; set; }
        public double ProductPrice { get; set; }

        public int Quanitity { get; set; }

    }
}
