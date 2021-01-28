using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace OnlineHelpDesk.Models
{
    [Table("Status")]

    public partial class Status
    {
       

        public Status()
        {
            Tickets = new HashSet<Ticket>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool Display { get; set; }
        public string Color { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
