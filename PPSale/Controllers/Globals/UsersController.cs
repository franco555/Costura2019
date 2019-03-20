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

        // GET: Users/Create
        public ActionResult Create()
        {
            var user = new User {
                URL = "~/Content/Logos/User/otro.png",
            };
            ViewBag.CityId = new SelectList(CombosHelpers.GetCities(0,0), "CityId", "Name");
            ViewBag.CountryId = new SelectList(CombosHelpers.GetCountries(), "CountryId", "Name");
            ViewBag.ProvinceId = new SelectList(CombosHelpers.GetProvinces(0), "ProvinceId", "Name");
            return View(user);
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
                user.Logo = "~/Content/Logos/User/otro.png";
                db.Users.Add(user);

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    if (user.LogoFile != null)
                    {
                        var folder = "~/Content/Logos/User";
                        var Name = string.Format("{0}.jpg", user.UserId);
                        var Succ = FilesHelpers.UploadPhoto(user.LogoFile, folder, Name);
                        if (Succ)
                        {
                            var pic = string.Format("{0}/{1}", folder, Name);
                            user.Logo = pic;

                            db.Entry(user).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    return RedirectToAction("Index", user);
                }
                ModelState.AddModelError(string.Empty, response.Message);
            }

            TempData["Error"] = "Modelo no válido";

            ViewBag.CityId = new SelectList(CombosHelpers.GetCities(user.CountryId, user.ProvinceId), "CityId", "Name", user.CityId);
            ViewBag.CountryId = new SelectList(CombosHelpers.GetCountries(), "CountryId", "Name", user.CountryId);
            ViewBag.ProvinceId = new SelectList(CombosHelpers.GetProvinces(user.CountryId), "ProvinceId", "Name", user.ProvinceId);
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.CityId = new SelectList(CombosHelpers.GetCities(user.CountryId, user.ProvinceId), "CityId", "Name", user.CityId);
            ViewBag.CountryId = new SelectList(CombosHelpers.GetCountries(), "CountryId", "Name", user.CountryId);
            ViewBag.ProvinceId = new SelectList(CombosHelpers.GetProvinces(user.CountryId), "ProvinceId", "Name", user.ProvinceId);

            if (string.IsNullOrEmpty(user.Logo))
            {
                user.Logo = "~/Content/Logos/User/otro.png";
            }
            user.URL = user.Logo;

            return View(user);
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
                        var folder = "~/Content/Logos/User/";
                        var Name = string.Format("{0}.jpg", user.UserId);
                        var Succ = FilesHelpers.UploadPhoto(user.LogoFile, folder, Name);
                        if (Succ)
                        {
                            var pic = string.Format("{0}/{1}", folder, Name);
                            user.Logo = pic;

                            db.Entry(user).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    return RedirectToAction("Index", user);
                }
                ModelState.AddModelError(string.Empty, response.Message);
            }

            ModelState.AddModelError(string.Empty, "Modelo no válido");
            ViewBag.CityId = new SelectList(CombosHelpers.GetCities(user.CountryId, user.ProvinceId), "CityId", "Name", user.CityId);
            ViewBag.CountryId = new SelectList(CombosHelpers.GetCountries(), "CountryId", "Name", user.CountryId);
            ViewBag.ProvinceId = new SelectList(CombosHelpers.GetProvinces(user.CountryId), "ProvinceId", "Name", user.ProvinceId);
            return View(user);
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
