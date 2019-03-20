using PPSale.Models.Conexion;
using PPSale.Models.Entry;
using PPSale.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PPSale.Controllers.Nav
{
    public class NavsController : Controller
    {
        private ConexionContext db = new ConexionContext();

        /*****************************************************************************
         * ****************         ***********************************
         * ******************************************************************************/

        // GET: Adicional
        public ActionResult Additional()
        {
            return View();
        }

        // GET: Configuracion
        public ActionResult Configuration()
        {
            return View();
        }
        // GET: Salida
        public ActionResult Departure()
        {
            return View();
        }
        // GET: Entrada
        
        public ActionResult Entry(int? id)
        {
            var month =(id==null) ?DateTime.Now.Month:id;
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



            var DayOne = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var DayEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            var Product = ReportMonth(DayOne, DayEnd,asing.CompanyId);
            var MoneyMonth = ReportYear(DateTime.Now.Year, asing.CompanyId);
            var docEntry =documentEntry(month, asing.CompanyId);

            var viewNavEntry = new ViewNavEntry
            {
                Date = DateTime.Now.AddDays(-7),
                Date2 = DateTime.Now,
                ProductQuantities = Product,
                MoneyMonths = MoneyMonth,
                DocumentEntry = docEntry,
                TotalDinero = Product.Sum(p => p.Price),
                TotalKilo = Product.Sum(p => p.Quantity),
            };

            return View(viewNavEntry);
        }

            
        // GET: Grafico
        public ActionResult Graphic()
        {
            return View();
        }
        // GET: Reporte
        public ActionResult Report()
        {
            return View();
        }
        // GET: Ahorro
        public ActionResult Saving()
        {
            return View();
        }
        // GET: Almacen
        public ActionResult Warehouse()
        {
            return View();
        }

        /*****************************************************************************
         * ****************    Configuracion     ***********************************
         * ******************************************************************************/

        //Pertenece a: ActionResult Entry
        public List<ViewMoneyMonth> ReportYear(int anio,int idc)
        {
            string[] Months = { "", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre", };

            var Stock = (from dde in db.DocumentEntries
                         where dde.Date.Year == anio &&dde.CompanyId ==idc
                         group dde by new { dde.Date.Month } into stock
                         select new
                         {
                             Fecha = stock.Key.Month,
                             Dinero = stock.Sum(x => x.PriceTotal),
                         })
            .ToList();


            var product = new List<ViewMoneyMonth>();
            foreach (var item in Stock)
            {
                product.Add(new ViewMoneyMonth() {ViewMoneyMonthId=item.Fecha ,Name = Months[item.Fecha], Price = item.Dinero });
            }

            return product.ToList();
        }
        //Pertenece a: ActionResult Entry
        public List<ViewProductQuantity> ReportMonth(DateTime desde, DateTime hasta,int idc)
        {
            //var uni = db.DerivativeProducts.ToList();

            
            var Stock = (from ddde in db.DocumentEntryDetails
                         join proCla in db.ProductClassifications on ddde.ProductClassificationId equals proCla.ProductClassificationId
                         join prod in db.Products on proCla.ProductId equals prod.ProductId
                         join proc in db.Classifications on proCla.ClassificationId equals proc.ClassificationId
                         join dde in db.DocumentEntries on ddde.DocumentEntryId equals dde.DocumentEntryId
                         where dde.Date >= desde && dde.Date <= hasta && dde.CompanyId==idc
                         group ddde by new { proCla.Product, proCla.Classification } into stock
                         select new
                         {
                             Product = stock.Key.Product.Name + " " + stock.Key.Classification.Name,
                             stocks = stock.Sum(x => x.Quantity),
                             Price = stock.Sum(x => x.Price * x.Quantity),
                         })
            .ToList();


            var product = new List<ViewProductQuantity>();
            foreach (var item in Stock)
            {
                product.Add(new ViewProductQuantity() { Name = item.Product, Quantity = item.stocks, Price = item.Price });
            }

            return product.ToList();
            
        }
        //Pertenece a: ActionResult Entry
        public List<DocumentEntry> documentEntry(int? month, int companyId)
        {
            var dEntry = db.DocumentEntries.Where(d=>d.CompanyId==companyId).OrderByDescending(d => d.Date).FirstOrDefault();
            //var dentry = db.DocumentEntries.Where(d => d.CompanyId == companyId).OrderByDescending(d => d.Date).First().Date;

            if (dEntry == null)
            {
                return null;
            }

            if (month > dEntry.Date.Month)
            {
                month = dEntry.Date.Month;
            }

            var docEnty = db.DocumentEntries.
                Where(s => s.Date.Month == month && s.CompanyId== companyId).
                OrderByDescending(s=>s.Date).
                ToList();

            return docEnty;
        }
    }
}