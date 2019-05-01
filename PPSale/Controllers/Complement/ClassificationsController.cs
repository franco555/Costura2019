using Newtonsoft.Json;
using PPSale.Classes;
using PPSale.Models.Complement;
using PPSale.Models.Conexion;
using PPSale.Models.View;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace PPSale.Controllers.Complement
{
    public class ClassificationsController : Controller
    {
        private ConexionContext db = new ConexionContext();
        private FunctionHelpers fn = new FunctionHelpers();

        // GET: Classifications
        public ActionResult Index()
        {
            var asing = db.AsingRolAndUsers.Where(a => a.Email == User.Identity.Name).FirstOrDefault();

            if (asing == null)
            {
                asing = new Models.Globals.AsingRolAndUser
                {
                    AsingRolAndUserId = 0,
                    CompanyId = 0,
                };

                TempData["Action"] = "Success";
                TempData["Message"] = fn.notRegistre;
            }

            var classifications = db.Classifications.
                Include(c => c.Company).
                Where(c => c.CompanyId == asing.CompanyId).
                OrderBy(c => c.Name).ToList();

            return View(classifications);
        }

        // GET: Classifications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["Action"] = "Success";
                TempData["Message"] = fn.notId;

                return RedirectToAction("Index");
            }

            var classification = db.Classifications.Find(id);

            if (classification == null)
            {
                TempData["Action"] = "Success";
                TempData["Message"] = fn.notExist;

                return RedirectToAction("Index");
            }

            //var product = new List<Product>();
            //var _productOn = db.ProductClassifications.Include(c => c.Product).Where(o => o.ClassificationId == classification.ClassificationId).ToList();
            //foreach (var item in _productOn) { product.Add(new Product() { ProductId = item.Product.ProductId, Name = item.Product.Name, }); }

            var _productOn = db.Products.Where(o => o.ProductClassifications.Select(i => i.ClassificationId).Contains(id.Value)).ToList();
            var _productOff = db.Products.Where(o => !o.ProductClassifications.Select(i => i.ClassificationId).Contains(id.Value)).ToList();

            // Obtiene todos los productos que aun no registrado en ProductClassifications
            /*
            var _productOff = (from prod in db.Products
                        where !(from clas in db.ProductClassifications
                                select clas.ProductId)
                               .Contains(prod.ProductId)
                        select prod).ToList();
            */

            var cl = new ClassificationWithProductsViewModel()
            {
                ClassificationId = classification.ClassificationId,
                Name = classification.Name,
                ProductOn = _productOn,
                ProductOff = _productOff,
            };

            return View(cl);
        }

        // GET: Classifications/Create
        public ActionResult Create()
        {
            var asing = db.AsingRolAndUsers.Where(a => a.Email == User.Identity.Name).FirstOrDefault();
            if (asing == null)
            {
                asing = new Models.Globals.AsingRolAndUser
                {
                    AsingRolAndUserId = 0,
                    CompanyId = 0,
                };

                TempData["Action"] = "Success";
                TempData["Message"] = fn.notRegistre;
            }


            var classification = new Classification
            {
                CompanyId = asing.CompanyId,
            };

            return PartialView(classification);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Classification classification)
        {
            if (ModelState.IsValid)
            {
                db.Classifications.Add(classification);

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    TempData["Action"] = "Success";
                    TempData["Message"] = fn.SuccessCreate;

                    return RedirectToAction("Index");
                }

                TempData["Action"] = "Error";
                TempData["Message"] = response.Message;
            }
            else
            {
                string messages = JsonConvert.SerializeObject(ModelState.Values.SelectMany(state => state.Errors).Select(error => error.ErrorMessage));

                TempData["Action"] = "Object";
                TempData["Message"] = messages;
            }

            return RedirectToAction("Index");
        }

        // GET: Classifications/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                TempData["Action"] = "Success";
                TempData["Message"] = fn.notId;

                return RedirectToAction("Index");
            }

            var classification = db.Classifications.Find(id);
            if (classification == null)
            {
                TempData["Action"] = "Success";
                TempData["Message"] = fn.notExist;

                return RedirectToAction("Index");
            }

            return PartialView(classification);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Classification classification)
        {
            if (ModelState.IsValid)
            {
                db.Entry(classification).State = EntityState.Modified;

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    TempData["Action"] = "Success";
                    TempData["Message"] = fn.SuccessCreate;

                    return RedirectToAction("Index");
                }

                TempData["Action"] = "Error";
                TempData["Message"] = response.Message;
            }
            else
            {
                string messages = JsonConvert.SerializeObject(ModelState.Values.SelectMany(state => state.Errors).Select(error => error.ErrorMessage));

                TempData["Action"] = "Object";
                TempData["Message"] = messages;
            }

            return RedirectToAction("Index");
        }

        // GET: Classifications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "No se envión ID...";
                return RedirectToAction("Index");
            }
            Classification classification = db.Classifications.Find(id);
            if (classification == null)
            {
                TempData["Error"] = "No existe regitro con este ID...";
                return RedirectToAction("Index");
            }
            return View(classification);
        }

        // POST: Classifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Classification classification = db.Classifications.Find(id);
            db.Classifications.Remove(classification);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        #region Add ProductClassification

        //Add  ProductClassification
        public ActionResult AddProdClass(int? idp, int? idc, int? idcom)
        {
            if (idp == null || idc == null || idcom == null)
            {
                TempData["Action"] = "Success";
                TempData["Message"] = fn.notId;

                return RedirectToAction($"Details/{idc}");
            }

            var asing = db.AsingRolAndUsers.Where(a => a.Email == User.Identity.Name).FirstOrDefault();

            if (asing == null)
            {
                asing = new Models.Globals.AsingRolAndUser
                {
                    AsingRolAndUserId = 0,
                    CompanyId = 0,
                };

                TempData["Action"] = "Error";
                TempData["Message"] = fn.notRegistre;

                return RedirectToAction($"Details/{idc}");
            }

            var productClass = new ProductClassificationAndKardex
            {
                CodeBar = DateTime.Now.ToString("yyyyMMddhhmmss"),
                CompanyId = idcom.Value,
                ClassificationId = idc.Value,
                ProductId = idp.Value,
            };

            ViewBag.UnitBaseId = new SelectList(CombosHelpers.GetUnitBase(), "UnitBaseId", "Name");

            return PartialView(productClass);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProdClass(ProductClassificationAndKardex pcKardex)
        {
            if (ModelState.IsValid)
            {
                var productClassification = new ProductClassification()
                {
                    CodeBar = pcKardex.CodeBar,
                    ClassificationId = pcKardex.ClassificationId,
                    ProductId = pcKardex.ProductId,
                    CompanyId = pcKardex.CompanyId,
                    UnitBaseId = pcKardex.UnitBaseId,

                };

                db.ProductClassifications.Add(productClassification);

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    var kardex = new Kardex()
                    {
                        Stock = pcKardex.Stock,
                        Price = pcKardex.Price,
                        ProductClassificationId = productClassification.ProductClassificationId,
                    };

                    db.Kardexes.Add(kardex);
                    var response2 = DBHelpers.SaveChage(db);

                    TempData["Action"] = "Success";
                    TempData["Message"] = fn.SuccessCreate;

                    return RedirectToAction($"details/{pcKardex.ClassificationId}");
                }

                TempData["Action"] = "Error";
                TempData["Message"] = response.Message;
            }
            else
            {
                string messages = JsonConvert.SerializeObject(ModelState.Values.SelectMany(state => state.Errors).Select(error => error.ErrorMessage));

                TempData["Action"] = "Object";
                TempData["Message"] = messages;
            }

            return RedirectToAction($"details/{pcKardex.ClassificationId}");
        }

        // Delete ProductClassification
        public ActionResult RestProdClass(int? idp, int? idc, int? idcom)
        {
            if (idp == null || idc == null || idcom == null)
            {
                TempData["Action"] = "Success";
                TempData["Message"] = fn.notId;

                return RedirectToAction($"Details/{idc}");
            }

            var productclassification =
                db.ProductClassifications.Where(o => o.ProductId == idp && o.ClassificationId == idc && o.CompanyId == idcom).FirstOrDefault();
            if (productclassification == null)
            {
                TempData["Action"] = "Error" +
                    "";
                TempData["Message"] = fn.notExist;

                return RedirectToAction($"Details/{idc}");
            }

            return PartialView(productclassification);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RestProdClass(ProductClassification pcKardex)
        {
            if (ModelState.IsValid)
            {
                var ok = db.ProductClassifications.Find(pcKardex.ProductClassificationId);

                if (ok==null)
                {
                    TempData["Action"] = "Error";
                    TempData["Message"] = fn.notId;

                    return RedirectToAction($"details/{pcKardex.ClassificationId}");
                }

                var kar = db.Kardexes.Where(k => k.ProductClassificationId == ok.ProductClassificationId).FirstOrDefault();

                db.Kardexes.Remove(kar);
                db.ProductClassifications.Remove(ok);

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    TempData["Action"] = "Success";
                    TempData["Message"] = fn.SuccessUpdate;

                    return RedirectToAction($"details/{pcKardex.ClassificationId}");
                }

                TempData["Action"] = "Error";
                TempData["Message"] = response.Message;
            }
            else
            {
                string messages = JsonConvert.SerializeObject(ModelState.Values.SelectMany(state => state.Errors).Select(error => error.ErrorMessage));

                TempData["Action"] = "Object";
                TempData["Message"] = messages;
            }

            return RedirectToAction($"details/{pcKardex.ClassificationId}");
        }

        #endregion

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
