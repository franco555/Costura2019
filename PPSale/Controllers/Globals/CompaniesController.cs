
namespace PPSale.Controllers.Globals
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;
    using Classes;
    using Models.Conexion;
    using Models.Globals;
    using PPSale.Models.View;

    [Authorize(Roles = "Admin")]

    public class CompaniesController : Controller
    {
        private ConexionContext db = new ConexionContext();

        // GET: Companies
        public ActionResult Index()
        {
            var companies = db.Companies.
                Include(c => c.City).
                Include(c => c.Country).
                Include(c => c.IvaCondition).
                Include(c => c.Province).
                OrderBy(c=>c.Name).
                ToList();
            return View(companies);
        }

        // GET: Companies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "No se envión ID...";

                return RedirectToAction("Index");
            }
            var company = db.Companies.Find(id);
            if (company == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "No existe regitro con este ID...";

                return RedirectToAction("Index");
            }

            var users= (from u in db.Users
                                   join a in db.AsingRolAndUsers on u.UserId equals a.UserId
                                   join c in db.Companies on a.CompanyId equals c.CompanyId
                                   where c.CompanyId == company.CompanyId

                                   select new {u }
                                   ).ToList();

            var user = new List<User>();
            foreach (var item in users)
            {
                user.Add(new User() {
                    UserId = item.u.UserId,
                    FirstName = item.u.FirstName,
                    LastName = item.u.LastName,
                });
            }

            var companyWithUsers = new CompanyWithUsersViewModel() {
                CompanyId=company.CompanyId,
                Name=company.Name,
                Logo=company.Logo,
                Address=company.Address,
                Users=user
            };
            return View(companyWithUsers);
        }

        // GET: Companies/Create
        public ActionResult Create()
        {
            var company = new Company {
                FirstDate = DateTime.Now,
                URL = "~/Content/Logos/Empresa/otro.png",
            };
            ViewBag.CityId = new SelectList(CombosHelpers.GetCities(0,0), "CityId", "Name");
            ViewBag.CountryId = new SelectList(CombosHelpers.GetCountries(), "CountryId", "Name");
            ViewBag.IvaConditionId = new SelectList(CombosHelpers.GetIvaConditions(), "IvaConditionId", "Name");
            ViewBag.ProvinceId = new SelectList(CombosHelpers.GetProvinces(0), "ProvinceId", "Name");
            return PartialView(company);
        }

        // POST: Companies/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Company company)
        {
            if (ModelState.IsValid)
            {
                company.Logo = "~/Content/Logos/Empresa/otro.png";
                db.Companies.Add(company);

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    if (company.LogoFile != null)
                    {
                        var folder = "~/Content/Logos/Empresa";
                        var Name = string.Format("{0}.jpg", company.CompanyId);
                        var Succ = FilesHelpers.UploadPhoto(company.LogoFile, folder, Name);
                        if (Succ.Succeded)
                        {
                            var pic = string.Format("{0}/{1}", folder, Name);
                            company.Logo = pic;

                            db.Entry(company).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    return RedirectToAction("Index", company);
                }
                ModelState.AddModelError(string.Empty, response.Message);
            }

            TempData["Error"] = "Modelo no válido";

            ViewBag.CityId = new SelectList(CombosHelpers.GetCities(company.CountryId, company.ProvinceId), "CityId", "Name", company.CityId);
            ViewBag.CountryId = new SelectList(CombosHelpers.GetCountries(), "CountryId", "Name", company.CountryId);
            ViewBag.IvaConditionId = new SelectList(CombosHelpers.GetIvaConditions(), "IvaConditionId", "Name", company.IvaConditionId);
            ViewBag.ProvinceId = new SelectList(CombosHelpers.GetProvinces(company.CountryId), "ProvinceId", "Name", company.ProvinceId);
            return View(company);
        }

        // GET: Companies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "No se envión ID...";
                return RedirectToAction("Index");
            }

            var company = db.Companies.Find(id);
            if (company == null)
            {
                TempData["Error"] = "No existe regitro con este ID...";
                return RedirectToAction("Index");
            }

            company.URL = company.Logo;
            company.Logo= company.Logo;


            ViewBag.CityId = new SelectList(CombosHelpers.GetCities(company.CountryId, company.ProvinceId), "CityId", "Name", company.CityId);
            ViewBag.CountryId = new SelectList(CombosHelpers.GetCountries(), "CountryId", "Name", company.CountryId);
            ViewBag.IvaConditionId = new SelectList(CombosHelpers.GetIvaConditions(), "IvaConditionId", "Name", company.IvaConditionId);
            ViewBag.ProvinceId = new SelectList(CombosHelpers.GetProvinces(company.CountryId), "ProvinceId", "Name", company.ProvinceId);
            return View(company);
        }

        // POST: Companies/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Company company)
        {
            if (ModelState.IsValid)
            {
                db.Entry(company).State = EntityState.Modified;

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    if (company.LogoFile != null)
                    {
                        var folder = "~/Content/Logos/Empresa";
                        var Name = string.Format("{0}.jpg", company.CompanyId);
                        var Succ = FilesHelpers.UploadPhoto(company.LogoFile, folder, Name);
                        if (Succ.Succeded)
                        {
                            var pic = string.Format("{0}/{1}", folder, Name);
                            company.Logo = pic;

                            db.Entry(company).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    return RedirectToAction("Index", company);
                }
                ModelState.AddModelError(string.Empty, response.Message);
            }

            TempData["Error"] = "Modelo no válido";

            ViewBag.CityId = new SelectList(CombosHelpers.GetCities(company.CountryId, company.ProvinceId), "CityId", "Name", company.CityId);
            ViewBag.CountryId = new SelectList(CombosHelpers.GetCountries(), "CountryId", "Name", company.CountryId);
            ViewBag.IvaConditionId = new SelectList(CombosHelpers.GetIvaConditions(), "IvaConditionId", "Name", company.IvaConditionId);
            ViewBag.ProvinceId = new SelectList(CombosHelpers.GetProvinces(company.CountryId), "ProvinceId", "Name", company.ProvinceId);
            return View(company);
        }

        // GET: Companies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "No se envión ID...";
                return RedirectToAction("Index");
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                TempData["Error"] = "No existe regitro con este ID...";
                return RedirectToAction("Index");
            }
            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Company company = db.Companies.Find(id);
            db.Companies.Remove(company);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Users/Create
        public ActionResult AddUsers(int? Id)
        {
            var user = new User
            {
                URL = "~/Content/Logos/User/otro.png",
                companyId = Id.Value,
            };
            ViewBag.CityId = new SelectList(CombosHelpers.GetCities(0, 0), "CityId", "Name");
            ViewBag.CountryId = new SelectList(CombosHelpers.GetCountries(), "CountryId", "Name");
            ViewBag.ProvinceId = new SelectList(CombosHelpers.GetProvinces(0), "ProvinceId", "Name");

            return PartialView(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUsers(User user)
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
                        if (Succ.Succeded)
                        {
                            var pic = string.Format("{0}/{1}", folder, Name);
                            user.Logo = pic;

                            db.Entry(user).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    TempData["Action"] = "Succes";
                    TempData["Message"] = "Guardado Correctamente!!";
                    return RedirectToAction($"Details/{user.companyId}");
                }
                ModelState.AddModelError(string.Empty, response.Message);
            }

            TempData["Action"] = "Error";
            TempData["Message"] = "Modelo No válido";


            return RedirectToAction($"Details/{user.companyId}");
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
