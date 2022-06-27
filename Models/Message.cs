using System;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

#nullable disable
namespace Fazilat.Models
{
    public class Message
    {
        public string Id { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }
        public virtual ApplicationUser Sender { get; set; }
        public virtual ApplicationUser Receiver { get; set; }
    }
}
