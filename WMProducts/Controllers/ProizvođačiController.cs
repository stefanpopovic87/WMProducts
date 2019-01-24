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
    public class ProizvođačiController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Proizvođači
        public ActionResult Index()
        {
            return View(db.Proizvođači.ToList());
        }

        // GET: Proizvođači/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proizvođač proizvođač = db.Proizvođači.Find(id);
            if (proizvođač == null)
            {
                return HttpNotFound();
            }
            return View(proizvođač);
        }

        // GET: Proizvođači/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Proizvođači/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Naziv")] Proizvođač proizvođač)
        {
            if (ModelState.IsValid)
            {
                db.Proizvođači.Add(proizvođač);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(proizvođač);
        }

        // GET: Proizvođači/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proizvođač proizvođač = db.Proizvođači.Find(id);
            if (proizvođač == null)
            {
                return HttpNotFound();
            }
            return View(proizvođač);
        }

        // POST: Proizvođači/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Naziv")] Proizvođač proizvođač)
        {
            if (ModelState.IsValid)
            {
                db.Entry(proizvođač).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(proizvođač);
        }

        // GET: Proizvođači/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proizvođač proizvođač = db.Proizvođači.Find(id);
            if (proizvođač == null)
            {
                return HttpNotFound();
            }
            return View(proizvođač);
        }

        // POST: Proizvođači/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Proizvođač proizvođač = db.Proizvođači.Find(id);
            db.Proizvođači.Remove(proizvođač);
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
