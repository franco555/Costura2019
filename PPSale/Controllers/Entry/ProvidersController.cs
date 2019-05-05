using Newtonsoft.Json;
using PPSale.Classes;
using PPSale.Models.Conexion;
using PPSale.Models.Entry;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace PPSale.Controllers.Entry
{
    public class ProvidersController : Controller
    {
        private ConexionContext db = new ConexionContext();
        private FunctionHelpers fn = new FunctionHelpers();

        // GET: Providers
        public ActionResult Index()
        {
            var asing = db.AsingRolAndUsers.Where(a => a.Email == User.Identity.Name).FirstOrDefault();

            if (asing==null){
                asing = new Models.Globals.AsingRolAndUser {
                    AsingRolAndUserId=0,
                    CompanyId = 0,
                };

                TempData["Error"] = "Error";
                TempData["Message"] = fn.notRegistre;
            }

            var providers = db.Providers.
                Include(p => p.City).
                Include(p => p.Company).
                Include(p => p.Country).
                Include(p => p.IvaCondition).
                Include(p => p.Province).
                Where(c => c.CompanyId == asing.CompanyId).
                OrderBy(c => c.Name).
                ToList();
            return View(providers);
        }

        // GET: Providers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "Error";
                TempData["Message"] = fn.notId;

                return RedirectToAction("Index");
            }

            var provider = db.Providers.Find(id);
            if (provider == null)
            {
                TempData["Error"] = "Error";
                TempData["Message"] = fn.notExist;

                return RedirectToAction("Index");
            }
            return View(provider);
        }

        // GET: Providers/Create
        public ActionResult Create()
        {
            var asing = db.AsingRolAndUsers.Where(a=>a.Email==User.Identity.Name).FirstOrDefault();
            var provider = new Provider {
                CompanyId =asing.CompanyId,
                FirstDate = DateTime.Now,
                URL = fn.notCompany,
            };

            ViewBag.CityId = new SelectList(CombosHelpers.GetCities(0, 0), "CityId", "Name");
            ViewBag.CountryId = new SelectList(CombosHelpers.GetCountries(), "CountryId", "Name");
            ViewBag.IvaConditionId = new SelectList(CombosHelpers.GetIvaConditions(), "IvaConditionId", "Name");
            ViewBag.ProvinceId = new SelectList(CombosHelpers.GetProvinces(0), "ProvinceId", "Name");
            return PartialView(provider);
        }

        // POST: Providers/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Provider provider)
        {
            if (ModelState.IsValid)
            {
                provider.Logo = fn.notCompany;

                db.Providers.Add(provider);

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    if (provider.LogoFile != null)
                    {
                        var folder = "Provider";
                        var Succ = FilesHelpers.UploadPhoto(provider.LogoFile, folder, "",true);
                        if (Succ.Succeded)
                        {
                            provider.Logo = Succ.Message;

                            db.Entry(provider).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                    }
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

        // GET: Providers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "Error";
                TempData["Message"] = fn.notId;

                return RedirectToAction("Index");
            }

            var provider = db.Providers.Find(id);
            if (provider == null)
            {
                TempData["Error"] = "Error";
                TempData["Message"] = fn.notExist;

                return RedirectToAction("Index");
            }

            provider.URL = provider.Logo;

            ViewBag.CityId = new SelectList(CombosHelpers.GetCities(provider.CountryId, provider.ProvinceId), "CityId", "Name", provider.CityId);
            ViewBag.CountryId = new SelectList(CombosHelpers.GetCountries(), "CountryId", "Name", provider.CountryId);
            ViewBag.IvaConditionId = new SelectList(CombosHelpers.GetIvaConditions(), "IvaConditionId", "Name", provider.IvaConditionId);
            ViewBag.ProvinceId = new SelectList(CombosHelpers.GetProvinces(provider.CountryId), "ProvinceId", "Name", provider.ProvinceId);

            return PartialView(provider);
        }

        // POST: Providers/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Provider provider)
        {
            if (ModelState.IsValid)
            {               
                db.Entry(provider).State = EntityState.Modified;

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    if (provider.LogoFile != null)
                    {
                        //var Name = provider.Logo.Substring(provider.Logo.LastIndexOf('/') + 1);
                        var folder = "Provider";
                        var Succ = FilesHelpers.UploadPhoto(provider.LogoFile, folder, provider.Logo,false);
                        if (Succ.Succeded)
                        {
                            provider.Logo = Succ.Message;

                            db.Entry(provider).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                    }
                    TempData["Action"] = "Success";
                    TempData["Message"] =fn.SuccessUpdate;

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
