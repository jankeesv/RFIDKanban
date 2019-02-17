using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class RFIDRegistrationsController : Controller
    {
        private RFIDKanbanEntities db = new RFIDKanbanEntities();

        // GET: RFIDRegistrations
        public ActionResult Index()
        {
            return View(db.RFIDRegistrations.ToList());
        }

        // GET: RFIDRegistrations/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RFIDRegistrations rFIDRegistrations = db.RFIDRegistrations.Find(id);
            if (rFIDRegistrations == null)
            {
                return HttpNotFound();
            }
            return View(rFIDRegistrations);
        }

        // GET: RFIDRegistrations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RFIDRegistrations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,RegistrationDateTime,ExerciseName,TagName,TagType,HostName,ParticipantName")] RFIDRegistrations rFIDRegistrations)
        {
            if (ModelState.IsValid)
            {
                rFIDRegistrations.ID = Guid.NewGuid();
                db.RFIDRegistrations.Add(rFIDRegistrations);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rFIDRegistrations);
        }

        // GET: RFIDRegistrations/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RFIDRegistrations rFIDRegistrations = db.RFIDRegistrations.Find(id);
            if (rFIDRegistrations == null)
            {
                return HttpNotFound();
            }
            return View(rFIDRegistrations);
        }

        // POST: RFIDRegistrations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,RegistrationDateTime,ExerciseName,TagName,TagType,HostName,ParticipantName")] RFIDRegistrations rFIDRegistrations)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rFIDRegistrations).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rFIDRegistrations);
        }

        // GET: RFIDRegistrations/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RFIDRegistrations rFIDRegistrations = db.RFIDRegistrations.Find(id);
            if (rFIDRegistrations == null)
            {
                return HttpNotFound();
            }
            return View(rFIDRegistrations);
        }

        // POST: RFIDRegistrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            RFIDRegistrations rFIDRegistrations = db.RFIDRegistrations.Find(id);
            db.RFIDRegistrations.Remove(rFIDRegistrations);
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
