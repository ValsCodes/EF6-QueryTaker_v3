using EF6_QueryTaker.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Mvc;
using System.Data.Entity;
using EF6_QueryTaker.Models.Proxies;
using System.Linq;
using EF6_QueryTaker.Models.Enums;
using System.Net;
using System.Threading.Tasks;
using EF6_QueryTaker.Context;

namespace EF6_QueryTaker.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ApplicationUserManager _userManager;

        public UsersController()
        {
            if (_dbContext == null)
            {
                _dbContext = new ApplicationDbContext();
                _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(_dbContext));
            }
        }

        // GET User
        public async Task<ActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();

            var temp = users.Select(user => new UserProxy
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email,
                IsAdmin = user.Roles.Any(r => r.RoleId == RolesEnum.Admin.GetEnum().ToString()),
                IsEngineer = user.Roles.Any(r => r.RoleId == RolesEnum.Engineer.GetEnum().ToString()),
                IsUser = user.Roles.Any(r => r.RoleId == RolesEnum.Customer.GetEnum().ToString())
            }).ToList();

            return View(temp);
        }

        // GET Details
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET Create
        public ActionResult Create()
        {
            return View();
        }

        // POST Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET Edit
        public async Task<ActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = await _userManager.Users.Include(u => u.Roles).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return HttpNotFound();
            }

            var temp = new UserProxy
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email,
                IsAdmin = user.Roles.Any(r => r.RoleId == RolesEnum.Admin.GetEnum().ToString()),
                IsEngineer = user.Roles.Any(r => r.RoleId == RolesEnum.Engineer.GetEnum().ToString()),
                IsUser = user.Roles.Any(r => r.RoleId == RolesEnum.Customer.GetEnum().ToString())
            };

            return View(temp);
        }

        // POST Edit
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET Delete
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST Delete
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _userManager.Dispose();
                _dbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
