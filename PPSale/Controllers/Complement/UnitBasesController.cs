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
    public class UnitBasesController : Controller
    {
        private ConexionContext db = new ConexionContext();

        // GET: UnitBases
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

                TempData["Error"] = "Necesita que el usuario este registado en alguna empresa, Éste usuario no tiene permiso de Modificar esta opción. Por favor comuniquese con el administrador";
                TempData["Valid"] = true;
            }

            var unitBases = db.UnitBases.Include(u => u.Company)
                .Where(u=>u.CompanyId==asing.CompanyId).
                OrderBy(u=>u.Name).ToList();
            return View(unitBases);
        }

        // GET: UnitBases/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "No se envión ID...";
                return RedirectToAction("Index");
            }
            UnitBase unitBase = db.UnitBases.Find(id);
            if (unitBase == null)
            {
                TempData["Error"] = "No existe regitro con este ID...";
                return RedirectToAction("Index");
            }
            return View(unitBase);
        }

        // GET: UnitBases/Create
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


            var UnitBases = new UnitBase
            {
                CompanyId = asing.CompanyId,
            };

            return View(UnitBases);
        }

        // POST: UnitBases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( UnitBase unitBase)
        {
            if (ModelState.IsValid)
            {
                db.UnitBases.Add(unitBase);

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }
            

            return View(unitBase);
        }

        // GET: UnitBases/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "No se envión ID...";
                return RedirectToAction("Index");
            }
            UnitBase unitBase = db.UnitBases.Find(id);
            if (unitBase == null)
            {
                TempData["Error"] = "No existe regitro con este ID...";
                return RedirectToAction("Index");
            }

            return View(unitBase);
        }

        // POST: UnitBases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( UnitBase unitBase)
        {
            if (ModelState.IsValid)
            {
                db.Entry(unitBase).State = EntityState.Modified;

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }

            ModelState.AddModelError(string.Empty, "Modelo no válido");

            return View(unitBase);
        }

        // GET: UnitBases/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnitBase unitBase = db.UnitBases.Find(id);
            if (unitBase == null)
            {
                return HttpNotFound();
            }
            return View(unitBase);
        }

        // POST: UnitBases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UnitBase unitBase = db.UnitBases.Find(id);
            db.UnitBases.Remove(unitBase);
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
