using PPSale.Classes;
using PPSale.Models.Conexion;
using PPSale.Models.Globals;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace PPSale.Controllers.Globals
{
    [Authorize(Roles = "Admin")]

    public class IvaConditionsController : Controller
    {
        private ConexionContext db = new ConexionContext();

        // GET: IvaConditions
        public ActionResult Index()
        {
            return View(db.IvaConditions.ToList());
        }

        // GET: IvaConditions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "No se envión ID...";
                return RedirectToAction("Index");
            }
            IvaCondition ivaCondition = db.IvaConditions.Find(id);
            if (ivaCondition == null)
            {
                TempData["Error"] = "No existe regitro con este ID...";
                return RedirectToAction("Index");
            }
            return View(ivaCondition);
        }

        // GET: IvaConditions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: IvaConditions/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IvaCondition ivaCondition)
        {
            if (ModelState.IsValid)
            {
                db.IvaConditions.Add(ivaCondition);

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Modelo no válido");
            }

            return View(ivaCondition);
        }

        // GET: IvaConditions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "No se envión ID...";
                return RedirectToAction("Index");
            }
            IvaCondition ivaCondition = db.IvaConditions.Find(id);
            if (ivaCondition == null)
            {
                TempData["Error"] = "No existe regitro con este ID...";
                return RedirectToAction("Index");
            }
            return View(ivaCondition);
        }

        // POST: IvaConditions/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IvaCondition ivaCondition)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ivaCondition).State = EntityState.Modified;

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Modelo no válido");
            }
            return View(ivaCondition);
        }

        // GET: IvaConditions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "No se envión ID...";
                return RedirectToAction("Index");
            }
            IvaCondition ivaCondition = db.IvaConditions.Find(id);
            if (ivaCondition == null)
            {
                TempData["Error"] = "No existe regitro con este ID...";
                return RedirectToAction("Index");
            }
            return View(ivaCondition);
        }

        // POST: IvaConditions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IvaCondition ivaCondition = db.IvaConditions.Find(id);
            db.IvaConditions.Remove(ivaCondition);
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
