using EF6_QueryTaker.Models.ManyToMany;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF6_QueryTaker.Models
{
    public class Comment
    {
        [Key]
        [Column("id")]
        public virtual long Id { get; set; }

        [Column("date_added")]
        public virtual DateTime DateAdded { get; set; }

        [Column("date_updated")]
        public virtual DateTime DateUpdated { get; set; }

        [Column("desc")]
        public virtual string Description { get; set; }

        [Column("type_id")]
        [ForeignKey(nameof(CommentType))]
        public virtual long TypeId { get; set; }
        public virtual CommentType CommentType { get; set; }

        public virtual ICollection<QueryComments> QueryComments { get; } = new HashSet<QueryComments>();
    }
}