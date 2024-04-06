using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using EF6_QueryTaker.Common;
using EF6_QueryTaker.Models;
using EF6_QueryTaker.Models.Common;
using EF6_QueryTaker.Models.Enums;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace EF6_QueryTaker.Controllers
{
    public class QueriesController : Controller
    {
        private ApplicationDbContext _dbContext;
        private ApplicationUserManager _userManager;

        //On Initialize, slow load
        //Consider filling collections async Task<>
        //Consider removing Initializer
        public QueriesController()
        {
            if(_dbContext == null)
            {
                _dbContext = new ApplicationDbContext();
                _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(_dbContext));             
            }
        }

        #region Properties
        public ObservableCollection<CommonProxy<string>> Engineers { get; set; }
        public ObservableCollection<CommonProxy<string>> Customers { get; set; }
        private bool IsInRoleEngineer => User.IsInRole(RolesEnum.Engineer.GetString());
        private bool IsInRoleAdmin => User.IsInRole(RolesEnum.Admin.GetString());
        private bool IsInRoleOperator => User.IsInRole(RolesEnum.Operator.GetString());
        private bool IsInRoleUser => User.IsInRole(RolesEnum.User.GetString());

        private ApplicationUser CurrentUser
        {
            get
            {
                var result = _userManager.FindById(User.Identity.GetUserId());
                return result;
            }
        }
 
        private IEnumerable<ApplicationUser> UsersList
        {
            get
            {
                var role = RolesEnum.User.GetEnum().ToString();
                var users = _userManager.Users.Where(x => x.Roles.Any(r => r.RoleId == role));
                return users;
            }
        }

        private IEnumerable<ApplicationUser> EngineersList
        {
            get
            {
                var role = RolesEnum.Engineer.GetEnum().ToString();
                var users = _userManager.Users.Where(x => x.Roles.Any(r => r.RoleId == role));
                return users;
            }
        }
        #endregion

        #region Private Methods
        private async Task FillUserCollections()
        {
            var users = await _userManager.Users.ToListAsync();


            var engineerRoleId = RolesEnum.Engineer.GetEnum().ToString();
            var customerRoleId = RolesEnum.User.GetEnum().ToString();

            var engineers = users.Where(x => x.Roles.Any(r => r.RoleId == engineerRoleId)).AsEnumerable().Select(x => new CommonProxy<string> { Name = x.UserName, Id = x.Id });
            var customers = users.Where(x => x.Roles.Any(r => r.RoleId == customerRoleId)).AsEnumerable().Select(x => new CommonProxy<string> { Name = x.UserName, Id = x.Id });
            
            Customers = new ObservableCollection<CommonProxy<string>>(customers);
            Customers.Insert(0, new CommonProxy<string>(string.Empty));

            Engineers = new ObservableCollection<CommonProxy<string>>(engineers);
            Engineers.Insert(0, new CommonProxy<string>(string.Empty));
        }
        #endregion

        // GET: Queries
        [Authorize]
        public async Task<ActionResult> Index()
        {
            if (Customers == null || Engineers == null)
            {
                await FillUserCollections();
            }

            if (CurrentUser == null)
            {
                return HttpNotFound("User Not Logged In.");
            }

            var roles = await _userManager.GetRolesAsync(CurrentUser.Id);

            if (!roles.Any())
            {
                return HttpNotFound("Bad User");
            }

            var queries = await _dbContext.Queries.ToListAsync();

            if (IsInRoleAdmin || IsInRoleOperator)
            {
                var queryable = queries.OrderBy(x => x.EngineerId).ToList();
                return View(queryable);
            }

            if (IsInRoleEngineer)
            {
                var queryable = queries.Where(x => x.EngineerId == CurrentUser.Id).ToList();
                return View(queryable);
            }

            if (IsInRoleUser)
            {
                var queryable = queries.Where(x => x.CustomerId == CurrentUser.Id).ToList();
                return View(queryable);
            }

            return HttpNotFound();
        }

        // GET: Queries/Details/5

        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var query = _dbContext.Queries.FindAsync(id);
            if (query == null)
            {
                return HttpNotFound();
            }
            return View(query);
        }

        // GET: Queries/Create
        public ActionResult Create()
        {
            if (IsInRoleAdmin)
            {
                var statuses = new ObservableCollection<CommonProxy<long>>(StaticCollections.QueryStatuses());
                ViewBag.Engineers = new SelectList(Engineers, "Id", "Name");
                ViewBag.Statuses = new SelectList(statuses, "Id", "Name");
                ViewBag.Customers = new SelectList(Customers, "Id", "Name");
            }

            return View();
        }

        // POST: Queries/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Query query)
        {
            if (query.Id != 0)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                if (IsInRoleAdmin)
                {
                    query.CustomerId = CurrentUser.Id;
                    query.EngineerId = null;
                    query.StatusId = (long)StatusEnums.ToBeProcessed;
                }

                query.DateAdded = DateTime.Now;
                query.DateUpdated = DateTime.Now;

                _dbContext.Queries.Add(query);
                await _dbContext.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        // GET: Queries/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var query = await _dbContext.Queries.FindAsync(id);

            if (query == null)
            {
                return HttpNotFound();
            }

            if (IsInRoleAdmin || IsInRoleEngineer)
            {
                var statuses = new ObservableCollection<CommonProxy<long>>(StaticCollections.QueryStatuses());
                ViewBag.Statuses = new SelectList(statuses, "Id", "Name", query.StatusId);
                ViewBag.Customers = new SelectList(Customers, "Id", "Name", query.CustomerId);

                if (!IsInRoleAdmin)
                {
                    return View(query);
                }

                ViewBag.Engineers = new SelectList(Engineers, "Id", "Name");
            }

            return View(query);
        }

        // POST: Queries/Edit/5
        /// <summary>
        /// query doesn't return selected values from dropdowns, UI problem probably
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Subject,Description,DateAdded,DateUpdated,StatusId,CustomerId,EngineerId")] Query query)
        {
            var temp = await _dbContext.Queries.FindAsync(query.Id);

            if (temp == null)
            {
                return HttpNotFound();
            }

            if (IsInRoleEngineer)
            {
                temp.StatusId = query.StatusId;
            }

            if (IsInRoleAdmin)
            {
                temp.StatusId = query.StatusId;
                temp.EngineerId = query.EngineerId;

                if (query.CustomerId != null)
                {
                    temp.CustomerId = query.CustomerId;
                }
            }

            temp.Description = query.Description;
            temp.Subject = query.Subject;
            temp.DateUpdated = DateTime.Now;

            _dbContext.Queries.AddOrUpdate(temp);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: Queries/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var query = await _dbContext.Queries.FindAsync(id);

            if (query == null)
            {
                return HttpNotFound();
            }

            return View(query);
        }

        // POST: Queries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async  Task<ActionResult> DeleteConfirmed(long id)
        {
            var query = await _dbContext.Queries.FindAsync(id);

            if (query == null)
            {
                return HttpNotFound();
            }

            _dbContext.Queries.Remove(query);
           await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if(_dbContext != null)
                {
                    _dbContext.Dispose();
                    _dbContext = null;

                    _userManager.Dispose();
                    _userManager = null;
                }                               
            }
            base.Dispose(disposing);
        }
    }
}
