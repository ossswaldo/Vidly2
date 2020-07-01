using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Vidly2.Data;
using Vidly2.Models;
using System.Linq.Dynamic;


namespace Vidly2.Controllers
{
    public class KeysController : Controller
    {
      
        private LoginDataBaseEntities db = new LoginDataBaseEntities();
        

        public ActionResult Index(int page = 1, string sort = "Id", string sortdir = "desc", string search = "")
        {
            int pageSize = 10;
            int totalRecord = 0;
            if (page < 1) page = 1;
            int skip = (page * pageSize) - pageSize;
            var data = GetKeys(search, sort, sortdir, skip, pageSize);
            ViewBag.TotalRows = totalRecord;
            ViewBag.search = search;
            return View(data);
        }

        public ActionResult UserIndex(int page = 1, string sort = "Id", string sortdir = "desc", string search = "",string pidusername = "")
        {
            int pageSize = 10;
            int totalRecord = 0;
            if (page < 1) page = 1;
            int skip = (page * pageSize) - pageSize;
            var data = UserGetKeys(search, sort, sortdir, skip, pageSize, pidusername);
            ViewBag.TotalRows = totalRecord;
            ViewBag.search = search;
            return View(data);
        }


        public List<ViblyyKeyy> GetKeys(string search, string sort, string sortdir, int skip, int pageSize)
        {
            
            using (LoginDataBaseEntities dc = new LoginDataBaseEntities())
            {
                var v = (from a in dc.ViblyyKeyies
                         where
                                 
                                 a.Name.Contains(search) ||
                                 a.PIDName.Contains(search) ||
                                 a.Value.Contains(search) ||
                                 a.ExpirationDate.ToString().Contains(search) ||
                                 a.Application.Contains(search) ||
                                 a.Type.Contains(search) ||
                                 a.Environment.Contains(search) ||
                                 a.Comments.Contains(search) ||
                                 a.LastNotifiedDate.ToString().Contains(search)
                         select a
                                );
                int totalRecord = v.Count();
                v = v.OrderBy(sort + " " + sortdir);
                if (pageSize > 0)
                {
                    v = v.Skip(skip).Take(pageSize);
                }
                return v.ToList();
            }
        }

        public List<ViblyyKeyy> UserGetKeys(string search, string sort, string sortdir, int skip, int pageSize, string pidusername)
        {

            pidusername = (string)Session["PID"];
            using (LoginDataBaseEntities dc = new LoginDataBaseEntities())
            {
                var v = (from a in dc.ViblyyKeyies
                         where             
                                (a.Name.Contains(search) ||
                                 a.PIDName.Contains(search) ||
                                 a.Value.Contains(search) ||
                                 a.ExpirationDate.ToString().Contains(search) ||
                                 a.Application.Contains(search) ||
                                 a.Type.Contains(search) ||
                                 a.Environment.Contains(search) ||
                                 a.Comments.Contains(search) ||
                                 a.LastNotifiedDate.ToString().Contains(search)) &&
                                 a.PIDName == pidusername
                                 
                         select a
                                );
                int totalRecord = v.Count();
                v = v.OrderBy(sort + " " + sortdir);
                if (pageSize > 0)
                {
                    v = v.Skip(skip).Take(pageSize);
                }
                return v.ToList();
            }
        }

        // GET: Keys/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViblyyKeyy key = db.ViblyyKeyies.Find(id);
            if (key == null)
            {
                return HttpNotFound();
            }
            return View(key);
        }

        // GET: Keys/UserDetails/5
        public ActionResult UserDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViblyyKeyy key = db.ViblyyKeyies.Find(id);
            if (key == null)
            {
                return HttpNotFound();
            }
            return View(key);
        }

        // GET: Keys/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Value,ExpirationDate,Type,Environment,Comments,LastNotifiedDate,Application,PIDName")] ViblyyKeyy key)
        {
            if (ModelState.IsValid)
            {
                db.ViblyyKeyies.Add(key);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(key);
        }


        // GET: Keys/UserCreate
        public ActionResult UserCreate()
        {
            return View();
        }

        // POST: Keys/UsersCreate
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserCreate([Bind(Include = "Id,Name,Value,ExpirationDate,Type,Environment,Comments,LastNotifiedDate,Application,PIDName")] ViblyyKeyy key)
        {
            if (ModelState.IsValid)
            {
                db.ViblyyKeyies.Add(key);
                db.SaveChanges();
                return RedirectToAction("UserIndex");
            }

            return View(key);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViblyyKeyy key = db.ViblyyKeyies.Find(id);
            if (key == null)
            {
                return HttpNotFound();
            }
            return View(key);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Value,ExpirationDate,Type,Environment,Comments,LastNotifiedDate,Application,PIDName")] ViblyyKeyy key)
        {
            if (ModelState.IsValid)
            {
                db.Entry(key).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(key);
        }

        public ActionResult UserEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViblyyKeyy key = db.ViblyyKeyies.Find(id);
            if (key == null)
            {
                return HttpNotFound();
            }
            return View(key);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserEdit([Bind(Include = "Id,Name,Value,ExpirationDate,Type,Environment,Comments,LastNotifiedDate,Application,PIDName")] ViblyyKeyy key)
        {
            if (ModelState.IsValid)
            {
                db.Entry(key).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(key);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViblyyKeyy key = db.ViblyyKeyies.Find(id);
            if (key == null)
            {
                return HttpNotFound();
            }
            return View(key);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViblyyKeyy key = db.ViblyyKeyies.Find(id);
            db.ViblyyKeyies.Remove(key);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }






    }
}