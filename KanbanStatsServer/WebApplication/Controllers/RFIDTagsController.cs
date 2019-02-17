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
    public class RFIDTagsController : Controller
    {
        private RFIDKanbanEntities db = new RFIDKanbanEntities();

        // GET: RFIDTags
        public ActionResult Index()
        {
            return View(db.RFIDTag.ToList());
        }

        // GET: RFIDTags/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RFIDTag rFIDTag = db.RFIDTag.Find(id);
            if (rFIDTag == null)
            {
                return HttpNotFound();
            }
            return View(rFIDTag);
        }

        // GET: RFIDTags/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RFIDTags/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,RFIDUID,TagName,TagType")] RFIDTag rFIDTag)
        {
            if (ModelState.IsValid)
            {
                rFIDTag.ID = Guid.NewGuid();
                db.RFIDTag.Add(rFIDTag);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rFIDTag);
        }

        // GET: RFIDTags/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RFIDTag rFIDTag = db.RFIDTag.Find(id);
            if (rFIDTag == null)
            {
                return HttpNotFound();
            }
            return View(rFIDTag);
        }

        // POST: RFIDTags/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,RFIDUID,TagName,TagType")] RFIDTag rFIDTag)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rFIDTag).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rFIDTag);
        }

        // GET: RFIDTags/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RFIDTag rFIDTag = db.RFIDTag.Find(id);
            if (rFIDTag == null)
            {
                return HttpNotFound();
            }
            return View(rFIDTag);
        }

        // POST: RFIDTags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            RFIDTag rFIDTag = db.RFIDTag.Find(id);
            db.RFIDTag.Remove(rFIDTag);
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
