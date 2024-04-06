using EF6_QueryTaker.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;
using EF6_QueryTaker.Models.Enums;
using System.Data.Entity;
using EF6_QueryTaker.Models.Proxies;
using System.Net;
using System.Threading.Tasks;

namespace EF6_QueryTaker.Controllers
{
    public class RolesController : Controller
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly ApplicationUserManager _userManager;

        public RolesController()
        {
            if(_dbContext == null)
            {
                _dbContext = new ApplicationDbContext();
                _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(_dbContext));
            }
        }

        // GET: Roles
        public ActionResult Index()
        {

            return View();
        }

        // GET: Create Roles
        public ActionResult Create()
        {
            return View();
        }

        // GET: Edit Roles
        public ActionResult Edit(string id)
        {

            return View();
        }

        // GET: Details Roles
        public ActionResult Details(long? id)
        {
            return View();
        }

        // GET: Delete Roles
        public ActionResult Delete(long? id)
        {
            return View();
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