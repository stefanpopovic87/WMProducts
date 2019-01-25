using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WMProducts.Models;
using System.Web.Script.Serialization;
using System.Web.Hosting;
using System.Text;
using WMProducts.ViewModel;

namespace WMProducts.Controllers
{
    //[Authorize]
    public class ProizvodiController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Proizvodi
        [AllowAnonymous]
        public ActionResult Index()
        {
            var proizvodi = db.Proizvodi
                .Include(p => p.Kategorija)
                .Include(p => p.Dobavljač)
                .Include(p => p.Proizvođač)
                .ToList();
            if (User.Identity.IsAuthenticated)
                return View(proizvodi);

            return View("IndexReadOnly");


        }

        [AllowAnonymous]
        public ActionResult GetData()
        {
            List<Proizvod> proizvodi = db.Proizvodi
                .Include(p => p.Kategorija)
                .Include(p => p.Dobavljač)
                .Include(p => p.Proizvođač)
                .ToList<Proizvod>();
            return Json(new { data = proizvodi }, JsonRequestBehavior.AllowGet);
        }


        // GET: Proizvodi/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var proizvod = db.Proizvodi.Include(p => p.Kategorija).Include(p => p.Proizvođač).Include(p => p.Dobavljač).SingleOrDefault(p => p.Id == id);
            if (proizvod == null)
            {
                return HttpNotFound();
            }
            return View(proizvod);
        }

        // GET: Proizvodi/Create
        public ActionResult Create()
        {
            var dobavljači = db.Dobavljači.ToList();
            var proizvođači = db.Proizvođači.ToList();
            var kategorije = db.Kategorije.ToList();

            var viewModel = new ProizvodViewModel
            {
                Dobavljači = dobavljači,
                Proizvođači = proizvođači,
                Kategorije = kategorije
            };
            return View("Create", viewModel);
        }

        // POST: Proizvodi/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Proizvod proizvod)
        {
            var karegorija = db.Kategorije.Find(proizvod.KategorijaId).Naziv;
            var proizvođač = db.Proizvođači.Find(proizvod.ProizvođačId).Naziv;
            var dobavljač = db.Dobavljači.Find(proizvod.DobavljačId).Naziv;
            bool isExist = db.Proizvodi.Where(
                p => p.Naziv.ToLower().Equals(proizvod.Naziv.ToLower()) &&
                p.Kategorija.Naziv.Equals(karegorija) &&
                p.Dobavljač.Naziv.Equals(dobavljač)
            ).FirstOrDefault() != null;

            if (!ModelState.IsValid)
            {
                var viewModel = new ProizvodViewModel(proizvod)
                {
                    Dobavljači = db.Dobavljači.ToList(),
                    Proizvođači = db.Proizvođači.ToList(),
                    Kategorije = db.Kategorije.ToList()
                };
                return View("Create", viewModel);
            }

            else if (isExist)
            {
                ModelState.AddModelError(string.Empty, "Proizvod već postoji");
                var viewModel = new ProizvodViewModel(proizvod)
                {
                    Dobavljači = db.Dobavljači.ToList(),
                    Proizvođači = db.Proizvođači.ToList(),
                    Kategorije = db.Kategorije.ToList()
                };
                return View("Create", viewModel);
            }

            else
            {

                db.Proizvodi.Add(proizvod);
                db.SaveChanges();
                SaveToJsonFile();
                return RedirectToAction("Index");
            }


        }

        // GET: Proizvodi/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proizvod proizvod = db.Proizvodi.Find(id);
            if (proizvod == null)
            {
                return HttpNotFound();
            }
            ViewBag.KategorijaId = new SelectList(db.Kategorije, "Id", "Naziv", proizvod.KategorijaId);
            ViewBag.ProizvođačId = new SelectList(db.Proizvođači, "Id", "Naziv", proizvod.ProizvođačId);
            ViewBag.DobavljačId = new SelectList(db.Dobavljači, "Id", "Naziv", proizvod.DobavljačId);

            return View(proizvod);
        }

        // POST: Proizvodi/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Naziv,Opis,Cena,DobavljačId,KategorijaId,ProizvođačId")] Proizvod proizvod)
        {
            var productInDb = db.Proizvodi
                .Include(p => p.Kategorija)
                .Include(p => p.Proizvođač)
                .Include(p => p.Dobavljač)
                .SingleOrDefault(p => p.Id == proizvod.Id);

            //Proizvod newProduct = proizvod;

            var karegorija = db.Kategorije.Find(proizvod.KategorijaId).Naziv;
            var proizvođač = db.Proizvođači.Find(proizvod.ProizvođačId).Naziv;
            var dobavljač = db.Dobavljači.Find(proizvod.DobavljačId).Naziv;
            bool isExist = db.Proizvodi.Where(
                p => p.Naziv.ToLower().Equals(proizvod.Naziv.ToLower()) &&
                p.Kategorija.Naziv.Equals(karegorija) &&
                p.Dobavljač.Naziv.Equals(dobavljač)
            ).FirstOrDefault() != null;

            if (!ModelState.IsValid)
            {
                ViewBag.KategorijaId = new SelectList(db.Kategorije, "Id", "Naziv", proizvod.KategorijaId);
                ViewBag.ProizvođačId = new SelectList(db.Proizvođači, "Id", "Naziv", proizvod.ProizvođačId);
                ViewBag.DobavljačId = new SelectList(db.Dobavljači, "Id", "Naziv", proizvod.DobavljačId);
                return View(proizvod);
            }
            else if (productInDb.Naziv == proizvod.Naziv
                && productInDb.Opis == proizvod.Opis
                && productInDb.Cena == proizvod.Cena
                && productInDb.Kategorija.Naziv == karegorija
                && productInDb.Dobavljač.Naziv == dobavljač
                && productInDb.Proizvođač.Naziv == proizvođač)
            {
                return RedirectToAction("Index");
            }
            else if (productInDb.Naziv == proizvod.Naziv
                 && productInDb.Kategorija.Naziv == karegorija
                  && productInDb.Dobavljač.Naziv == dobavljač)
            {
                productInDb.Opis = proizvod.Opis;
                productInDb.Cena = proizvod.Cena;
                productInDb.Proizvođač.Naziv = proizvođač;
                db.SaveChanges();
                SaveToJsonFile();
                return RedirectToAction("Index");
            }

            else if (isExist)
            {
                ModelState.AddModelError(string.Empty, "Proizvod već postoji");
                ViewBag.KategorijaId = new SelectList(db.Kategorije, "Id", "Naziv", proizvod.KategorijaId);
                ViewBag.ProizvođačId = new SelectList(db.Proizvođači, "Id", "Naziv", proizvod.ProizvođačId);
                ViewBag.DobavljačId = new SelectList(db.Dobavljači, "Id", "Naziv", proizvod.DobavljačId);
                return View(proizvod);
            }

            else
            {
                int newCategoryId = proizvod.KategorijaId;
                int newManufacturetId = proizvod.ProizvođačId;
                int newSupplierId = proizvod.DobavljačId;
                productInDb.Naziv = proizvod.Naziv;
                productInDb.Opis = proizvod.Opis;
                productInDb.Cena = proizvod.Cena;
                productInDb.KategorijaId = newCategoryId;
                productInDb.ProizvođačId = newManufacturetId;
                productInDb.DobavljačId = newSupplierId;
                db.SaveChanges();
                SaveToJsonFile();
                return RedirectToAction("Index");
            }

        }

        // GET: Proizvodi/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var proizvod = db.Proizvodi.Include(p => p.Kategorija).Include(p => p.Proizvođač).Include(p => p.Dobavljač).SingleOrDefault(p => p.Id == id);
            if (proizvod == null)
            {
                return HttpNotFound();
            }
            return View(proizvod);
        }

        // POST: Proizvodi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Proizvod proizvod = db.Proizvodi.Find(id);
            db.Proizvodi.Remove(proizvod);
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
