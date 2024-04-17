using EF6_QueryTaker.Models.ManyToMany;
using EF6_QueryTaker.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace EF6_QueryTaker.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Query> Queries { get; set; }

        public DbSet<QueryCategory> QueryCategory { get; set; }

        public DbSet<QueryStatus> QueryStatus { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<CommentType> CommentTypes { get; set; }

        public DbSet<QueryComments> QueryComments { get; set; }
    }
}