using System;

#nullable disable
namespace Fazilat.Models
{
    public class UserInformation
    {
        public string UserId { get; set; }
        public string NationalCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public byte[] BirthCertificate { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
