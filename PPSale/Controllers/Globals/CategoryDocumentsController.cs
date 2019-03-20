using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PPSale.Classes;
using PPSale.Models.Conexion;
using PPSale.Models.Globals;

namespace PPSale.Controllers.Globals
{
    public class CategoryDocumentsController : Controller
    {
        private ConexionContext db = new ConexionContext();

        // GET: CategoryDocuments
        public ActionResult Index()
        {
            return View(db.CategoryDocuments.ToList());
        }

        // GET: CategoryDocuments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "No se envión ID...";
                return RedirectToAction("Index");
            }
            CategoryDocument categoryDocument = db.CategoryDocuments.Find(id);
            if (categoryDocument == null)
            {
                TempData["Error"] = "No existe regitro con este ID...";
                return RedirectToAction("Index");
            }
            return View(categoryDocument);
        }

        // GET: CategoryDocuments/Create
        public ActionResult Create()
        {
            ViewBag.TypeDocumentId = new SelectList(CombosHelpers.GetTypeDocuments(), "TypeDocumentId", "Name");
            return View();
        }

        // POST: CategoryDocuments/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryDocument categoryDocument)
        {
            if (ModelState.IsValid)
            {
                db.CategoryDocuments.Add(categoryDocument);

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }

            ModelState.AddModelError(string.Empty, "Modelo no válido");

            ViewBag.TypeDocumentId = new SelectList(CombosHelpers.GetTypeDocuments(), "TypeDocumentId", "Name", categoryDocument.TypeDocumentId);

            return View(categoryDocument);
        }

        // GET: CategoryDocuments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "No se envión ID...";
                return RedirectToAction("Index");
            }
            CategoryDocument categoryDocument = db.CategoryDocuments.Find(id);
            if (categoryDocument == null)
            {
                TempData["Error"] = "No existe regitro con este ID...";
                return RedirectToAction("Index");
            }

            ViewBag.TypeDocumentId = new SelectList(CombosHelpers.GetTypeDocuments(), "TypeDocumentId", "Name", categoryDocument.TypeDocumentId);

            return View(categoryDocument);
        }

        // POST: CategoryDocuments/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryDocument categoryDocument)
        {
            if (ModelState.IsValid)
            {
                db.Entry(categoryDocument).State = EntityState.Modified;

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }

            ModelState.AddModelError(string.Empty, "Modelo no válido");

            ViewBag.TypeDocumentId = new SelectList(CombosHelpers.GetTypeDocuments(), "TypeDocumentId", "Name", categoryDocument.TypeDocumentId);

            return View(categoryDocument);
        }

        // GET: CategoryDocuments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "No se envión ID...";
                return RedirectToAction("Index");
            }
            CategoryDocument categoryDocument = db.CategoryDocuments.Find(id);
            if (categoryDocument == null)
            {
                TempData["Error"] = "No existe regitro con este ID...";
                return RedirectToAction("Index");
            }
            return View(categoryDocument);
        }

        // POST: CategoryDocuments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CategoryDocument categoryDocument = db.CategoryDocuments.Find(id);
            db.CategoryDocuments.Remove(categoryDocument);
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
