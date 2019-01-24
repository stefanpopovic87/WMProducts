using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WMProducts.Models;

namespace WMProducts.Controllers
{
    public class DobavljačiController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Dobavljači
        public ActionResult Index()
        {
            return View(db.Dobavljači.ToList());
        }

        // GET: Dobavljači/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dobavljač dobavljač = db.Dobavljači.Find(id);
            if (dobavljač == null)
            {
                return HttpNotFound();
            }
            return View(dobavljač);
        }

        // GET: Dobavljači/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dobavljači/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Naziv")] Dobavljač dobavljač)
        {
            if (ModelState.IsValid)
            {
                db.Dobavljači.Add(dobavljač);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dobavljač);
        }

        // GET: Dobavljači/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dobavljač dobavljač = db.Dobavljači.Find(id);
            if (dobavljač == null)
            {
                return HttpNotFound();
            }
            return View(dobavljač);
        }

        // POST: Dobavljači/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Naziv")] Dobavljač dobavljač)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dobavljač).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dobavljač);
        }

        // GET: Dobavljači/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dobavljač dobavljač = db.Dobavljači.Find(id);
            if (dobavljač == null)
            {
                return HttpNotFound();
            }
            return View(dobavljač);
        }

        // POST: Dobavljači/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Dobavljač dobavljač = db.Dobavljači.Find(id);
            db.Dobavljači.Remove(dobavljač);
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
