using PPSale.Classes;
using PPSale.Models.Conexion;
using PPSale.Models.Globals;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace PPSale.Controllers.Globals
{
    [Authorize(Roles = "Admin")]

    public class UsersController : Controller
    {
        private ConexionContext db = new ConexionContext();
        private FunctionHelpers fn = new FunctionHelpers();

        // GET: Users
        public ActionResult Index()
        {
            var users = db.Users.
                Include(u => u.City).
                Include(u => u.Country).
                Include(u => u.Province).
                OrderBy(u=>u.FirstName).
                ToList();
            return View(users);
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "No se envión ID...";

                return RedirectToAction("Index");
            }
            var user = db.Users.Find(id);
            if (user == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "Registro no existente...";

                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            var user = new User {
                URL = fn.notUser,
            };
            ViewBag.CityId = new SelectList(CombosHelpers.GetCities(0,0), "CityId", "Name");
            ViewBag.CountryId = new SelectList(CombosHelpers.GetCountries(), "CountryId", "Name");
            ViewBag.ProvinceId = new SelectList(CombosHelpers.GetProvinces(0), "ProvinceId", "Name");
            return PartialView(user);
        }

        // POST: Users/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                user.Logo = fn.notUser;
                db.Users.Add(user);

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    if (user.LogoFile != null)
                    {
                        var folder = "User";
                        var Succ = FilesHelpers.UploadPhoto(user.LogoFile, folder, "",true);
                        if (Succ.Succeded)
                        {
                            user.Logo = Succ.Message;

                            db.Entry(user).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    TempData["Action"] = "Success";
                    TempData["Message"] = "Guardado Exitosamente!!!";

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

            return RedirectToAction("Index"); ;
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "No se envión ID...";

                return RedirectToAction("Index");
            }
            var user = db.Users.Find(id);
            if (user == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "Registro no existente...";

                return RedirectToAction("Index");
            }
            ViewBag.CityId = new SelectList(CombosHelpers.GetCities(user.CountryId, user.ProvinceId), "CityId", "Name", user.CityId);
            ViewBag.CountryId = new SelectList(CombosHelpers.GetCountries(), "CountryId", "Name", user.CountryId);
            ViewBag.ProvinceId = new SelectList(CombosHelpers.GetProvinces(user.CountryId), "ProvinceId", "Name", user.ProvinceId);

            if (string.IsNullOrEmpty(user.Logo))
            {
                user.Logo = fn.notUser;
            }
            user.URL = user.Logo;

            return PartialView(user);
        }

        // POST: Users/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    if (user.LogoFile != null)
                    {
                        var folder = "User";
                        var Succ = FilesHelpers.UploadPhoto(user.LogoFile, folder, user.Logo,false);
                        if (Succ.Succeded)
                        {
                            user.Logo = Succ.Message;

                            db.Entry(user).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
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

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "No se envión ID...";
                return RedirectToAction("Index");
            }
            var user = db.Users.Find(id);
            if (user == null)
            {
                TempData["Error"] = "No existe regitro con este ID...";
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var user = db.Users.Find(id);
            db.Users.Remove(user);
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
