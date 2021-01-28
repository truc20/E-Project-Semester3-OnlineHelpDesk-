using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace OnlineHelpDesk.Models
{
    [Table("Photo")]
    public partial class Photo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TicketId { get; set; }

        public virtual Ticket Ticket { get; set; }
    }
}
