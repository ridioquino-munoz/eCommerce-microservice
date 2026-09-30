using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Product
{
    public class Product
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public required string Name { get; set; }
        public double Price { get; set; } = 0.0;

        public Guid CreatedByUser { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    }
}
