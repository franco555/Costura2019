using Newtonsoft.Json;
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

    public class RolsController : Controller
    {
        private ConexionContext db = new ConexionContext();
        private UsersHelper UserHelpers = new UsersHelper();

        public ActionResult Index()
        {
            var rol = db.Rols.ToList();
            return View(rol);
        }

        public ActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Rol rol)
        {
            if (ModelState.IsValid)
            {
                db.Rols.Add(rol);

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    UserHelpers.CheckRol(rol.Name,false);
                    TempData["Action"] = "Success";
                    TempData["Message"] = "Guardado Exitosamente!!!";

                    return RedirectToAction("Index");
                }

                TempData["Action"] = "Error";
                TempData["Message"] = response.Message;

                return RedirectToAction("Index");
            }
            else
            {
                string messages = JsonConvert.SerializeObject(ModelState.Values.SelectMany(state => state.Errors).Select(error => error.ErrorMessage));

                TempData["Action"] = "Object";
                TempData["Message"] = messages;
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "No se envio ID...";

                return RedirectToAction("Index");
            }
            var rol = db.Rols.Find(id);
            if (rol == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "Registro no existente...";

                return RedirectToAction("Index");
            }
            return PartialView(rol);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Rol rol)
        {
            var db2= new ConexionContext();
            var oldRol = db2.Rols.Find(rol.RolId).Name;
            db2.Dispose();

            if (ModelState.IsValid)
            {
                db.Entry(rol).State = EntityState.Modified;

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    UserHelpers.UpdateRol(oldRol, rol.Name);

                    TempData["Action"] = "Success";
                    TempData["Message"] = "Actualizado Exitosamente!!!";

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

        // GET: Rols/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "No se envión ID...";
                return RedirectToAction("Index");
            }
            var rol = db.Rols.Find(id);
            if (rol == null)
            {
                TempData["Error"] = "No existe regitro con este ID...";
                return RedirectToAction("Index");
            }
            return View(rol);
        }

        // POST: Rols/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var rol = db.Rols.Find(id);
            db.Rols.Remove(rol);
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
