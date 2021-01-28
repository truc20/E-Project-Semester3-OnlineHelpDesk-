using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace OnlineHelpDesk.Models
{
    [Table("Ticket")]
    public partial class Ticket
    {
        public Ticket()
        {
            Discussions = new HashSet<Discussion>();
            Photos = new HashSet<Photo>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? StatusId { get; set; }
        public int? CategoryId { get; set; }
        public int? PeriodId { get; set; }
        public int? EmployeeId { get; set; }
        public int? SupporterId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Account Employee { get; set; }
        public virtual Period Period { get; set; }
        public virtual Status Status { get; set; }
        public virtual Account Supporter { get; set; }
        public virtual ICollection<Discussion> Discussions { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
    }
}
