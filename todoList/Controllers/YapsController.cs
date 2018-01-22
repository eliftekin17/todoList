using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using todoList.Models;
using Microsoft.AspNet.Identity;

namespace todoList.Controllers
{
    public class YapsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Yaps
        public ActionResult Index()
        { 
            return View();
        }

        private IEnumerable<Yap> GetMyYaps()
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            return db.yapilacaklar.ToList().Where(x => x.User == currentUser);
        }
        public ActionResult BuildToDoTable()
        {
           
            return PartialView("_ToDoTable",GetMyYaps());
               
        }
        // GET: Yaps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yap yap = db.yapilacaklar.Find(id);
            if (yap == null)
            {
                return HttpNotFound();
            }
            return View(yap);
        }

        // GET: Yaps/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Yaps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description,IsDone")] Yap yap)
        {
            if (ModelState.IsValid)
            {
                string currentuserId = User.Identity.GetUserId();
                ApplicationUser correntUser = db.Users.FirstOrDefault(x => x.Id == currentuserId);
                yap.User = correntUser;
                db.yapilacaklar.Add(yap);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(yap);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AJAXCreate([Bind(Include = "Id,Description")] Yap yap)
        {
            if (ModelState.IsValid)
            {
                string currentuserId = User.Identity.GetUserId();
                ApplicationUser correntUser = db.Users.FirstOrDefault(x => x.Id == currentuserId);
                yap.User = correntUser;
                yap.IsDone = false;
                db.yapilacaklar.Add(yap);
                db.SaveChanges();
                
            }

            return PartialView("_ToDoTable", GetMyYaps());
        }


        // GET: Yaps/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yap yap = db.yapilacaklar.Find(id);
            if (yap == null)
            {
                return HttpNotFound();
            }
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            //kullanıcının sadece kendi listesini editleyebilmesinin kontrolü
            if(yap.User != currentUser)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(yap);
        }

        // POST: Yaps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,IsDone")] Yap yap)
        {
            if (ModelState.IsValid)
            {
                db.Entry(yap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(yap);
        }

        [HttpPost]

        public ActionResult AJAXEdit(int? id, bool value)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yap yap = db.yapilacaklar.Find(id);

            if(yap == null)
            {
                return HttpNotFound();
            }
            else
            {
                yap.IsDone = value;
                db.Entry(yap).State = EntityState.Modified;
                db.SaveChanges();
                return PartialView("_ToDoTable", GetMyYaps());
            }  
        }

        // GET: Yaps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yap yap = db.yapilacaklar.Find(id);
            if (yap == null)
            {
                return HttpNotFound();
            }
            return View(yap);
        }

        // POST: Yaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Yap yap = db.yapilacaklar.Find(id);
            db.yapilacaklar.Remove(yap);
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
