using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace OnlineHelpDesk.Models
{
    [Table("Discussion")]
    public partial class Discussion
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
        public int TicketId { get; set; }
        public int AccountId { get; set; }

        public virtual Account Account { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}
