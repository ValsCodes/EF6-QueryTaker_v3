using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF6_QueryTaker.Models
{
    public class CommentType
    {
        [Key]
        [Column("id")]
        public virtual long Id { get; set; }

        [Column("name")]
        public virtual string Name { get; set; }
    }
}