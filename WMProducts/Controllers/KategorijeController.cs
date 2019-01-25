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
    public class KategorijeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Kategorije
        public ActionResult Index()
        {
            return View(db.Kategorije.ToList());
        }

        // GET: Kategorije/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategorija kategorija = db.Kategorije.Find(id);
            if (kategorija == null)
            {
                return HttpNotFound();
            }
            return View(kategorija);
        }

        // GET: Kategorije/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Kategorije/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Naziv")] Kategorija kategorija)
        {
            bool isExist = db.Kategorije.Where(
               k => k.Naziv.ToLower().Equals(kategorija.Naziv.ToLower())
           ).FirstOrDefault() != null;

            if (!ModelState.IsValid)
            {
                return View(kategorija);

            }
            else if (isExist)
            {
                ModelState.AddModelError(string.Empty, "Kategorija već postoji");
                return View(kategorija);
            }
            else
            {
                db.Kategorije.Add(kategorija);
                db.SaveChanges();
                SaveToJsonFile();
                return RedirectToAction("Index");
            }


        }

        // GET: Kategorije/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategorija kategorija = db.Kategorije.Find(id);
            if (kategorija == null)
            {
                return HttpNotFound();
            }
            return View(kategorija);
        }

        // POST: Kategorije/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Naziv")] Kategorija kategorija)
        {
            var categoryInDb = db.Kategorije.FirstOrDefault(k => k.Id == kategorija.Id);
            var newCategory = kategorija;

            bool isExist = db.Kategorije.Where(
               k => k.Naziv.ToLower().Equals(kategorija.Naziv.ToLower())
           ).FirstOrDefault() != null;

            if (!ModelState.IsValid)
            {
                return View(kategorija);

            }
            else if (newCategory.Naziv == categoryInDb.Naziv)
            {
                return RedirectToAction("Index");
            }
            else if (isExist)
            {
                ModelState.AddModelError(string.Empty, "Kategorija već postoji");
                return View(kategorija);
            }
            else
            {
                db.Entry(kategorija).State = EntityState.Modified;
                db.SaveChanges();
                SaveToJsonFile();
                return RedirectToAction("Index");
            }

        }

        // GET: Kategorije/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategorija kategorija = db.Kategorije.Find(id);
            if (kategorija == null)
            {
                return HttpNotFound();
            }
            return View(kategorija);
        }

        // POST: Kategorije/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Kategorija kategorija = db.Kategorije.Find(id);
            db.Kategorije.Remove(kategorija);
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
