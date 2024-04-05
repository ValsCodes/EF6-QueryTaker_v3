using EF6_QueryTaker.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;
using EF6_QueryTaker.Models.Enums;
using System.Data.Entity;
using EF6_QueryTaker.Models.Proxies;
using System.Collections.Generic;
using System.Threading.Tasks;
using Castle.Core.Resource;

namespace EF6_QueryTaker.Controllers
{
    public class RolesController : Controller
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public RolesController()
        {
            if(_dbContext == null)
            {
                _dbContext = new ApplicationDbContext();
                _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_dbContext));
            }
        }

        // GET: Roles
        public ActionResult Index()
        {
            var usersWithRoles = _userManager.Users
                                   .Include(u => u.Roles)
                                   .ToList();

            var userProxies = usersWithRoles.Select(user => new UserProxy
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email,
                IsAdmin = user.Roles.Any(r => r.RoleId == RolesEnum.Admin.GetEnum().ToString()),
                IsEngineer = user.Roles.Any(r => r.RoleId == RolesEnum.Engineer.GetEnum().ToString()),
                IsUser = user.Roles.Any(r => r.RoleId == RolesEnum.User.GetEnum().ToString())
            }).ToList();

            return View(userProxies);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
                _userManager.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}