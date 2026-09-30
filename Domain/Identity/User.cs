using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public  class User
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string EmailAddress { get; set; }
        public required string ContactNumber { get; set; }
        public required string Address { get; set; }

        public  string UserName { get; set; } = string.Empty;
        public  string Password { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public bool Active { get; set; } = true;
    }
}
