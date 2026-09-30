using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Identity
{
 // TABLE WAS DESIGN FOR MULTIPLE DEVICE LOGIN SESSION BUT FOR NOW ONLY SUPPORT ONE LOGIN TOKEN
    public class TokenRefresh
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public Guid UserID { get; set; }

        public string Token { get; set; } = string.Empty;

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime ExpiryDate { get; set; }
        // public DateTime RevokedDate { get; set; } FOR MORE THAN ONE REFRESHTOKEN
    }
}
