using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace OnlineHelpDesk.Models
{
    [Table("Account")]
    public partial class Account
    {
        public Account()
        {
            Discussions = new HashSet<Discussion>();
            TicketEmployees = new HashSet<Ticket>();
            TicketSupporters = new HashSet<Ticket>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public bool Status { get; set; }
        public string Email { get; set; }
        public int? Roleld { get; set; }
        public string Phone { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<Discussion> Discussions { get; set; }
        public virtual ICollection<Ticket> TicketEmployees { get; set; }
        public virtual ICollection<Ticket> TicketSupporters { get; set; }
    }
}
