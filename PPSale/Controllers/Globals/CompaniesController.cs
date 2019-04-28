
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
    using Newtonsoft.Json;
    using PPSale.Models.View;

    [Authorize(Roles = "Admin")]

    public class CompaniesController : Controller
    {
        private ConexionContext db = new ConexionContext();
        private FunctionHelpers fn = new FunctionHelpers();
        
        public ActionResult Index()
        {
            var query = (from com in db.Companies
                         join iva in db.IvaConditions on com.IvaConditionId equals iva.IvaConditionId
                         join cit in db.Cities on com.CityId equals cit.CityId
                         join pro in db.Provinces on cit.ProvinceId equals pro.ProvinceId
                         join cou in db.Countries on pro.CountryId equals cou.CountryId
                         where com.Name !=("Anónimo")
                         select new {com,cit,pro,cou,iva }).ToList();

            var companies = new List<CompanyViewModel>();
            foreach (var item in query)
            {
                companies.Add(new CompanyViewModel()
                {
                    CompanyId = item.com.CompanyId,
                    Name = item.com.Name,
                    Cuit = item.com.Cuit,
                    Address = item.com.Address,
                    Email = item.com.Email,
                    Web = item.com.Web,
                    FirstDate = item.com.FirstDate,
                    Logo = item.com.Logo,
                    IvaConditionName = item.iva.Name,
                    CountryName = item.cou.Name,
                    ProvinceName = item.pro.Name,
                    CityName = item.cit.Name
                });
            }
            
            return View(companies);
        }
        
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

            var users = (from u in db.Users
                         join a in db.AsingRolAndUsers on u.UserId equals a.UserId
                         join c in db.Companies on a.CompanyId equals c.CompanyId
                         join r in db.Rols on a.RolId equals r.RolId
                         where c.CompanyId == company.CompanyId

                         select new { u, r }
                                   ).ToList();

            var user = new List<User>();
            foreach (var item in users)
            {
                user.Add(new User()
                {
                    UserId = item.u.UserId,
                    companyId = item.u.companyId,
                    URL = item.r.Name,
                    Logo = item.u.Logo,
                    FirstName = item.u.FirstName,
                    LastName = item.u.LastName,
                });
            }

            var companyWithUsers = new CompanyWithUsersViewModel()
            {
                CompanyId = company.CompanyId,
                Name = company.Name,
                Logo = company.Logo,
                Address = company.Address,
                Users = user
            };
            return View(companyWithUsers);
        }
        
        public ActionResult Create()
        {
            var company = new Company
            {
                FirstDate = DateTime.Now,
                URL = fn.notCompany,
            };
            ViewBag.CityId = new SelectList(CombosHelpers.GetCities(0, 0), "CityId", "Name");
            ViewBag.CountryId = new SelectList(CombosHelpers.GetCountries(), "CountryId", "Name");
            ViewBag.IvaConditionId = new SelectList(CombosHelpers.GetIvaConditions(), "IvaConditionId", "Name");
            ViewBag.ProvinceId = new SelectList(CombosHelpers.GetProvinces(0), "ProvinceId", "Name");
            return PartialView(company);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Company company)
        {
            if (ModelState.IsValid)
            {
                company.Logo = fn.notCompany;
                db.Companies.Add(company);

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    if (company.LogoFile != null)
                    {
                        var folder = "Empresa";
                        var Succ = FilesHelpers.UploadPhoto(company.LogoFile, folder, "", true); //True si es nuevo, false si es editado
                        if (Succ.Succeded)
                        {
                            company.Logo = Succ.Message;

                            db.Entry(company).State = EntityState.Modified;
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

            return RedirectToAction("Index");
        }
        
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

            company.Logo = company.Logo == null ? fn.notCompany : company.Logo;
            company.URL = company.Logo;
            
            ViewBag.CityId = new SelectList(CombosHelpers.GetCities(company.City.CountryId, company.City.ProvinceId), "CityId", "Name", company.CityId);
            ViewBag.CountryId = new SelectList(CombosHelpers.GetCountries(), "CountryId", "Name", company.City.CountryId);
            ViewBag.IvaConditionId = new SelectList(CombosHelpers.GetIvaConditions(), "IvaConditionId", "Name", company.IvaConditionId);
            ViewBag.ProvinceId = new SelectList(CombosHelpers.GetProvinces(company.City.CountryId), "ProvinceId", "Name", company.City.ProvinceId);

            return PartialView(company);
        }
        
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
                        var folder = "Empresa";
                        var Succ = FilesHelpers.UploadPhoto(company.LogoFile, folder, company.Logo, false);
                        if (Succ.Succeded)
                        {
                            company.Logo = Succ.Message;

                            db.Entry(company).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    TempData["Action"] = "Success";
                    TempData["Message"] = "Actualizado Exitosamente!!!";

                    return RedirectToAction("Index", company);
                }

                TempData["Action"] = "Error";
                TempData["Message"] =response.Message;

            }
            else
            {
                string messages = JsonConvert.SerializeObject(ModelState.Values.SelectMany(state => state.Errors).Select(error => error.ErrorMessage));

                TempData["Action"] = "Object";
                TempData["Message"] = messages;
            }

            return RedirectToAction("Index");
        }
        
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
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Company company = db.Companies.Find(id);
            db.Companies.Remove(company);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        public ActionResult AddUsers(int? Id)
        {
            if (Id == null)
            {
                TempData["Error"] = "No se envión ID de Useuario...";
                return RedirectToAction($"Details/{Id}");
            }

            var user = new User
            {
                URL = "~/Content/Logos/NotUser.jpg",
                companyId = Id.Value,
            };
            ViewBag.CityId = new SelectList(CombosHelpers.GetCities(0, 0), "CityId", "Name");
            ViewBag.CountryId = new SelectList(CombosHelpers.GetCountries(), "CountryId", "Name");
            ViewBag.ProvinceId = new SelectList(CombosHelpers.GetProvinces(0), "ProvinceId", "Name");

            ViewBag.rolId = new SelectList(CombosHelpers.GetRols(), "RolId", "Name");

            return PartialView(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUsers(User user)
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
                        var Succ = FilesHelpers.UploadPhoto(user.LogoFile, folder, "", true);
                        if (Succ.Succeded)
                        {
                            user.Logo = Succ.Message;

                            db.Entry(user).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            TempData["Message"] = $"Guardado Correctamente!!. Error en AsingAndRol: { Succ.Message}";
                        }
                    }

                    var asingRolAndUser = new AsingRolAndUser()
                    {
                        CompanyId = user.companyId,
                        UserId = user.UserId,
                        RolId = user.rolId,
                        Email = user.UserName,
                        Lock = false
                    };

                    db.AsingRolAndUsers.Add(asingRolAndUser);

                    var rol = db.Rols.Find(user.rolId);
                    var Succ2 = DBHelpers.SaveChage(db);

                    if (Succ2.Succeded)
                    {
                        UsersHelper.CreateUserASP(user.UserName, user.UserName, rol.Name);
                    }
                    else
                    {
                        TempData["Message"] = $"Guardado Correctamente!!. Error en UserASP: { Succ2.Message}";
                    }


                    TempData["Action"] = "Success";
                    return RedirectToAction($"Details/{user.companyId}");
                }

                TempData["Action"] = "Error";
                TempData["Message"] = response.Message;
            }
            else
            {
                //string messages = string.Join(";", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                string messages = JsonConvert.SerializeObject(ModelState.Values.SelectMany(state => state.Errors).Select(error => error.ErrorMessage));

                TempData["Action"] = "Object";
                TempData["Message"] = messages;
            }

            return RedirectToAction($"Details/{user.companyId}");
        }

        public ActionResult ChangeRol(int? IdCompany, int? IdUser)
        {
            if (IdCompany == null || IdUser == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = $"No existe regitro con este IDs: compañiaId={IdCompany}. UsuarioId{ IdUser}";

                return RedirectToAction($"Details/{IdCompany}");
            }

            var us = new ChangeRolViewModel
            {
                CompanyId = IdCompany.Value,
                UserId = IdUser.Value,
            };

            var aar = db.AsingRolAndUsers.Where(d => d.UserId == IdUser && d.CompanyId == IdCompany).FirstOrDefault();
            ViewBag.RolId = new SelectList(CombosHelpers.GetRols(), "RolId", "Name", aar.RolId);
            return PartialView(us);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeRol(ChangeRolViewModel model)
        {
            if (ModelState.IsValid)
            {
                var asing = db.AsingRolAndUsers.Where(r => r.CompanyId == model.CompanyId && r.UserId == model.UserId).FirstOrDefault();
                var newROl = db.Rols.Find(model.RolId);

                asing.RolId = newROl.RolId;
                db.Entry(asing).State = EntityState.Modified;

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    TempData["Action"] = "Success";
                    TempData["Message"] = "Actualizado Exitosamente!!!";
                }

                var OldROl = db.Rols.Find(asing.RolId);

                UsersHelper.RemoveRol(asing.Email, OldROl.Name);
                UsersHelper.CreateUserASP(asing.Email, asing.Email, newROl.Name);
            }

            return RedirectToAction($"Details/{model.CompanyId}");
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
