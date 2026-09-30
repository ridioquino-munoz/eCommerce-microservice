using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Inventory
{
    public  class ProductInventory
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public Guid ProductID { get; set; }
        public int Quantity { get; set; }
        public int ReservedQuantity { get; set; }
        public DateTime DateLastUpdate { get; set; } = DateTime.UtcNow;
    }
}
