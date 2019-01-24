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
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products
                .Include(p => p.Category)
                .Include(p => p.Manufacturer)
                .Include(p => p.Supplier)
                .OrderBy(p => p.Category.Name)
                .ToList();
            return View(products);
        }

        public ActionResult GetData()
        {
            List<Product> productList = db.Products.ToList<Product>();
            return Json(new { data = productList }, JsonRequestBehavior.AllowGet);
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

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = db.Products.Include(p => p.Category).Include(p => p.Manufacturer).Include(p => p.Supplier).SingleOrDefault(p => p.Id == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            //ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            //ViewBag.ManufacturerId = new SelectList(db.Manufacturers, "Id", "Name");
            //ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name");
            var suppliers = db.Suppliers.ToList();
            var manufacturers = db.Manufacturers.ToList();
            var categories = db.Categories.ToList();

            var viewModel = new ProductViewModel
            {
                Suppliers = suppliers,
                Manufacturers = manufacturers,
                Categories = categories
            };
            return View("Create", viewModel);
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "Id,Name,Description,Price,SupplierId,CategoryId,ManufacturerId")]*/ Product product)
        {
            //ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            //ViewBag.ManufacturerId = new SelectList(db.Manufacturers, "Id", "Name", product.ManufacturerId);
            //ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", product.SupplierId);

            if (!ModelState.IsValid)
            {
                var viewModel = new ProductViewModel(product)
                {
                    Suppliers = db.Suppliers.ToList(),
                    Manufacturers = db.Manufacturers.ToList(),
                    Categories = db.Categories.ToList()
                };
                return View("Create", viewModel);
            }
            //if (product.Id == 0)
            //    db.Products.Add(product);
            else
            {
                //var prodictInDb = db.Products.Single(p => p.Id == product.Id);

                //prodictInDb.Name = product.Name;
                //prodictInDb.Description = product.Description;
                //prodictInDb.Price = product.Price;
                //prodictInDb.CategoryId = product.CategoryId;
                //prodictInDb.SupplierId = product.SupplierId;
                //prodictInDb.ManufacturerId = product.ManufacturerId;

                db.Products.Add(product);
                db.SaveChanges();
                string JSONresult = JsonConvert.SerializeObject(product);
                string path = HostingEnvironment.MapPath("~/Data/products.json");

                StringBuilder sb = new StringBuilder();
                JsonWriter jw = new JsonTextWriter(new StringWriter(sb));

                jw.Formatting = Formatting.Indented;
                jw.WriteStartObject();

                using (var tw = new StreamWriter(path, true))
                {
                    tw.WriteLine(JSONresult.ToString());
                    tw.Close();
                }
                return RedirectToAction("Index");
            }

           
        }




        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            ViewBag.ManufacturerId = new SelectList(db.Manufacturers, "Id", "Name", product.ManufacturerId);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", product.SupplierId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Price,SupplierId,CategoryId,ManufacturerId")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            ViewBag.ManufacturerId = new SelectList(db.Manufacturers, "Id", "Name", product.ManufacturerId);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", product.SupplierId);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = db.Products.Include(p => p.Category).Include(p => p.Manufacturer).Include(p => p.Supplier).SingleOrDefault(p => p.Id == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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

        private static string FormatJson(string json)
        {
            dynamic parsedJson = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
        }
    }
}
