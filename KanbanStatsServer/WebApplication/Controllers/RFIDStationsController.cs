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
    public class RFIDStationsController : Controller
    {
        private RFIDKanbanEntities db = new RFIDKanbanEntities();

        // GET: RFIDStations
        public ActionResult Index()
        {
            return View(db.RFIDStations.ToList());
        }

        // GET: RFIDStations/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RFIDStations rFIDStations = db.RFIDStations.Find(id);
            if (rFIDStations == null)
            {
                return HttpNotFound();
            }
            return View(rFIDStations);
        }

        // GET: RFIDStations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RFIDStations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ParticipantName,HostName")] RFIDStations rFIDStations)
        {
            if (ModelState.IsValid)
            {
                rFIDStations.ID = Guid.NewGuid();
                db.RFIDStations.Add(rFIDStations);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rFIDStations);
        }

        // GET: RFIDStations/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RFIDStations rFIDStations = db.RFIDStations.Find(id);
            if (rFIDStations == null)
            {
                return HttpNotFound();
            }
            return View(rFIDStations);
        }

        // POST: RFIDStations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ParticipantName,HostName")] RFIDStations rFIDStations)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rFIDStations).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rFIDStations);
        }

        // GET: RFIDStations/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RFIDStations rFIDStations = db.RFIDStations.Find(id);
            if (rFIDStations == null)
            {
                return HttpNotFound();
            }
            return View(rFIDStations);
        }

        // POST: RFIDStations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            RFIDStations rFIDStations = db.RFIDStations.Find(id);
            db.RFIDStations.Remove(rFIDStations);
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
