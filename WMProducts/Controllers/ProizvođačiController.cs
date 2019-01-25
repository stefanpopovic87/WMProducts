using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;
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
        public ActionResult Create([Bind(Include = "Id,Naziv,Adresa,Pib")] Proizvođač proizvođač)
        {
            bool isExist = db.Proizvođači.Where(
               p => p.Pib.ToLower().Equals(proizvođač.Pib.ToLower())
           ).FirstOrDefault() != null;

            if (!ModelState.IsValid)
            {
                return View(proizvođač);
            }
            else if (isExist)
            {
                ModelState.AddModelError(string.Empty, "Proizvođač sa unesenim PIB-om već postoji");
                return View(proizvođač);
            }
            else
            {
                db.Proizvođači.Add(proizvođač);
                db.SaveChanges();
                SaveToJsonFile();
                return RedirectToAction("Index");
            }
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
        public ActionResult Edit([Bind(Include = "Id,Naziv,Adresa,Pib")] Proizvođač proizvođač)
        {
            var manufacturerInDb = db.Proizvođači.FirstOrDefault(m => m.Id == proizvođač.Id);
            var newManufacturer = proizvođač;

            bool isExist = db.Proizvođači.Where(
              p => p.Pib.ToLower().Equals(proizvođač.Pib.ToLower())
          ).FirstOrDefault() != null;

            if (!ModelState.IsValid)
            {
                return View(proizvođač);
            }
            else if (manufacturerInDb.Naziv == newManufacturer.Naziv
                && manufacturerInDb.Adresa == newManufacturer.Adresa
                && manufacturerInDb.Pib == newManufacturer.Pib)
            {
                return RedirectToAction("Index");
            }
            else if (manufacturerInDb.Pib == newManufacturer.Pib)
            {
                manufacturerInDb.Naziv = proizvođač.Naziv;
                manufacturerInDb.Adresa = proizvođač.Adresa;
                db.SaveChanges();
                SaveToJsonFile();
                return RedirectToAction("Index");
            }
            else if (isExist)
            {
                ModelState.AddModelError(string.Empty, "Proizvođač sa unesenim PIB-om već postoji");
                return View(proizvođač);
            }
            else
            {
                db.Entry(proizvođač).State = EntityState.Modified;
                db.SaveChanges();
                SaveToJsonFile();
                return RedirectToAction("Index");
            }
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
            SaveToJsonFile();
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

        public void SaveToJsonFile()
        {
            List<Proizvod> proizvodi = db.Proizvodi
                    .Include(p => p.Kategorija)
                    .Include(p => p.Dobavljač)
                    .Include(p => p.Proizvođač)
                    .ToList();

            string JSONresult = JsonConvert.SerializeObject(proizvodi);
            string path = HostingEnvironment.MapPath("~/Data/proizvodi.json");
            System.IO.File.WriteAllText(path, JSONresult);

        }

    }
}
