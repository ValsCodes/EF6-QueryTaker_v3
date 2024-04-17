using EF6_QueryTaker.Context;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;
using EF6_QueryTaker.Models.Common;

namespace EF6_QueryTaker.Controllers
{
    public class RolesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public RolesController()
        {
            if(_dbContext == null)
            {
                _dbContext = new ApplicationDbContext();
            }
        }

        // GET Roles
        public async Task<ActionResult> Index()
        {
            var roles = await _dbContext.Roles.ToListAsync();
            var temp = roles.Select(x => new CommonProxy<string>() { Name = x.Name, Id = x.Id }).ToList();

            return View(temp);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();             
            }
            base.Dispose(disposing);
        }
    }
}