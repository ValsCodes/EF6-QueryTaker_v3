using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using EF6_QueryTaker.Models.ManyToMany;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EF6_QueryTaker.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

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