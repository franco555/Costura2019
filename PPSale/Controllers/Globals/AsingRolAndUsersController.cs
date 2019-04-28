using Newtonsoft.Json;
using PPSale.Classes;
using PPSale.Models.Conexion;
using PPSale.Models.Globals;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace PPSale.Controllers.Globals
{
    [Authorize(Roles = "Admin")]

    public class AsingRolAndUsersController : Controller
    {
        private ConexionContext db = new ConexionContext();

        // GET: AsingRolAndUsers
        public ActionResult Index()
        {
            var asingRolAndUsers = db.AsingRolAndUsers.
                Include(a => a.Company).
                Include(a => a.Rol).
                Include(a => a.User).
                OrderBy(a => a.Company.Name).
                ThenBy(a => a.User.FirstName).ToList();
            return View(asingRolAndUsers);
        }

        // GET: AsingRolAndUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "No se envio ID...";

                return RedirectToAction("Index");
            }
            AsingRolAndUser asingRolAndUser = db.AsingRolAndUsers.Find(id);
            if (asingRolAndUser == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "Registro no existente...";

                return RedirectToAction("Index");
            }
            return View(asingRolAndUser);
        }

        // GET: AsingRolAndUsers/Create
        public ActionResult Create()
        {
            var us = new AsingRolAndUser { Email = "Firts", };

            ViewBag.CompanyId = new SelectList(CombosHelpers.Getcompanies(), "CompanyId", "Name");
            ViewBag.RolId = new SelectList(CombosHelpers.GetRols(), "RolId", "Name");
            ViewBag.UserId = new SelectList(CombosHelpers.GetUsers(), "UserId", "UserName");
            return PartialView(us);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AsingRolAndUser asingRolAndUser)
        {
            if (ModelState.IsValid)
            {
                var db1 = new ConexionContext();
                var user = db1.Users.Find(asingRolAndUser.UserId);
                db1.Dispose();

                asingRolAndUser.Email = user.UserName;
                db.AsingRolAndUsers.Add(asingRolAndUser);

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    var rol = db.Rols.Find(asingRolAndUser.RolId);
                    UsersHelper.CreateUserASP(user.UserName, user.UserName, rol.Name);


                    TempData["Action"] = "Success";
                    TempData["Message"] = "Guardado Correctamente..!!!";
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

        // GET: AsingRolAndUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "No se envio ID...";

                return RedirectToAction("Index");
            }
            AsingRolAndUser asingRolAndUser = db.AsingRolAndUsers.Find(id);
            if (asingRolAndUser == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "Registro no existente...";

                return RedirectToAction("Index");
            }
            ViewBag.CompanyId = new SelectList(CombosHelpers.Getcompanies(), "CompanyId", "Name", asingRolAndUser.CompanyId);
            ViewBag.RolId = new SelectList(CombosHelpers.GetRols(), "RolId", "Name", asingRolAndUser.RolId);
            ViewBag.UserId = new SelectList(CombosHelpers.GetUsers(), "UserId", "UserName", asingRolAndUser.UserId);
            return PartialView(asingRolAndUser);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AsingRolAndUser asingRolAndUser)
        {
            if (ModelState.IsValid)
            {

                //Obtener valores anteriores
                var db1 = new ConexionContext();
                var asing = db1.AsingRolAndUsers.Find(asingRolAndUser.AsingRolAndUserId);
                var oldRol = db1.Rols.Find(asing.RolId);
                var oldUser = db1.Users.Find(asing.UserId);

                var newrol = db1.Rols.Find(asingRolAndUser.RolId);
                var newuser = db1.Users.Find(asingRolAndUser.UserId);
                db1.Dispose();

                if (asingRolAndUser.UserId != oldUser.UserId)
                {
                    asingRolAndUser.Email = newuser.UserName;
                }

                db.Entry(asingRolAndUser).State = EntityState.Modified;

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    UsersHelper.RemoveRol(oldUser.UserName, oldRol.Name);
                    UsersHelper.CreateUserASP(asingRolAndUser.Email, newuser.UserName, newrol.Name);

                    TempData["Action"] = "Success";
                    TempData["Message"] = "Guardado Correctamente..!!!";

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

        // GET: AsingRolAndUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "No se envión ID...";
                return RedirectToAction("Index");
            }
            AsingRolAndUser asingRolAndUser = db.AsingRolAndUsers.Find(id);
            if (asingRolAndUser == null)
            {
                TempData["Error"] = "No existe regitro con este ID...";
                return RedirectToAction("Index");
            }
            return View(asingRolAndUser);
        }

        // POST: AsingRolAndUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AsingRolAndUser asingRolAndUser = db.AsingRolAndUsers.Find(id);
            db.AsingRolAndUsers.Remove(asingRolAndUser);
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
