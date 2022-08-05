using System;

#nullable disable
namespace Fazilat.Models
{
    public class UserLimitation
    {
        public string UserId { get; set; }
        public DateTime Expiration { get; set; }
        public virtual int ExpirationYear { get; set; }
        public virtual int ExpirationMonth { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
