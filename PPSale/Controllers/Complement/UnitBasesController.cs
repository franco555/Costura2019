using Newtonsoft.Json;
using PPSale.Classes;
using PPSale.Models.Complement;
using PPSale.Models.Conexion;
using PPSale.Models.View;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace PPSale.Controllers.Complement
{
    public class UnitBasesController : Controller
    {
        private ConexionContext db = new ConexionContext();
        private FunctionHelpers fn = new FunctionHelpers();

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

                TempData["Action"] = "Error";
                TempData["Message"] = fn.notRegistre;
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
                TempData["Action"] = "Error";
                TempData["Message"] = fn.notId;

                return RedirectToAction("Index");
            }

            var unitBase = db.UnitBases .Where(c => c.UnitBaseId == id).Include(i => i.MeasuredUnits).FirstOrDefault();

            if (unitBase == null)
             {
                 TempData["Action"] = "Error";
                 TempData["Message"] = fn.notExist;

                 return RedirectToAction("Index");
             }

            var newUnit = new UnitBaseWithMeasureUnitViewModel()
            {
                UnitBaseId=unitBase.UnitBaseId,
                Name=unitBase.Name,
                Value=unitBase.Value,
                measuredUnits=unitBase.MeasuredUnits,
            };

            return View(newUnit);
        }

        // GET: UnitBases/Create
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

                TempData["Action"] = "Error";
                TempData["Message"] = fn.notRegistre;

                return RedirectToAction("Index");
            }


            var UnitBases = new UnitBase
            {
                CompanyId = asing.CompanyId,
            };

            return PartialView(UnitBases);
        }
        
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
                TempData["Action"] = "Warning";
                TempData["Message"] = messages;
            }

            return RedirectToAction("Index");
        }

        // GET: UnitBases/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = fn.notId;

                return RedirectToAction("Index");
            }

            var unitBase = db.UnitBases.Find(id);
            if (unitBase == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = fn.notExist;

                return RedirectToAction("Index");
            }

            return PartialView(unitBase);
        }
        
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
                    TempData["Action"] = "Success";
                    TempData["Message"] = fn.SuccessUpdate;

                    return RedirectToAction("Index");
                }

                TempData["Action"] = "Error";
                TempData["Message"] = response.Message;
            }
            else
            {
                string messages = JsonConvert.SerializeObject(ModelState.Values.SelectMany(state => state.Errors).Select(error => error.ErrorMessage));
                TempData["Action"] = "Warning";
                TempData["Message"] = messages;
            }

            return RedirectToAction("Index");
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

        #region MasuredUnit
        //Create
        public ActionResult CreateMeasuredUnit(int? id)
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

                return RedirectToAction($"details/{id}");
            }

            var measuredUnit = new MeasuredUnit
            {
                CompanyId = asing.CompanyId,
                UnitBaseId=id.Value,
            };

            return PartialView(measuredUnit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMeasuredUnit(MeasuredUnit measuredUnit)
        {
            if (ModelState.IsValid)
            {
                db.MeasuredUnits.Add(measuredUnit);

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    TempData["Action"] = "Success";
                    TempData["Message"] = fn.SuccessCreate;

                    return RedirectToAction($"details/{measuredUnit.UnitBaseId}");
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

            return RedirectToAction($"details/{measuredUnit.UnitBaseId}");
        }

        //Edit
        public ActionResult EditMeasuredUnit(int? id)
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

                return RedirectToAction($"details/{id}");
            }

            if (id == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = fn.notId;
                return RedirectToAction("Index");
            }

            var  measuredUnit = db.MeasuredUnits.Find(id);
            if (measuredUnit == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = fn.notExist;

                return RedirectToAction("Index");
            }
            
            return PartialView(measuredUnit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMeasuredUnit(MeasuredUnit measuredUnit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(measuredUnit).State = EntityState.Modified;

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    TempData["Action"] = "Success";
                    TempData["Message"] = fn.SuccessUpdate;

                    return RedirectToAction($"details/{measuredUnit.UnitBaseId}");
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

            return RedirectToAction($"details/{measuredUnit.UnitBaseId}");
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
