using PPSale.Classes;
using PPSale.Models.Conexion;
using PPSale.Models.Globals;
using PPSale.Models.View;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace PPSale.Controllers.Globals
{
    [Authorize(Roles = "Admin")]

    public class CountriesController : Controller
    {
        private ConexionContext db = new ConexionContext();

        // GET: Countries
        public ActionResult Index()
        {
            var countries = db.Countries.OrderBy(c => c.Name).ToList();
            return View(countries);
        }

        // GET: Countries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "No se envión ID...";

                return RedirectToAction("Index");
            }
            var country = db.Countries.Find(id);
            if (country == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "No existe regitro con este ID...";

                return RedirectToAction("Index");
            }
            return View(country);
        }

        // GET: Countries/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: Countries/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Country country)
        {
            if (ModelState.IsValid)
            {
                db.Countries.Add(country);

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    TempData["Action"] = "Success";
                    TempData["Message"] = "Guardado Exitosamente!!!";

                    return RedirectToAction("Index");
                }

                TempData["Action"] = "Error";
                TempData["Message"] =$"{country.Name }: {response.Message}";
            }
            else
            {
                TempData["Action"] = "Warning";
                TempData["Message"] = "Campos vacíos!!!";
            }

            return RedirectToAction("Index");
        }

        // GET: Countries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "No se envión ID...";

                return RedirectToAction("Index");
            }
            var country = db.Countries.Find(id);
            if (country == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "No existe regitro con este ID...";

                return RedirectToAction("Index");
            }
            return PartialView(country);
        }

        // POST: Countries/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Country country)
        {
            if (ModelState.IsValid)
            {
                db.Entry(country).State = EntityState.Modified;

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    TempData["Action"] = "Success";
                    TempData["Message"] = "Actualizado Exitosamente!!!";

                    return RedirectToAction("Index");
                }

                TempData["Action"] = "Error";
                TempData["Message"] =$"{country.Name}: {response.Message}";
            }
            else
            {
                TempData["Action"] = "Warning";
                TempData["Message"] = "Campos vacíos!!!";
            }
            return RedirectToAction("Index");
        }

        /**************************************/
        /**************************************/
        /*********   Second Page        ******/
        /**************************************/
        /**************************************/

        [HttpGet]
        public ActionResult CountryWithProvinces(int? id)
        {
            if (id == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "No se envión ID...";

                return RedirectToAction("Index");
            }
            var country = db.Countries.Include(c=>c.Provinces).Where(c=>c.CountryId==id).FirstOrDefault();
            if (country == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "No existe regitro con este ID...";

                return RedirectToAction("Index");
            }

            var countryWithProvinces = new CountryWithProvincesViewModel()
            {
                ContryId = country.CountryId,
                Name=country.Name,
                provinces=country.Provinces
            };

            return View(countryWithProvinces);
        }

        [HttpGet]
        public ActionResult AddProvince(int? Id)
        {
            if (Id == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "No se envión ID...";

                return RedirectToAction("CountryWithProvinces","Countries", new { id = Id });
            }
            var country = db.Countries.Find(Id);
            if (country == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "No existe regitro con este ID...";

                return RedirectToAction("CountryWithProvinces", "Countries", new {id=country.CountryId });
            }

            var province = new Province()
            {
                CountryId = Id.Value
            };
            
            return PartialView(province);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProvince(Province province)
        {
            if (ModelState.IsValid)
            {
                db.Provinces.Add(province);

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    TempData["Action"] = "Success";
                    TempData["Message"] = "Guardado Exitosamente!!!";

                    return RedirectToAction("CountryWithProvinces", "Countries", new { id = province.CountryId });
                }

                TempData["Action"] = "Error";
                TempData["Message"] = response.Message;
            }
            else
            {
                TempData["Action"] = "Warning";
                TempData["Message"] = "Formulario con campos vacios!!";
            }

            return RedirectToAction("CountryWithProvinces", "Countries", new { id = province.CountryId });
        }

        [HttpGet]
        public ActionResult EditProvince(int? Idc , int? Idp)
        {
            if (Idc == null || Idp==null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "No se los IDs de País y provincia...";

                return RedirectToAction("CountryWithProvinces", "Countries", new { id = Idc });
            }
            var province = db.Provinces.Find(Idp);
            if (province == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "No existe regitro con este ID...";

                return RedirectToAction("CountryWithProvinces", "Countries", new { id = Idc });
            }
            return PartialView(province);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProvince(Province province)
        {
            if (ModelState.IsValid)
            {
                db.Entry(province).State = EntityState.Modified;

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    TempData["Action"] = "Succes";
                    TempData["Message"] = "Actualizado Exitosamente!!!";

                    return RedirectToAction("CountryWithProvinces", "Countries", new { id = province.CountryId });
                }

                TempData["Action"] = "Error";
                TempData["Message"] = response.Message;
            }
            else
            {
                TempData["Action"] = "Warning";
                TempData["Message"] = "Formulario con campos vacios!!";
            }

            return RedirectToAction("CountryWithProvinces", "Countries", new { id = province.CountryId });
        }

        /**************************************/
        /**************************************/
        /*********   Third Page        ******/
        /**************************************/
        /**************************************/

        [HttpGet]
        public ActionResult ProvinceWithCities(int? Idc, int? Idp)
        {
            if (Idc == null || Idp==null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "No se envión ID...";

                return RedirectToAction("CountryWithProvinces",new {id= Idc});
            }
            var province = db.Provinces.Include(c => c.Cities).Where(c => c.ProvinceId ==Idp).FirstOrDefault();
            if (province == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "No existe regitro con este ID...";

                return RedirectToAction("CountryWithProvinces", new { id = Idc });
            }

            var provinceWithCitites = new ProvinceWithCititesViewModel()
            {
                ContryId = province.CountryId,
                ProvinceId = province.ProvinceId,
                Name = province.Name,
                cities = province.Cities
            };

            return View(provinceWithCitites);
        }

        [HttpGet]
        public ActionResult AddCity(int? idc, int? idp)
        {
            if (idc == null || idp==null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "No se envión ID...";

                return RedirectToAction("ProvinceWithCities", "Countries", new { Idc = idc, Idp=idp });
            }
            var province = db.Provinces.Find(idp);
            if (province == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "No existe regitro con este ID...";

                return RedirectToAction("ProvinceWithCities", "Countries", new { Idc = idc, Idp = idp });
            }

            var city = new City()
            {
                CountryId = idc.Value,
                ProvinceId=idp.Value
            };

            return PartialView(city);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCity(City city)
        {
            if (ModelState.IsValid)
            {
                db.Cities.Add(city);

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    TempData["Action"] = "Success";
                    TempData["Message"] = "Guardado Exitosamente!!!";

                    return RedirectToAction("ProvinceWithCities", "Countries", new {Idc=city.CountryId ,Idp = city.ProvinceId });
                }

                TempData["Action"] = "Error";
                TempData["Message"] = response.Message;
            }
            else
            {
                TempData["Action"] = "Warning";
                TempData["Message"] = "Formulario con campos vacios!!";
            }

            return RedirectToAction("ProvinceWithCities", "Countries", new { Idc = city.CountryId, Idp = city.ProvinceId });
        }

        [HttpGet]
        public ActionResult EditCity(int? idc, int? idp, int? idci)
        {
            if (idc == null || idp == null || idci==null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "No se recibieron los IDs de País y provincia...";

                return RedirectToAction("ProvinceWithCities", "Countries", new { Idc = idc, Idp = idp });
            }
            var city = db.Cities.Find(idci);
            if (city == null)
            {
                TempData["Action"] = "Error";
                TempData["Message"] = "No existe regitro con este ID...";

                return RedirectToAction("ProvinceWithCities", "Countries", new { Idc = idc, Idp = idp });
            }
            return PartialView(city);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCity(City city)
        {
            if (ModelState.IsValid)
            {
                db.Entry(city).State = EntityState.Modified;

                var response = DBHelpers.SaveChage(db);
                if (response.Succeded)
                {
                    TempData["Action"] = "Succes";
                    TempData["Message"] = "Actualizado Exitosamente!!!";

                    return RedirectToAction("ProvinceWithCities", "Countries", new { Idc=city.CountryId,Idp = city.ProvinceId });
                }

                TempData["Action"] = "Error";
                TempData["Message"] = response.Message;
            }
            else
            {
                TempData["Action"] = "Warning";
                TempData["Message"] = "Formulario con campos vacios!!";
            }

            return RedirectToAction("ProvinceWithCities", "Countries", new { Idc = city.CountryId, Idp = city.ProvinceId });
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
