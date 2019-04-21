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

        // GET: Rols
        public ActionResult Index()
        {
            var rol = UserHelpers.listRole();
            return View(rol);
        }

        // GET: Rols/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "No se envión ID...";

                return RedirectToAction("Index");
            }
            var rol = db.Rols.Find(id);
            if (rol == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "No existe registro...";

                return RedirectToAction("Index");
            }
            return View(rol);
        }

        // GET: Rols/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: Rols/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
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
                TempData["Action"] = "Warning";
                TempData["Message"] = "Hay campos vacios...";
            }

            return RedirectToAction("Index");
        }

        // GET: Rols/Edit/5
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

        // POST: Rols/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Rol rol)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rol).State = EntityState.Modified;

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    TempData["Action"] = "Success";
                    TempData["Message"] = "Actualizado Exitosamente!!!";

                    return RedirectToAction("Index");
                }
                TempData["Action"] = "Error";
                TempData["Message"] = response.Message;
            }
            else
            {
                TempData["Action"] = "Warning";
                TempData["Message"] = "Hay campos vacios...";
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
