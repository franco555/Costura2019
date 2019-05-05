using Newtonsoft.Json;
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
        private FunctionHelpers fn = new FunctionHelpers();

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

                TempData["Error"] = "Error";
                TempData["Message"] = fn.notRegistre;
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



        // GET: Clients/Create
        public ActionResult Create()
        {
            var asing = db.AsingRolAndUsers.Where(a => a.Email == User.Identity.Name).FirstOrDefault();
            var client = new Client
            {
                CompanyId = asing.CompanyId,
                FirstDate = DateTime.Now,
                URL = fn.notCompany,
            };

            ViewBag.CityId = new SelectList(CombosHelpers.GetCities(0, 0), "CityId", "Name");
            ViewBag.CountryId = new SelectList(CombosHelpers.GetCountries(), "CountryId", "Name");
            ViewBag.IvaConditionId = new SelectList(CombosHelpers.GetIvaConditions(), "IvaConditionId", "Name");
            ViewBag.ProvinceId = new SelectList(CombosHelpers.GetProvinces(0), "ProvinceId", "Name");

            return PartialView(client);
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
                client.Logo = fn.notCompany;

                db.Clients.Add(client);

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    if (client.LogoFile != null)
                    {
                        var folder = "Client";
                        var Succ = FilesHelpers.UploadPhoto(client.LogoFile, folder, "",true);
                        if (Succ.Succeded)
                        {
                            client.Logo = Succ.Message;

                            db.Entry(client).State = EntityState.Modified;
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

        // GET: Clients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["Action"] = "Object";
                TempData["Message"] = fn.notId;

                return RedirectToAction("Index");
            }

            var client = db.Clients.Find(id);
            if (client == null)
            {
                TempData["Action"] = "Object";
                TempData["Message"] = fn.notExist;

                return RedirectToAction("Index");
            }

            client.URL = client.Logo;

            ViewBag.CityId = new SelectList(CombosHelpers.GetCities(client.CountryId, client.ProvinceId), "CityId", "Name", client.CityId);
            ViewBag.CountryId = new SelectList(CombosHelpers.GetCountries(), "CountryId", "Name", client.CountryId);
            ViewBag.IvaConditionId = new SelectList(CombosHelpers.GetIvaConditions(), "IvaConditionId", "Name", client.IvaConditionId);
            ViewBag.ProvinceId = new SelectList(CombosHelpers.GetProvinces(client.CountryId), "ProvinceId", "Name", client.ProvinceId);

            return PartialView(client);
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
                        var folder = "Client";
                        var Succ = FilesHelpers.UploadPhoto(client.LogoFile, folder, client.Logo,false);
                        if (Succ.Succeded)
                        {
                            client.Logo = Succ.Message;

                            db.Entry(client).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                    }
                    TempData["Action"] = "Success";
                    TempData["Message"] = fn.SuccessUpdate;

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
