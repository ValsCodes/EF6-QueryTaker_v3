using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EF6_QueryTaker.Models;
using EF6_QueryTaker.Models.Common;
using EF6_QueryTaker.Models.Enums;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace EF6_QueryTaker.Controllers
{
    public class QueriesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public QueriesController()
        {
            _dbContext = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_dbContext));

            Statuses = new ObservableCollection<CommonProxy<long>>(QueryStatuses);
            Statuses.Insert(0, new CommonProxy<long>(string.Empty));

            Customers = new ObservableCollection<CommonProxy<string>>(UsersList.Select(x => new CommonProxy<string> { Name = x.UserName, Id = x.Id }));
            Customers.Insert(0, new CommonProxy<string>(string.Empty));

            Engineers = new ObservableCollection<CommonProxy<string>>(EngineersList.Select(x => new CommonProxy<string> { Name = x.UserName, Id = x.Id }));
            Engineers.Insert(0, new CommonProxy<string>(string.Empty));
        }

        #region Properties
        public ObservableCollection<CommonProxy<string>> Engineers { get; set; }
        public ObservableCollection<CommonProxy<string>> Customers { get; set; }
        public ObservableCollection<CommonProxy<long>> Statuses { get; set; }
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

        private IEnumerable<CommonProxy<long>> QueryStatuses
        {
            get
            {
                var statuses = _dbContext.QueryStatus
                    .Select(x => new { x.Name, x.Id })
                    .AsEnumerable()
                    .Select(x => new CommonProxy<long> { Name = x.Name, Id = x.Id });
                return statuses;
            }
        }

        #endregion

        // GET: Queries
        [Authorize]
        public ActionResult Index()
        {
            if (CurrentUser == null)
            {
                return HttpNotFound("User Not Logged In.");
            }

            var roles = _userManager.GetRoles(CurrentUser.Id);

            if (!roles.Any())
            {
                return HttpNotFound("Bad User");
            }

            var queries = _dbContext.Queries;

            if (IsInRoleAdmin || IsInRoleOperator)
            {
                return View(queries.OrderBy(x => x.EngineerId).ToList());
            }

            if (IsInRoleEngineer)
            {
                var queryable = queries.Where(x => x.EngineerId == CurrentUser.Id);
                return View(queryable.ToList());
            }

            if (IsInRoleUser)
            {
                var queryable = queries.Where(x => x.CustomerId == CurrentUser.Id);
                return View(queryable.ToList());
            }

            return HttpNotFound();
        }

        // GET: Queries/Details/5

        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var query = _dbContext.Queries.Find(id);
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
                ViewBag.Engineers = new SelectList(Engineers, "Id", "Name");
                ViewBag.Statuses = new SelectList(Statuses, "Id", "Name");
                ViewBag.Customers = new SelectList(Customers, "Id", "Name");
            }

            return View();
        }

        // POST: Queries/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Query query)
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
                _dbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // GET: Queries/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var query = _dbContext.Queries.Find(id);

            if (query == null)
            {
                return HttpNotFound();
            }

            if (IsInRoleAdmin || IsInRoleEngineer)
            {
                ViewBag.Statuses = new SelectList(Statuses, "Id", "Name", query.StatusId);
                ViewBag.Customers = new SelectList(Customers, "Id", "Name", query.CustomerId);

                if (!IsInRoleAdmin)
                {
                    return View(query);
                }

                ViewBag.Engineers = new SelectList(Engineers, "Id", "Name");

            }
/*            else
            {
                ViewBag.EngineerId = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.StatusId = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.UserId = new SelectList(Enumerable.Empty<SelectListItem>());
            }*/

            return View(query);
        }

        // POST: Queries/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Subject,Description,DateAdded,DateUpdated,StatusId,CustomerId,EngineerId")] Query query)
        {
            var updatedQuery = _dbContext.Queries.Find(query.Id);

            if (updatedQuery == null)
            {
                return HttpNotFound();
            }

            if (IsInRoleEngineer)
            {
                updatedQuery.StatusId = query.StatusId;
            }

            if (IsInRoleAdmin)
            {
                updatedQuery.StatusId = query.StatusId;
                updatedQuery.EngineerId = query.EngineerId;
                updatedQuery.CustomerId = query.CustomerId;             
            }

            updatedQuery.Description = query.Description;
            updatedQuery.Subject = query.Subject;
            updatedQuery.DateUpdated = DateTime.Now;

            _dbContext.Queries.AddOrUpdate(updatedQuery);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Queries/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var query = _dbContext.Queries.Find(id);

            if (query == null)
            {
                return HttpNotFound();
            }

            return View(query);
        }

        // POST: Queries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            var query = _dbContext.Queries.Find(id);

            if (query == null)
            {
                return HttpNotFound();
            }

            _dbContext.Queries.Remove(query);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
                _userManager.Dispose();

                Engineers.Clear();
                Statuses.Clear();
                Customers.Clear();
            }
            base.Dispose(disposing);
        }
    }
}
