﻿using EF6_QueryTaker.Models.ManyToMany;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF6_QueryTaker.Models
{
    public class Ticket
    {
        [Key]
        [Column("id")]
        public virtual long Id { get; set; }

        [Column("subject")]
        public virtual string Subject { get; set; }

        [Column("desc")]
        public virtual string Description { get; set; }

        [Column("date_added")]
        public virtual DateTime DateAdded { get; set; }

        [Column("date_last_update")]
        public virtual DateTime DateUpdated { get; set; }

        [Column("status_id")]
        [ForeignKey(nameof(QueryStatus))]
        public virtual long StatusId { get; set; }
        public virtual TicketStatus QueryStatus { get; set; }

        [Column("user_id")]
        [ForeignKey(nameof(Customer))]
        public virtual string CustomerId { get; set; }
        public virtual ApplicationUser Customer { get; set; }

        [Column("engineer_id")]
        [ForeignKey(nameof(Engineer))]
        public virtual string EngineerId { get; set; }
        public virtual ApplicationUser Engineer { get; set; }

        public virtual ICollection<TicketComments> QueryComments { get; } = new HashSet<TicketComments>();
    }
}