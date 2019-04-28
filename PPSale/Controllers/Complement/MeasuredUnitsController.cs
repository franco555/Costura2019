using Newtonsoft.Json;
using PPSale.Classes;
using PPSale.Models.Complement;
using PPSale.Models.Conexion;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace PPSale.Controllers.Complement
{
    public class MeasuredUnitsController : Controller
    {
        private ConexionContext db = new ConexionContext();
        private FunctionHelpers fn = new FunctionHelpers();

        // GET: MeasuredUnits
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

                TempData["Action"] = "Error";
                TempData["Message"] = fn.notRegistre;
            }

            var measuredUnits = db.MeasuredUnits.
                Include(m => m.Company).
                Where(m=>m.CompanyId==asing.CompanyId).
                OrderBy(m=>m.Name)
                .ThenBy(m=>m.UnitBaseId).
                ToList();
            return View(measuredUnits);
        }

        // GET: MeasuredUnits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "No se envión ID...";
                return RedirectToAction("Index");
            }
            MeasuredUnit measuredUnit = db.MeasuredUnits.Find(id);
            if (measuredUnit == null)
            {
                TempData["Error"] = "No existe regitro con este ID...";
                return RedirectToAction("Index");
            }
            return View(measuredUnit);
        }

        // GET: MeasuredUnits/Create
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

                TempData["Error"] = "Error";
                TempData["Message"] = fn.notRegistre;
            }

            var measuredUnit = new MeasuredUnit
            {
                CompanyId = asing.CompanyId,
            };

            ViewBag.UnitBaseId = new SelectList(CombosHelpers.GetUnitBase(), "unitBaseId", "Name");

            return PartialView(measuredUnit);
        }

        // POST: MeasuredUnits/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MeasuredUnit measuredUnit)
        {
            if (ModelState.IsValid)
            {
                db.MeasuredUnits.Add(measuredUnit);

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    TempData["Error"] = "Success";
                    TempData["Message"] = fn.SuccessCreate;

                    return RedirectToAction("Index");
                }

                TempData["Error"] = "Error";
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

        // GET: MeasuredUnits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "Error";
                TempData["Message"] = fn.notId;

                return RedirectToAction("Index");
            }

            var measuredUnit = db.MeasuredUnits.Find(id);
            if (measuredUnit == null)
            {
                TempData["Error"] = "Error";
                TempData["Message"] = fn.notExist;

                return RedirectToAction("Index");
            }

            ViewBag.UnitBaseId = new SelectList(CombosHelpers.GetUnitBase(), "unitBaseId", "Name",measuredUnit.UnitBaseId);

            return PartialView(measuredUnit);
        }

        // POST: MeasuredUnits/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( MeasuredUnit measuredUnit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(measuredUnit).State = EntityState.Modified;

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    TempData["Error"] = "Success";
                    TempData["Message"] = fn.SuccessUpdate;

                    return RedirectToAction("Index");
                }

                TempData["Error"] = "Error";
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

        // GET: MeasuredUnits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "No se envión ID...";
                return RedirectToAction("Index");
            }
            MeasuredUnit measuredUnit = db.MeasuredUnits.Find(id);
            if (measuredUnit == null)
            {
                TempData["Error"] = "No existe regitro con este ID...";
                return RedirectToAction("Index");
            }
            return View(measuredUnit);
        }

        // POST: MeasuredUnits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MeasuredUnit measuredUnit = db.MeasuredUnits.Find(id);
            db.MeasuredUnits.Remove(measuredUnit);
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
