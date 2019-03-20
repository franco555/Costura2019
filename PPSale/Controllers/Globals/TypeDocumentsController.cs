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
    public class TypeDocumentsController : Controller
    {
        private ConexionContext db = new ConexionContext();

        // GET: TypeDocuments
        public ActionResult Index()
        {
            return View(db.TypeDocuments.ToList());
        }

        // GET: TypeDocuments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "No se envión ID...";
                return RedirectToAction("Index");
            }
            TypeDocument typeDocument = db.TypeDocuments.Find(id);
            if (typeDocument == null)
            {
                TempData["Error"] = "No existe regitro con este ID...";
                return RedirectToAction("Index");
            }
            return View(typeDocument);
        }

        // GET: TypeDocuments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TypeDocuments/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TypeDocument typeDocument)
        {
            if (ModelState.IsValid)
            {
                db.TypeDocuments.Add(typeDocument);

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }

            ModelState.AddModelError(string.Empty, "Modelo no válido");
            
            return View(typeDocument);
        }

        // GET: TypeDocuments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "No se envión ID...";
                return RedirectToAction("Index");
            }
            TypeDocument typeDocument = db.TypeDocuments.Find(id);
            if (typeDocument == null)
            {
                TempData["Error"] = "No existe regitro con este ID...";
                return RedirectToAction("Index");
            }
            return View(typeDocument);
        }

        // POST: TypeDocuments/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( TypeDocument typeDocument)
        {
            if (ModelState.IsValid)
            {
                db.Entry(typeDocument).State = EntityState.Modified;

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }

            ModelState.AddModelError(string.Empty, "Modelo no válido");

            return View(typeDocument);
        }

        // GET: TypeDocuments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "No se envión ID...";
                return RedirectToAction("Index");
            }
            TypeDocument typeDocument = db.TypeDocuments.Find(id);
            if (typeDocument == null)
            {
                TempData["Error"] = "No existe regitro con este ID...";
                return RedirectToAction("Index");
            }
            return View(typeDocument);
        }

        // POST: TypeDocuments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TypeDocument typeDocument = db.TypeDocuments.Find(id);
            db.TypeDocuments.Remove(typeDocument);
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
