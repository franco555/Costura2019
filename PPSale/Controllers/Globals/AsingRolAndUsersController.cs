using PPSale.Classes;
using PPSale.Models.Conexion;
using PPSale.Models.Globals;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace PPSale.Controllers.Globals
{
    [Authorize(Roles ="Admin")]

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

        // GET: AsingRolAndUsers/Create
        public ActionResult Create()
        {
            var us = new AsingRolAndUser {Email="Firts",};

            ViewBag.CompanyId = new SelectList(CombosHelpers.Getcompanies(), "CompanyId", "Name");
            ViewBag.RolId = new SelectList(CombosHelpers.GetRols(), "RolId", "Name");
            ViewBag.UserId = new SelectList(CombosHelpers.GetUsers(), "UserId", "UserName");
            return View(us);
        }

        // POST: AsingRolAndUsers/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AsingRolAndUser asingRolAndUser)
        {
            if (ModelState.IsValid)
            {
                var db1 = new ConexionContext();
                var user = db1.Users.Find(asingRolAndUser.UserId);
                var newEmail = asingRolAndUser.CompanyId + user.UserName;
                db1.Dispose();

                asingRolAndUser.Email = newEmail;
                db.AsingRolAndUsers.Add(asingRolAndUser);

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    var rol = db.Rols.Find(asingRolAndUser.RolId);
                    UsersHelper.CreateUserASP(newEmail, rol.Name,user.UserName);

                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Modelo no válido");
            }

            ViewBag.CompanyId = new SelectList(CombosHelpers.Getcompanies(), "CompanyId", "Name", asingRolAndUser.CompanyId);
            ViewBag.RolId = new SelectList(CombosHelpers.GetRols(), "RolId", "Name", asingRolAndUser.RolId);
            ViewBag.UserId = new SelectList(CombosHelpers.GetUsers(), "UserId", "UserName", asingRolAndUser.UserId);
            return View(asingRolAndUser);
        }

        // GET: AsingRolAndUsers/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.CompanyId = new SelectList(CombosHelpers.Getcompanies(), "CompanyId", "Name", asingRolAndUser.CompanyId);
            ViewBag.RolId = new SelectList(CombosHelpers.GetRols(), "RolId", "Name", asingRolAndUser.RolId);
            ViewBag.UserId = new SelectList(CombosHelpers.GetUsers(), "UserId", "UserName", asingRolAndUser.UserId);
            return View(asingRolAndUser);
        }

        // POST: AsingRolAndUsers/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AsingRolAndUser asingRolAndUser)
        {
            if (ModelState.IsValid)
            {
                var newEmail = string.Empty;
                var modifidEmail = false;

                //Obtener valores anteriores
                var bd2 = new ConexionContext();
                var asing = bd2.AsingRolAndUsers.Find(asingRolAndUser.AsingRolAndUserId);
                var rName = bd2.Rols.Find(asing.RolId);
                var uName = bd2.Users.Find(asing.UserId);
                bd2.Dispose();

                //si se cambia Company o User se modifica el EMAIL
                if (asingRolAndUser.UserId!=asing.UserId || asingRolAndUser.CompanyId != asing.CompanyId)
                {
                    modifidEmail = true;

                    var bd3 = new ConexionContext();
                    var uss = bd3.Users.Find(asingRolAndUser.UserId);
                    bd3.Dispose();

                    newEmail = asingRolAndUser.CompanyId + uss.UserName;
                    asingRolAndUser.Email = newEmail;
                }

                db.Entry(asingRolAndUser).State = EntityState.Modified;

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    var rol = db.Rols.Find(asingRolAndUser.RolId);

                    if (modifidEmail)
                    {
                        UsersHelper.UpdateUser(asing.Email, asingRolAndUser.Email);
                    }

                    UsersHelper.RemoveRol(asing.Email, rName.Name);
                    UsersHelper.CreateUserASP(asingRolAndUser.Email, rol.Name, uName.UserName);
                    
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Modelo no válido");
            }
            ViewBag.CompanyId = new SelectList(CombosHelpers.Getcompanies(), "CompanyId", "Name", asingRolAndUser.CompanyId);
            ViewBag.RolId = new SelectList(CombosHelpers.GetRols(), "RolId", "Name", asingRolAndUser.RolId);
            ViewBag.UserId = new SelectList(CombosHelpers.GetUsers(), "UserId", "UserName", asingRolAndUser.UserId);
            return View(asingRolAndUser);
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
