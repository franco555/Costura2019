using PPSale.Classes;
using PPSale.Models.Conexion;
using PPSale.Models.Departure;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace PPSale.Controllers.Departure
{
    public class ClientsController : Controller
    {
        private ConexionContext db = new ConexionContext();

        // GET: Clients
        public ActionResult Index()
        {
            var asing = db.AsingRolAndUsers.Where(a => a.Email == User.Identity.Name).FirstOrDefault();

            if (asing == null)
            {
                asing = new Models.Globals.AsingRolAndUser
                {
                    AsingRolAndUserId = 0,
                    CompanyId = 0,
                };

                TempData["Error"] = "Necesita que el usuario este registado en alguna empresa, Éste usuario no tiene permiso de Modificar esta opción. Por favor comuniquese con el administrador";
                TempData["Valid"] = true;
            }

            var clients = db.Clients.
                Include(c => c.City).
                Include(c => c.Company).
                Include(c => c.Country).
                Include(c => c.IvaCondition).
                Include(c => c.Province).
                Where(c=>c.CompanyId==asing.CompanyId).
                OrderBy(c=>c.Name).
                ToList();
            return View(clients);
        }

        // GET: Clients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "No se envión ID...";
                return RedirectToAction("Index");
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                TempData["Error"] = "No existe regitro con este ID...";
                return RedirectToAction("Index");
            }
            return View(client);
        }

        // GET: Clients/Create
        public ActionResult Create()
        {
            var asing = db.AsingRolAndUsers.Where(a => a.Email == User.Identity.Name).FirstOrDefault();
            var client = new Client
            {
                CompanyId = asing.CompanyId,
                FirstDate = DateTime.Now,
                URL = "~/Content/Logos/Empresa/otro.png",
            };

            ViewBag.CityId = new SelectList(CombosHelpers.GetCities(0, 0), "CityId", "Name");
            ViewBag.CountryId = new SelectList(CombosHelpers.GetCountries(), "CountryId", "Name");
            ViewBag.IvaConditionId = new SelectList(CombosHelpers.GetIvaConditions(), "IvaConditionId", "Name");
            ViewBag.ProvinceId = new SelectList(CombosHelpers.GetProvinces(0), "ProvinceId", "Name");

            return View(client);
        }

        // POST: Clients/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Client client)
        {
            if (ModelState.IsValid)
            {
                client.Logo = "~/Content/Logos/Empresa/otro.png";

                db.Clients.Add(client);

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    if (client.LogoFile != null)
                    {
                        var folder = "~/Content/Logos/Client";
                        var Name = string.Format("{0}.jpg", client.ClientId);
                        var Succ = FilesHelpers.UploadPhoto(client.LogoFile, folder, Name);
                        if (Succ)
                        {
                            var pic = string.Format("{0}/{1}", folder, Name);
                            client.Logo = pic;

                            db.Entry(client).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, response.Message);
            }

            TempData["Error"] = "Modelo no válido";

            ViewBag.CityId = new SelectList(CombosHelpers.GetCities(client.CountryId, client.ProvinceId), "CityId", "Name", client.CityId);
            ViewBag.CountryId = new SelectList(CombosHelpers.GetCountries(), "CountryId", "Name", client.CountryId);
            ViewBag.IvaConditionId = new SelectList(CombosHelpers.GetIvaConditions(), "IvaConditionId", "Name", client.IvaConditionId);
            ViewBag.ProvinceId = new SelectList(CombosHelpers.GetProvinces(client.CountryId), "ProvinceId", "Name", client.ProvinceId);

            return View(client);
        }

        // GET: Clients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "No se envión ID...";
                return RedirectToAction("Index");
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                TempData["Error"] = "No existe regitro con este ID...";
                return RedirectToAction("Index");
            }

            client.URL = client.Logo;

            ViewBag.CityId = new SelectList(CombosHelpers.GetCities(client.CountryId, client.ProvinceId), "CityId", "Name", client.CityId);
            ViewBag.CountryId = new SelectList(CombosHelpers.GetCountries(), "CountryId", "Name", client.CountryId);
            ViewBag.IvaConditionId = new SelectList(CombosHelpers.GetIvaConditions(), "IvaConditionId", "Name", client.IvaConditionId);
            ViewBag.ProvinceId = new SelectList(CombosHelpers.GetProvinces(client.CountryId), "ProvinceId", "Name", client.ProvinceId);

            return View(client);
        }

        // POST: Clients/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Client client)
        {
            if (ModelState.IsValid)
            {
                db.Entry(client).State = EntityState.Modified;

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    if (client.LogoFile != null)
                    {
                        var folder = "~/Content/Logos/Client";
                        var Name = string.Format("{0}.jpg", client.ClientId);
                        var Succ = FilesHelpers.UploadPhoto(client.LogoFile, folder, Name);
                        if (Succ)
                        {
                            var pic = string.Format("{0}/{1}", folder, Name);
                            client.Logo = pic;

                            db.Entry(client).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, response.Message);
            }

            TempData["Error"] = "Modelo no válido";

            ViewBag.CityId = new SelectList(CombosHelpers.GetCities(client.CountryId, client.ProvinceId), "CityId", "Name", client.CityId);
            ViewBag.CountryId = new SelectList(CombosHelpers.GetCountries(), "CountryId", "Name", client.CountryId);
            ViewBag.IvaConditionId = new SelectList(CombosHelpers.GetIvaConditions(), "IvaConditionId", "Name", client.IvaConditionId);
            ViewBag.ProvinceId = new SelectList(CombosHelpers.GetProvinces(client.CountryId), "ProvinceId", "Name", client.ProvinceId);

            return View(client);
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "No se envión ID...";
                return RedirectToAction("Index");
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                TempData["Error"] = "No existe regitro con este ID...";
                return RedirectToAction("Index");
            }
            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Client client = db.Clients.Find(id);
            db.Clients.Remove(client);
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
