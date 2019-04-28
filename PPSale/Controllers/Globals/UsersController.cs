using Newtonsoft.Json;
using PPSale.Classes;
using PPSale.Models.Conexion;
using PPSale.Models.Globals;
using PPSale.Models.View;
using System.Collections.Generic;
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
            var query = (from user in db.Users
                         join cit in db.Cities on user.CityId equals cit.CityId
                         join pro in db.Provinces on cit.ProvinceId equals pro.ProvinceId
                         join cou in db.Countries on pro.CountryId equals cou.CountryId
                         where user.LastName != ("Anónimo")
                         select new { user, cit, pro, cou }).ToList();

            var users = new List<UserViewModel>();
            foreach (var item in query)
            {
                users.Add(new UserViewModel()
                {
                    userId = item.user.UserId,
                    UserName = item.user.UserName,
                    LastName = item.user.LastName,
                    Cuit = item.user.Cuit,
                    Phone = item.user.Phone,
                    Address = item.user.Address,
                    Logo = item.user.Logo,
                    CountryName = item.cou.Name,
                    ProvinceName = item.pro.Name,
                    CityName = item.cit.Name
                });
            }

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
                string messages = JsonConvert.SerializeObject(ModelState.Values.SelectMany(state => state.Errors).Select(error => error.ErrorMessage));

                TempData["Action"] = "Object";
                TempData["Message"] = messages;
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

            user.Logo = user.Logo ==null? fn.notUser:user.Logo;
            user.URL = user.Logo;

            ViewBag.CityId = new SelectList(CombosHelpers.GetCities(user.City.CountryId, user.City.ProvinceId), "CityId", "Name", user.CityId);
            ViewBag.CountryId = new SelectList(CombosHelpers.GetCountries(), "CountryId", "Name", user.City.CountryId);
            ViewBag.ProvinceId = new SelectList(CombosHelpers.GetProvinces(user.City.CountryId), "ProvinceId", "Name", user.City.ProvinceId);
            
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
                string messages = JsonConvert.SerializeObject(ModelState.Values.SelectMany(state => state.Errors).Select(error => error.ErrorMessage));

                TempData["Action"] = "Object";
                TempData["Message"] = messages;
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
