using Newtonsoft.Json;
using PPSale.Classes;
using PPSale.Models.Complement;
using PPSale.Models.Conexion;
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
                Where(c=>c.CompanyId==asing.CompanyId ).
                OrderBy(c=>c.Name).ToList();

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
            return View(classification);
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
        public ActionResult Edit( Classification classification)
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
