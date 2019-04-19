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

        // GET: Providers
        public ActionResult Index()
        {
            var asing = db.AsingRolAndUsers.Where(a => a.Email == User.Identity.Name).FirstOrDefault();

            if (asing==null){
                asing = new Models.Globals.AsingRolAndUser {
                    AsingRolAndUserId=0,
                    CompanyId = 0,
                };

                TempData["Error"] = "Necesita que el usuario este registado en alguna empresa, Éste usuario no tiene permiso de Modificar esta opción. Por favor comuniquese con el administrador";
                TempData["Valid"] = true;
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
                TempData["Error"] = "No se envión ID...";
                return RedirectToAction("Index");
            }
            Provider provider = db.Providers.Find(id);
            if (provider == null)
            {
                TempData["Error"] = "No existe regitro con este ID...";
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
                URL = "~/Content/Logos/Empresa/otro.png",
            };

            ViewBag.CityId = new SelectList(CombosHelpers.GetCities(0, 0), "CityId", "Name");
            ViewBag.CountryId = new SelectList(CombosHelpers.GetCountries(), "CountryId", "Name");
            ViewBag.IvaConditionId = new SelectList(CombosHelpers.GetIvaConditions(), "IvaConditionId", "Name");
            ViewBag.ProvinceId = new SelectList(CombosHelpers.GetProvinces(0), "ProvinceId", "Name");
            return View(provider);
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
                provider.Logo = "~/Content/Logos/Empresa/otro.png";

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
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, response.Message);
            }

            TempData["Error"] = "Modelo no válido";

            ViewBag.CityId = new SelectList(CombosHelpers.GetCities(provider.CountryId, provider.ProvinceId), "CityId", "Name", provider.CityId);
            ViewBag.CountryId = new SelectList(CombosHelpers.GetCountries(), "CountryId", "Name", provider.CountryId);
            ViewBag.IvaConditionId = new SelectList(CombosHelpers.GetIvaConditions(), "IvaConditionId", "Name", provider.IvaConditionId);
            ViewBag.ProvinceId = new SelectList(CombosHelpers.GetProvinces(provider.CountryId), "ProvinceId", "Name", provider.ProvinceId);

            return View(provider);
        }

        // GET: Providers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "No se envión ID...";
                return RedirectToAction("Index");
            }
            Provider provider = db.Providers.Find(id);
            if (provider == null)
            {
                TempData["Error"] = "No existe regitro con este ID...";
                return RedirectToAction("Index");
            }

            provider.URL = provider.Logo;

            ViewBag.CityId = new SelectList(CombosHelpers.GetCities(provider.CountryId, provider.ProvinceId), "CityId", "Name", provider.CityId);
            ViewBag.CountryId = new SelectList(CombosHelpers.GetCountries(), "CountryId", "Name", provider.CountryId);
            ViewBag.IvaConditionId = new SelectList(CombosHelpers.GetIvaConditions(), "IvaConditionId", "Name", provider.IvaConditionId);
            ViewBag.ProvinceId = new SelectList(CombosHelpers.GetProvinces(provider.CountryId), "ProvinceId", "Name", provider.ProvinceId);

            return View(provider);
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
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, response.Message);
            }

            TempData["Error"] = "Modelo no válido";

            ViewBag.CityId = new SelectList(CombosHelpers.GetCities(provider.CountryId, provider.ProvinceId), "CityId", "Name", provider.CityId);
            ViewBag.CountryId = new SelectList(CombosHelpers.GetCountries(), "CountryId", "Name", provider.CountryId);
            ViewBag.IvaConditionId = new SelectList(CombosHelpers.GetIvaConditions(), "IvaConditionId", "Name", provider.IvaConditionId);
            ViewBag.ProvinceId = new SelectList(CombosHelpers.GetProvinces(provider.CountryId), "ProvinceId", "Name", provider.ProvinceId);

            return View(provider);
        }

        // GET: Providers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "No se envión ID...";
                return RedirectToAction("Index");
            }
            Provider provider = db.Providers.Find(id);
            if (provider == null)
            {
                TempData["Error"] = "No existe regitro con este ID...";
                return RedirectToAction("Index");
            }
            return View(provider);
        }

        // POST: Providers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Provider provider = db.Providers.Find(id);
            db.Providers.Remove(provider);
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
