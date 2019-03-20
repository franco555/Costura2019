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
    public class KardexesController : Controller
    {
        private ConexionContext db = new ConexionContext();

        // GET: Kardexes
        public ActionResult Index()
        {
            TempData["Valid"] = null;
            TempData["Error"] = null;

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

            var kardexes = db.Kardexes.
                Include(k => k.Company).
                Include(k => k.MeasuredUnit).
                Include(k => k.ProductClassification).
                Where(c => c.CompanyId == asing.CompanyId).
                OrderBy(o=>o.ProductClassification.Product.Name).
                ThenBy (o=>o.ProductClassification.Classification.Name).
                ToList();
            return View(kardexes);
        }

        // GET: Kardexes/Details/5
        public ActionResult Details(int? id)
        {
            TempData["Valid"] = null;
            TempData["Error"] = null;

            if (id == null)
            {
                TempData["Error"] = "No se envión ID...";
                return RedirectToAction("Index");
            }

            var kardex = db.Kardexes.Find(id);

            if (kardex == null)
            {
                TempData["Error"] = "No existe regitro con este ID...";
                return RedirectToAction("Index");
            }

            return View(kardex);
        }

        // GET: Kardexes/Create
        public ActionResult Create()
        {
            TempData["Valid"] = null;
            TempData["Error"] = null;

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

            var barcode = DateTime.Now; //$"{DateTime.Now.ToString('MMddyyyyhhmm')}"

            var kardex = new Kardex
            {
                CompanyId = asing.CompanyId,
                CodeBar= barcode.ToString("yyyyMMddhhmmss"),
            };
            
            ViewBag.MeasuredUnitId = new SelectList(CombosHelpers.GetMeasuredUnit(asing.CompanyId), "MeasuredUnitId", "Name");
            ViewBag.ProductClassificationId = new SelectList(CombosHelpers.GetProductClassification(asing.CompanyId), "ProductClassificationId", "ProductFull");

            return View(kardex);
        }

        // POST: Kardexes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Kardex kardex)
        {
            if (ModelState.IsValid)
            {
                db.Kardexes.Add(kardex);

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }

            ModelState.AddModelError(string.Empty, "Modelo no válido");
            

            ViewBag.MeasuredUnitId = new SelectList(CombosHelpers.GetMeasuredUnit(kardex.CompanyId), "MeasuredUnitId", "Name", kardex.MeasuredUnitId);
            ViewBag.ProductClassificationId = new SelectList(CombosHelpers.GetProductClassification(kardex.CompanyId), "ProductClassificationId", "ProductFull", kardex.ProductClassificationId);


            return View(kardex);
        }

        // GET: Kardexes/Edit/5
        public ActionResult Edit(int? id)
        {

            TempData["Valid"] = null;
            TempData["Error"] = null;

            if (id == null)
            {
                TempData["Error"] = "No se envión ID...";
                return RedirectToAction("Index");
            }
            var kardex = db.Kardexes.Find(id);
            if (kardex == null)
            {
                TempData["Error"] = "No existe regitro con este ID...";
                return RedirectToAction("Index");
            }

            ViewBag.MeasuredUnitId = new SelectList(CombosHelpers.GetMeasuredUnit(kardex.CompanyId), "MeasuredUnitId", "Name", kardex.MeasuredUnitId);
            ViewBag.ProductClassificationId = new SelectList(CombosHelpers.GetProductClassification(kardex.CompanyId), "ProductClassificationId", "ProductFull", kardex.ProductClassificationId);

            return View(kardex);
        }

        // POST: Kardexes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Kardex kardex)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kardex).State = EntityState.Modified;

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }

            ModelState.AddModelError(string.Empty, "Modelo no válido");

            ViewBag.MeasuredUnitId = new SelectList(CombosHelpers.GetMeasuredUnit(kardex.CompanyId), "MeasuredUnitId", "Name", kardex.MeasuredUnitId);
            ViewBag.ProductClassificationId = new SelectList(CombosHelpers.GetProductClassification(kardex.CompanyId), "ProductClassificationId", "ProductFull", kardex.ProductClassificationId);

            return View(kardex);
        }

        // GET: Kardexes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kardex kardex = db.Kardexes.Find(id);
            if (kardex == null)
            {
                return HttpNotFound();
            }
            return View(kardex);
        }

        // POST: Kardexes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Kardex kardex = db.Kardexes.Find(id);
            db.Kardexes.Remove(kardex);
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
