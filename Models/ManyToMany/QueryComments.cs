using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF6_QueryTaker.Models.ManyToMany
{
    public class QueryComments
    {
        [Key]
        [Column("query_id", Order = 1)]
        [ForeignKey(nameof(Query))]
        public virtual long QueryId { get; set; }
        [InverseProperty(nameof(Models.Query.QueryComments))]
        public virtual Query Query { get; set; }

        [Key]
        [Column("comment_id", Order = 2)]
        [ForeignKey(nameof(Comment))]
        public virtual long CommentId { get; set; }
        [InverseProperty(nameof(Models.Comment.QueryComments))]
        public virtual Comment Comment { get; set; }
    }
}