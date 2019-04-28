using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PPSale.Classes;
using PPSale.Models.Complement;
using PPSale.Models.Conexion;

namespace PPSale.Controllers.Complement
{
    public class ProductClassificationsController : Controller
    {
        private ConexionContext db = new ConexionContext();
        private FunctionHelpers fn = new FunctionHelpers();

        // GET: ProductClassifications
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

            var productClassifications = db.ProductClassifications.
                Include(p => p.Classification).
                Include(p => p.Product).
                Where(p=>p.CompanyId==asing.CompanyId).
                ToList();

            return View(productClassifications);
        }

        // GET: ProductClassifications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "No se envión ID...";
                return RedirectToAction("Index");
            }
            ProductClassification productClassification = db.ProductClassifications.Find(id);
            if (productClassification == null)
            {
                TempData["Error"] = "No existe regitro con este ID...";
                return RedirectToAction("Index");
            }
            return View(productClassification);
        }

        // GET: ProductClassifications/Create
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

                TempData["Error"] = "Necesita que el usuario este registado en alguna empresa, Éste usuario no tiene permiso de Modificar esta opción. Por favor comuniquese con el administrador";
                TempData["Valid"] = true;
            }

            ViewBag.ClassificationId = new SelectList(CombosHelpers.GetClassificationsAll(asing.CompanyId), "ClassificationId", "Name");
            ViewBag.ProductId = new SelectList(CombosHelpers.GetProducts(asing.CompanyId), "ProductId", "Name");

            var productClass = new ProductClassification {
                CompanyId=asing.CompanyId,
            };

            return View(productClass);
        }

        // POST: ProductClassifications/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductClassification productClassification)
        {
            

            if (ModelState.IsValid)
            {
                db.ProductClassifications.Add(productClassification);

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }

            ModelState.AddModelError(string.Empty, "Modelo no válido");

            var asing = db.AsingRolAndUsers.Where(a => a.Email == User.Identity.Name).FirstOrDefault();

            if (asing == null)
            {
                asing = new Models.Globals.AsingRolAndUser
                {
                    AsingRolAndUserId = 0,
                    CompanyId = 0,
                };

                TempData["Error"] = "Necesita que el usuario este registado en alguna empresa, Éste usuario no tiene permiso de Modificar esta opción. Por favor comuniquese con el administrador";
                TempData["Valid"] = true;
            }

            ViewBag.ClassificationId = new SelectList(CombosHelpers.GetClassificationsAll(asing.CompanyId), "ClassificationId", "Name", productClassification.ClassificationId);
            ViewBag.ProductId = new SelectList(CombosHelpers.GetProducts(asing.CompanyId), "ProductId", "Name", productClassification.ProductId);

            return View(productClassification);
        }

        // GET: ProductClassifications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "No se envión ID...";
                return RedirectToAction("Index");
            }
            ProductClassification productClassification = db.ProductClassifications.Find(id);
            if (productClassification == null)
            {
                TempData["Error"] = "No existe regitro con este ID...";
                return RedirectToAction("Index");
            }

            var asing = db.AsingRolAndUsers.Where(a => a.Email == User.Identity.Name).FirstOrDefault();

            if (asing == null)
            {
                asing = new Models.Globals.AsingRolAndUser
                {
                    AsingRolAndUserId = 0,
                    CompanyId = 0,
                };

                TempData["Error"] = "Necesita que el usuario este registado en alguna empresa, Éste usuario no tiene permiso de Modificar esta opción. Por favor comuniquese con el administrador";
                TempData["Valid"] = true;
            }

            ViewBag.ClassificationId = new SelectList(CombosHelpers.GetClassificationsAll(asing.CompanyId), "ClassificationId", "Name", productClassification.ClassificationId);
            ViewBag.ProductId = new SelectList(CombosHelpers.GetProducts(asing.CompanyId), "ProductId", "Name", productClassification.ProductId);
            return View(productClassification);
        }

        // POST: ProductClassifications/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductClassification productClassification)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productClassification).State = EntityState.Modified;

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }

            ModelState.AddModelError(string.Empty, "Modelo no válido");

            var asing = db.AsingRolAndUsers.Where(a => a.Email == User.Identity.Name).FirstOrDefault();

            if (asing == null)
            {
                asing = new Models.Globals.AsingRolAndUser
                {
                    AsingRolAndUserId = 0,
                    CompanyId = 0,
                };

                TempData["Error"] = "Necesita que el usuario este registado en alguna empresa, Éste usuario no tiene permiso de Modificar esta opción. Por favor comuniquese con el administrador";
                TempData["Valid"] = true;
            }

            ViewBag.ClassificationId = new SelectList(CombosHelpers.GetClassificationsAll(asing.CompanyId), "ClassificationId", "Name", productClassification.ClassificationId);
            ViewBag.ProductId = new SelectList(CombosHelpers.GetProducts(asing.CompanyId), "ProductId", "Name", productClassification.ProductId);
            return View(productClassification);
        }

        // GET: ProductClassifications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "No se envión ID...";
                return RedirectToAction("Index");
            }
            ProductClassification productClassification = db.ProductClassifications.Find(id);
            if (productClassification == null)
            {
                TempData["Error"] = "No existe regitro con este ID...";
                return RedirectToAction("Index");
            }
            return View(productClassification);
        }

        // POST: ProductClassifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductClassification productClassification = db.ProductClassifications.Find(id);
            db.ProductClassifications.Remove(productClassification);
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
