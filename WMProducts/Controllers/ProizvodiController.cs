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
    public class ProizvodiController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Proizvodi
        public ActionResult Index()
        {
            var proizvodi = db.Proizvodi
                .Include(p => p.Kategorija)
                .Include(p => p.Dobavljač)
                .Include(p => p.Proizvođač)
                .ToList();
            return View(proizvodi);
        }

        public ActionResult GetData()
        {
            List<Proizvod> proizvodi = db.Proizvodi
                .Include(p => p.Kategorija)
                .Include(p => p.Dobavljač)
                .Include(p => p.Proizvođač)
                .ToList<Proizvod>();
            return Json(new { data = proizvodi }, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public ActionResult Upload(HttpPostedFileBase jsonFile)
        //{
        //    if (!jsonFile.FileName.EndsWith(".json"))
        //    {
        //        ViewBag.Error = "Invalid file type(Only JSON file allowed)";
        //    }
        //    else
        //    {
        //        jsonFile.SaveAs(Server.MapPath("~/FileUpload/" + Path.GetFileName(jsonFile.FileName)));
        //        StreamReader streamReader = new StreamReader(Server.MapPath("~/FileUpload/" + Path.GetFileName(jsonFile.FileName)));
        //        string data = streamReader.ReadToEnd();
        //        List<Product> products = JsonConvert.DeserializeObject<List<Product>>(data);

        //        products.ForEach(p =>
        //        {
        //            Product product = new Product()
        //            {
        //                Name = p.Name,
        //                Price = p.Price,
        //                Description = p.Description
        //            };
        //            db.Products.Add(product);
        //            db.SaveChanges();
        //        });
        //        ViewBag.Success = "File uploaded Successfully..";
        //    }
        //    return View("Index");
        //}

        //public ActionResult GetAll()
        //{
        //    db.Configuration.ProxyCreationEnabled = false;
        //    var productsList = db.Products.ToList<Product>();
        //    return Json(new { data = productsList }, JsonRequestBehavior.AllowGet);
        //}

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
            //ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            //ViewBag.ManufacturerId = new SelectList(db.Manufacturers, "Id", "Name");
            //ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name");
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
        public ActionResult Create(/*[Bind(Include = "Id,Name,Description,Price,SupplierId,CategoryId,ManufacturerId")]*/ Proizvod proizvod)
        {
            //ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            //ViewBag.ManufacturerId = new SelectList(db.Manufacturers, "Id", "Name", product.ManufacturerId);
            //ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", product.SupplierId);
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
            if (ModelState.IsValid)
            {
                db.Entry(proizvod).State = EntityState.Modified;
                db.SaveChanges();
                SaveToJsonFile();
                return RedirectToAction("Index");
            }
            ViewBag.KategorijaId = new SelectList(db.Kategorije, "Id", "Naziv", proizvod.KategorijaId);
            ViewBag.ProizvođačId = new SelectList(db.Proizvođači, "Id", "Naziv", proizvod.ProizvođačId);
            ViewBag.DobavljačId = new SelectList(db.Dobavljači, "Id", "Naziv", proizvod.DobavljačId);
            return View(proizvod);
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
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
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
